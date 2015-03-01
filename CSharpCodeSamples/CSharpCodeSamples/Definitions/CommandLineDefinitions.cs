namespace CSharpCodeSamples.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using Newtonsoft.Json;

    using Common.Interfaces.Models.Definitions;
    using Configuration;

    [JsonObject][Serializable]
    public class CommandLineDefinitions : ICommandLineDefinitions, ICommandLineData
    {
        //* Scope Entity Name Definitions *
        private readonly Dictionary<string, IEntityTypeBrief>  _entityAliases;     //alias, (name,flags)
        //* Scope Definitions *
        private readonly Dictionary<string, IEntityDefinition> _entityDefinitions; //entity, definitions

        public Dictionary<string, IEntityTypeBrief>  EntityAliases     { get { return _entityAliases; } }
        public Dictionary<string, IEntityDefinition> EntityDefinitions { get { return _entityDefinitions; } }

        public CommandLineDefinitions()
        {
            _entityAliases     = new Dictionary<string, IEntityTypeBrief>();   //alias, (name,flags)
            _entityDefinitions = new Dictionary<string, IEntityDefinition>();  //entity name, def

            LoadEntityAliases();
            LoadEntityFieldDefinitions();
        }

        /// <summary>
        /// Determine if the provided entity type name is valid.
        /// </summary>
        /// <param name="entityTypeName">Name of the entity type.</param>
        /// <returns>true if Entity Type exists in configuration, false otherwise.</returns>
        public bool DoesEntityTypeExist(string entityTypeName)
        {
            return _entityAliases.Values.Any(i => i.Name == entityTypeName);
        }
        /// <summary>
        /// Determine if the provided entity type alias is valid.
        /// </summary>
        /// <param name="entityAlias">The entity alias name.</param>
        /// <returns>true if the Entity Type Alias exists in the configuration, false otherwise</returns>
        public bool DoesEntityTypeAliasExist(string entityAlias)
        {
            return _entityAliases.ContainsKey(entityAlias);
        }
        /// <summary>
        /// Returns Entity specific configuration for the specified entity type.
        /// </summary>
        /// <param name="entityTypeName">Name of the entity type.</param>
        /// <returns><see cref="IEntityDefinition"/> definitions for the specified entity type.</returns>
        /// <exception cref="ArgumentException">Invalid entity type name specified.  This indicates that the entity type name does not exist within the configuration.</exception>
        public IEntityDefinition ForEntityType(string entityTypeName)
        {
            if (!DoesEntityTypeExist(entityTypeName) ||
                !_entityDefinitions.ContainsKey(entityTypeName))
            {
                throw new ArgumentException("Invalid entity type name specified", "entityTypeName");
            }
            return _entityDefinitions[entityTypeName];
        }
        /// <summary>
        /// Attempts to parse a supplied alias name into a valid entity name.
        /// </summary>
        /// <param name="aliasName">The alias name of the entity type.</param>
        /// <param name="name">The valid entity type name.</param>
        /// <returns>true if the alias name was successfully parsed, false otherwise.</returns>
        public bool TryParseEntityTypeName(string aliasName, out string name)
        {
            Debug.Assert(aliasName.Length == 2);

            name = "";
            aliasName = aliasName.ToUpperInvariant();
            if (DoesEntityTypeAliasExist(aliasName))
            {
                name = _entityAliases[aliasName].Name;
                return true;
            }
            if (!DoesEntityTypeExist(aliasName)) return false;

            name = aliasName;
            return true;
        }

        private void LoadEntityAliases()
        {
            EntityDefinitionsSection configSection = (EntityDefinitionsSection)ConfigurationManager.GetSection("EntityDefinitions");
            foreach (EntityDefinitionElement ede in configSection.EntityDefinitions)
            {
                //Scope items should only always appear once, with a single unique alias
                if (DoesEntityTypeAliasExist(ede.Alias) ||
                    DoesEntityTypeExist(ede.Name))
                {
                    throw new ConfigurationErrorsException("Scope or Scope Alias is not unique in configuration file");
                }
                _entityAliases.Add(ede.Alias,
                                   new EntityTypeBrief
                                   {
                                       AliasName    = ede.Alias,
                                       Name         = ede.Name,
                                       PermName     = ede.PermName,
                                       IsAddable    = ede.IsAddable,
                                       IsDeletable  = ede.IsDeletable,
                                       MenuTemplate = ede.MenuTemplate
                                   });
            }
        }
        private void LoadEntityFieldDefinitions()
        {
            SearchFieldDefinitionsSection configSectionFieldDefinitions   = (SearchFieldDefinitionsSection)ConfigurationManager.GetSection("SearchFieldDefinitions");
            DisplayDefinitionsSection     configSectionDisplayDefinitions = (DisplayDefinitionsSection)ConfigurationManager.GetSection("DisplayDefinitions");
            Debug.WriteLine("Loading Command Line Definitions");

            foreach (SearchFieldDefinitionElement cde in configSectionFieldDefinitions.SearchFieldDefinitions)
            {
                if (!DoesEntityTypeExist(cde.EntityName))
                {
                    throw new ConfigurationErrorsException("Invalid entity type specified in configuration file");
                }
                IEntityTypeBrief  info       = _entityAliases.Values.First(v => v.Name == cde.EntityName);
                IEntityDefinition definition = new EntityDefinition(info);
                var configSectionDisplayDefinition = configSectionDisplayDefinitions.DisplayFieldDefinitions.GetDefinition(cde.EntityName);

                var i = 0;
                foreach (DisplayFieldDefinitionElement fd in configSectionDisplayDefinition.DisplayFields)
                {
                    IDisplayColumn_Build dc = new DisplayColumn();
                    dc.ColumnWidth     = fd.ColumnWidth;
                    dc.ColumnMinWidth  = fd.ColumnMinWidth;
                    dc.DataType        = fd.DataType;
                    dc.HeaderText      = fd.DisplayLabel;
                    dc.Key             = fd.Name;
                    dc.Template        = fd.ColumnTemplate;
                    dc.TextAlignHeader = fd.TextAlignHeader;
                    dc.TextAlign       = fd.TextAlign;
                    dc.IsHidden        = fd.IsHidden;
                    dc.IsUpdatable     = fd.IsUpdatable;
                    definition.DisplayColumnsSorted.Add(i++, (IDisplayColumn)dc);
                }

                foreach (FieldGroupDefinitionElement fe in cde.SearchFieldGroups)
                {
                    List<IFieldDefinition> fieldsForGroup = (from FieldDefinitionElement fde in fe.Fields
                                                             select new FieldDefinition
                                                             {
                                                                 DataType                     = DataTypeForFieldDataType(fe.DataType),
                                                                 IsCrossField                 = fde.IsCrossField,
                                                                 IsDefaultDate                = fde.IsDefaultDate,
                                                                 IsUpdatable                  = fde.IsUpdatable,
                                                                 Name                         = fde.Name,
                                                                 RootEntitySetExpression      = ServicesForParameterExpressions.GetParamExpressionForEntityType(GetEntityTypeForEntity(cde.EntityName)),
                                                                 EntityType                   = GetEntityTypeForEntity(cde.EntityName)
                                                             })
                                                             .ToList<IFieldDefinition>();
                    foreach (string aliasName in fe.AliasList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        IFieldGroupDefinition fgd = new FieldGroupDefinition
                        {
                            AliasName  = aliasName,
                            DataType   = DataTypeForFieldDataType(fe.DataType),
                            IsLongText = fe.IsLongText
                        };
                        fgd.FieldDefinitions.AddRange(fieldsForGroup);
                        definition.FieldGroups[fgd.AliasName] = fgd;
                    }
                }
                _entityDefinitions.Add(cde.EntityName, definition);
            }
        }

        private static Type DataTypeForFieldDataType(string fieldType)
        {
            switch (fieldType.ToUpperInvariant())
            {
                case "STRING":
                    return typeof(string);
                case "INT":
                    return typeof(int);
                case "DECIMAL":
                    return typeof(decimal);
                case "DATE":
                    return typeof(DateTime);
                case "BOOL":
                    return typeof(bool);
                default:
                    throw new ConfigurationErrorsException("Invalid data type specified in configuration");
            }
        }
        //Really don't love this
        private static Type GetEntityTypeForEntity(string entityName)
        {
            //Details have been removed as they were proprietary
            //However, you just need to provide the EFModel type for the specified string entityName
            //this can be dynamic or you could just use a switch statement and hard code things.
            //switch (entityName.ToUpperInvariant())
            //{
            //}
            throw new NotImplementedException();
        }
    }
}
