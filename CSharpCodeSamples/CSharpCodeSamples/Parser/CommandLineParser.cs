namespace CSharpCodeSamples.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using Definitions;
    using Messaging.Requests;

    using Common;
    using Common.Enumerations;
    using Common.Interfaces.Messaging.Requests;
    using Common.Interfaces.Messaging.Requests.Payloads;
    using Common.Interfaces.Models;
    using Common.Interfaces.Parser;
    using Common.Interfaces.Models.Definitions;
    using Domain.Models;
    using Messaging.Requests.Payloads;

    public class CommandLineParser : ICommandLineParser
    {
        private const string ACTIONTOKEN_DELETE = "DELETE";
        private const string ACTIONTOKEN_TO     = "TO";

        //private const char   OPERATOR_DEFAULT   = ' '; //Not used, but implied
        private const char   OPERATOR_ANY       = '*';
        private const char   OPERATOR_CONTAINS  = '~';
        private const char   OPERATOR_EQ        = '=';
        private const char   OPERATOR_GT        = '>';
        private const char   OPERATOR_LT        = '<';
        private const char   OPERATOR_NE        = '!';

        private readonly ICommandLineDefinitions _commandLineDefinitions;
        private readonly IParsedRequest          _parsedRequestBuilder;

        public ICommandLineData CommandLineData { get { return (ICommandLineData)_commandLineDefinitions; } }

        /// <summary>
        /// ctor for parser.  Input parameters should be provided by Unity.
        ///  </summary>
        /// <remarks>
        /// Note, ParsedRequest is not cloned and is not intended for re-use.
        /// create a new parser for each request, or modify so that parsed request can be re-used.
        /// </remarks>
        /// <param name="commandLineDefinitions">
        /// The fully loaded <seealso cref="CommandLineDefinitions"/>.
        /// </param>
        /// <param name="parsedRequestBuilder">
        /// A blank <seealso cref="ParsedRequest"/> which is loaded with the results of the parse.
        /// </param>
        public CommandLineParser(ICommandLineDefinitions commandLineDefinitions, IParsedRequest parsedRequestBuilder) {
            _commandLineDefinitions = commandLineDefinitions;
            _parsedRequestBuilder   = parsedRequestBuilder;
        }


        /// <summary>
        /// Accepts a user request, the main data point being a string containing the text to parse.
        /// Looks for individual space delimited content, field names (indicated by an abbrieviation followed by a colon)
        /// or payloads (actions indicated by parenthesis).
        /// Parses these out based upon definitions retrieved from config file and available through the command line definitions.
        /// <seealso cref="CommandLineDefinitions"/>
        /// </summary>
        /// <param name="userRequest">
        /// The loaded <seealso cref="ParsedRequest"/> to parse.
        /// </param>
        /// <returns>A populated <seealso cref="ParsedRequest"/> object containing the results of the parsing operation.</returns>
        public IParsedRequest ParseTheUserRequest(IUserRequest userRequest)
        {
            Debug.Assert(userRequest != null, "Command Line Parser: The user request was null");
            Debug.Assert(_commandLineDefinitions != null, "Command Line Parser: Command line definitions were null");

            ActionStage                 actionStage     = ActionStage.NotInAction;
            ActionUpdateStage           updateStage     = ActionUpdateStage.None;
            List<string>                parseErrors     = new List<string>();
            List<IPayload_Update_Build> workingPayloads = new List<IPayload_Update_Build>();

            char   currentChar       = Constants.DELIMITER_SPACE;
            bool   literalWhiteSpace = false;
            bool   inQuotes          = false;
            string parsedItem        = "";

            string requestedScope;
            if (!_commandLineDefinitions.TryParseEntityTypeName(userRequest.EntityName, out requestedScope))
            {
                return null;
            }

            IEntityDefinition currentDefinition = _commandLineDefinitions.ForEntityType(requestedScope);
            IParsedRequest parsedRequestBuilder = _parsedRequestBuilder.ForEntity(requestedScope, currentDefinition.CrossFieldTargets);

            IFieldGroupDefinition  currentFieldGroup    = currentDefinition.CrossFieldDefinition();
            SearchFieldOperators   currentFieldOperator = SearchFieldOperators.Default;
            List<IFieldDefinition> fieldsToSearch       = currentFieldGroup.FieldDefinitions;

            Queue<char> searchQueue = new Queue<char>(userRequest.CommandLine.ToCharArray());
            //loop through search string characters
            while (searchQueue.Count > 0)
            {
                //keep track of remaining text in working command line
                //string searchRemainder = userRequest.CommandLine.Substring(userRequest.CommandLine.Length - searchQueue.Count);

                char previousChar = currentChar; //keep track of prior character

                currentChar = searchQueue.Dequeue();  //get current character

                #region Deal with apostrophe delimiter
                if (currentChar == Constants.DELIMITER_QUOTE)
                {
                    if (!inQuotes &&
                        (previousChar == Constants.DELIMITER_SPACE ||
                         previousChar == Constants.DELIMITER_FIELDNAME ||
                         previousChar == OPERATOR_EQ ||
                         previousChar == OPERATOR_GT ||
                         previousChar == OPERATOR_LT ||
                         previousChar == OPERATOR_NE))
                    {
                        inQuotes = true;
                    }
                    else if (searchQueue.Peek() == Constants.DELIMITER_SPACE ||
                             searchQueue.Peek() == Constants.DELIMITER_CLOSEACTION)
                    {
                        inQuotes = false;
                        literalWhiteSpace = string.IsNullOrWhiteSpace(parsedItem);
                    }
                    continue;
                }

                if (!inQuotes) currentChar = char.ToUpperInvariant(currentChar);
                #endregion

                //*
                //* Process "normal" characters
                //*
                if (inQuotes ||
                    (currentChar >= 'A' && currentChar <= 'Z') ||
                    (currentChar >= '0' && currentChar <= '9') ||
                    currentChar == '-' ||
                    currentChar == '/' ||
                    currentChar == '\'')
                {
                    parsedItem += currentChar;
                    continue;
                }
                parsedItem = parsedItem.Trim();

                //*
                //* Look for action begin token
                //*
                if (currentChar == Constants.DELIMITER_OPENACTION &&
                    actionStage == ActionStage.NotInAction)
                {
                    actionStage = ActionStage.InferringAction;
                    continue;
                }

                //*
                //* Process possible operator
                //*
                SearchFieldOperators candidateOperation;
                if (actionStage == ActionStage.NotInAction &&
                    SearchOperationsTryParse(currentChar, out candidateOperation))
                {
                    currentFieldOperator = candidateOperation;
                }

                //*
                //* If currentItem is empty, ignore it.  This happens
                //*
                if (string.IsNullOrEmpty(parsedItem) &&
                    currentFieldOperator != SearchFieldOperators.AnyNotBlank && //Even if value is '', we still need to process if
                    currentChar != Constants.DELIMITER_CLOSEACTION &&           //it is fieldname:* or fieldname:'', or the end of an action
                    !literalWhiteSpace) continue;

                if (!string.IsNullOrWhiteSpace(parsedItem) &&
                    updateStage == ActionUpdateStage.FoundTOToken)
                {
                    updateStage = ActionUpdateStage.FoundTOValue;
                }

                //*
                //* Process field names (text immediately preceding colon represents specified fieldname).
                //*
                if (currentChar == Constants.DELIMITER_FIELDNAME)
                {
                    if (actionStage == ActionStage.InAction_Update &&
                        updateStage == ActionUpdateStage.SeekingToValue)
                    {
                        if (parsedItem == ACTIONTOKEN_TO)
                        {
                            updateStage = ActionUpdateStage.FoundTOToken;
                        }
                        else
                        {
                            actionStage = ActionStage.InAction_SkipToEnd;
                            parseErrors.Add("Unrecognized fieldname keyword.  Update action ignored.");
                        }
                    }
                    else
                    {
                        //Try to find a field group def for the corresponding field name
                        IFieldGroupDefinition candidateFGD = currentDefinition.WithSearchField(parsedItem);
                        //If we didn't find it, default to cross field
                        currentFieldGroup = candidateFGD ?? currentDefinition.CrossFieldDefinition();
                        //Get the default operator for the current field group
                        currentFieldOperator = OperationForTextField(currentFieldGroup);
                        //Update the fields that will be search for this parse item
                        fieldsToSearch = currentFieldGroup.FieldDefinitions;

                        //If we had to default to cross field, mention it.
                        if (candidateFGD == null)
                        {
                            parseErrors.Add(String.Format("The field name {0} is not recognized.  Searching across all order fields for the item.", parsedItem));
                        }
                    }

                    parsedItem = "";
                }

                if (actionStage == ActionStage.InAction_Update &&
                    updateStage == ActionUpdateStage.FoundTOToken) continue;

                //If it is the end of a value, or a fieldname:* or a fieldname:'' or the end of the action,
                //then keep on truckin, otherwise next character.
                if (currentChar != Constants.DELIMITER_SPACE &&
                    currentFieldOperator != SearchFieldOperators.AnyNotBlank &&
                    currentChar != Constants.DELIMITER_CLOSEACTION &&
                    !literalWhiteSpace) continue;

                //*
                //* Process Actions Here
                //*
                if (actionStage == ActionStage.InferringAction)
                {
                    switch (parsedItem)
                    {
                        case ACTIONTOKEN_DELETE:
                            if (currentDefinition.IsDeletable)
                            {
                                actionStage = ActionStage.InAction_Delete;
                            }
                            else
                            {
                                actionStage = ActionStage.InAction_SkipToEnd;
                                parseErrors.Add("Unauthorized delete action specified.  Action was ignored.");
                            }
                            continue;
                        default: //Update
                            if (currentFieldGroup.AliasName == "root")
                            {
                                parseErrors.Add("Update field name was missing or invalid.");
                                actionStage = ActionStage.InAction_SkipToEnd;
                                continue;
                            }
                            if (!currentFieldGroup.FieldDefinitions.Any(i => i.IsUpdatable))
                            {
                                parseErrors.Add("Unauthorized update action specified.  Cannot update specified field.  Action was ignored.");
                                actionStage = ActionStage.InAction_SkipToEnd;
                                continue;
                            }
                            actionStage = ActionStage.InAction_Update;
                            break;
                    }
                }

                DateTime candidateDate;
                decimal candidateDecimal;
                int candidateInt;
                IDynamicValue currentParsedValue = null;
                if (!currentFieldGroup.IsRoot)
                {
                    #region Process Field Specific Special Cases
                    if (currentFieldGroup.DataType == typeof(DateTime)) //Is current field a recognized date field
                    {
                        if (DateTime.TryParse(parsedItem, out candidateDate)) // Is the value a valid date?
                        {
                            currentParsedValue = new DynamicValue(candidateDate, currentFieldOperator);
                        }
                        else
                        {
                            parseErrors.Add(String.Format("Unable to recognize {0} as a valid date format.  Value was ignored.", parsedItem));
                            continue;
                        }
                    }
                    else if (currentFieldGroup.DataType == typeof(decimal)) //Is current field intended to be numeric?
                    {
                        if (decimal.TryParse(parsedItem, out candidateDecimal)) // Is the value a valid number?
                        {
                            currentParsedValue = new DynamicValue(candidateDecimal, currentFieldOperator);
                        }
                        else
                        {
                            parseErrors.Add(String.Format("Unable to recognize {0} as a valid number.  Value was ignored.", parsedItem));
                            continue;
                        }
                    }
                    else if (currentFieldGroup.DataType == typeof(int)) //Is current field intended to be numeric?
                    {
                        if (int.TryParse(parsedItem, out candidateInt)) // Is the value a valid number?
                        {
                            currentParsedValue = new DynamicValue(candidateInt, currentFieldOperator);
                        }
                        else
                        {
                            parseErrors.Add(String.Format("Unable to recognize {0} as a valid number.  Value was ignored.", parsedItem));
                            continue;
                        }
                    }
                    #endregion
                }
                else
                {
                    #region Process Cross Field Special Cases
                    if (DateTime.TryParse(parsedItem, out candidateDate)) //Is value a date?
                    {
                        if (currentDefinition.HasAnyFieldWithDefaultDate) //If there is a defined default date field, use it.
                        {
                            fieldsToSearch = new List<IFieldDefinition> { currentDefinition.DefaultDate };
                            currentParsedValue = new DynamicValue(candidateDate, currentFieldOperator);
                        }
                    }
                    else if (int.TryParse(parsedItem, out candidateInt))
                    {
                        if (currentDefinition.HasAnyFieldThatIsInt)
                        {
                            fieldsToSearch = currentDefinition.DefaultInts;
                            currentParsedValue = new DynamicValue(candidateInt, currentFieldOperator);
                        }
                    }
                    else if (decimal.TryParse(parsedItem, out candidateDecimal))
                    {
                        if (currentDefinition.HasAnyFieldThatIsDecimal)
                        {
                            fieldsToSearch = currentDefinition.DefaultDecimals;
                            currentParsedValue = new DynamicValue(candidateDecimal, currentFieldOperator);
                        }
                    }
                    #endregion
                }

                if (currentParsedValue == null)
                {
                    currentParsedValue = new DynamicValue(parsedItem, ((parsedItem == "" && currentFieldOperator != SearchFieldOperators.AnyNotBlank) ?
                                                                       SearchFieldOperators.Equal :
                                                                       currentFieldOperator));
                }
                //At this point we should have a valid parsed value. (value/operator combo)

                currentFieldOperator = OperationForTextField(currentFieldGroup);  //Reset field operator for next value (if any)
                literalWhiteSpace = false;
                parsedItem = "";

                //Deal with parsed data based upon action stage
                switch (actionStage)
                {
                    case ActionStage.InAction_Delete:
                        parsedRequestBuilder.AddPayload(new Payload_Delete()); //TODO?

                        break;
                    case ActionStage.InAction_Update:
                        switch (updateStage)
                        {
                            case ActionUpdateStage.None:
                                if (string.IsNullOrWhiteSpace(currentParsedValue.SearchValue))
                                {
                                    throw new Exception("Invalid value");
                                }
                                foreach (IFieldDefinition fd in currentFieldGroup.FieldDefinitions.Where(i => i.IsUpdatable))
                                {
                                    IPayload_Update_Build payload = new Payload_Update();
                                    payload.ExistingField = new SearchItem(fd, currentParsedValue);
                                    workingPayloads.Add(payload);
                                }
                                updateStage = ActionUpdateStage.SeekingToValue;
                                break;
                            case ActionUpdateStage.FoundTOValue:
                                if (string.IsNullOrWhiteSpace(currentParsedValue.SearchValue))
                                {
                                    throw new Exception("Invalid value");
                                }

                                foreach (IPayload_Update_Build p in workingPayloads)
                                {
                                    p.UpdateFieldValue = currentParsedValue;
                                }
                                updateStage = ActionUpdateStage.SeekingComments;
                                break;
                            case ActionUpdateStage.SeekingComments:
                                foreach (IPayload_Update_Build p in workingPayloads)
                                {
                                    p.Comments = currentParsedValue.SearchValue;
                                }
                                updateStage = ActionUpdateStage.AlmostDone;
                                break;
                        }
                        break;
                    case ActionStage.InAction_SkipToEnd:
                        break;
                    default: //Not in Action 
                        parsedRequestBuilder.AddParsedSearchValueToRequest(fieldsToSearch, currentParsedValue);
                        break;
                }

                if (actionStage != ActionStage.NotInAction &&
                    currentChar == Constants.DELIMITER_CLOSEACTION)
                {
                    if (actionStage == ActionStage.InAction_Delete ||
                        (actionStage == ActionStage.InAction_Update &&
                         updateStage >= ActionUpdateStage.FoundTOValue))
                    {
                        parsedRequestBuilder.AddPayloads(workingPayloads.Cast<IPayload_Update>());  //Validation Performed by Request
                        workingPayloads.Clear();
                    }
                    actionStage = ActionStage.NotInAction;
                    updateStage = ActionUpdateStage.None;
                    currentFieldGroup = currentDefinition.CrossFieldDefinition();
                    fieldsToSearch = currentFieldGroup.FieldDefinitions;
                    Debug.WriteLine("Leaving Action");
                }
            }

            if ((actionStage == ActionStage.InAction_Delete ||
                (actionStage == ActionStage.InAction_Update &&
                 updateStage >= ActionUpdateStage.FoundTOValue)) &&
                workingPayloads.Any())
            {
                if (updateStage == ActionUpdateStage.SeekingComments &&
                    !string.IsNullOrWhiteSpace(parsedItem))
                {
                    parsedItem = parsedItem.Trim();
                    if (parsedItem.EndsWith(Constants.DELIMITER_CLOSEACTION.ToString(CultureInfo.InvariantCulture)))
                    {
                        parsedItem = parsedItem.TrimEnd(Constants.DELIMITER_CLOSEACTION);
                    }
                    foreach (IPayload_Update_Build p in workingPayloads)
                    {
                        p.Comments = parsedItem;
                    }
                }
                parsedRequestBuilder.AddPayloads(workingPayloads.Cast<IPayload_Update>());  //Validation Performed by Request
            }
            parsedRequestBuilder.ParseErrors.AddRange(parseErrors);
            return parsedRequestBuilder;
        }


        private SearchFieldOperators OperationForTextField(IFieldGroupDefinition fieldGroup)
        {
            return (fieldGroup != null && fieldGroup.IsLongText) ? SearchFieldOperators.Contains : SearchFieldOperators.Default;
        }
        private bool SearchOperationsTryParse(char c, out SearchFieldOperators result)
        {
            switch (c)
            {
                case OPERATOR_ANY:
                    result = SearchFieldOperators.AnyNotBlank;
                    break;
                case OPERATOR_CONTAINS:
                    result = SearchFieldOperators.Contains;
                    break;
                case OPERATOR_EQ:
                    result = SearchFieldOperators.Equal;
                    break;
                case OPERATOR_GT:
                    result = SearchFieldOperators.GreaterThan;
                    break;
                case OPERATOR_LT:
                    result = SearchFieldOperators.LessThan;
                    break;
                case OPERATOR_NE:
                    result = SearchFieldOperators.NotEqual;
                    break;
                default:
                    result = SearchFieldOperators.Default;
                    return false;
            }
            return true;
        }
    }


    internal class ParseContext
    {
        public string CommandLine { get; private set; }
        public IFieldGroupDefinition FieldGroup { get; set; }
        public SearchFieldOperators FieldOperator { get; set; }
        public List<string> ParsingErrors { get; private set; }
        public IEntityDefinition EntityDefinition { get; private set; }



        internal ParseContext(IEntityDefinition definition, string commandLine)
        {
            ParsingErrors = new List<string>();

            CommandLine = commandLine;
            EntityDefinition = definition;
            FieldGroup = definition.CrossFieldDefinition();
            FieldOperator = SearchFieldOperators.Default;

        }
    }
}
