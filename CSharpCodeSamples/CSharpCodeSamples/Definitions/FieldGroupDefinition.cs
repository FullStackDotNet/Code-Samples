namespace CSharpCodeSamples.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    using Common.Interfaces.Models.Definitions;

    [Serializable]
    internal class FieldGroupDefinition : IFieldGroupDefinition
    {
        private string _aliasName;

        public string AliasName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_aliasName))
                    throw new ConfigurationErrorsException("Access to uninitialized field group definition");
                return _aliasName;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(value, "Invalid alias name specified");
                _aliasName = value;
            }
        }

        public Type   DataType        { get; set; }
        public string DataTypeSimple  { get { return DataType.ToString().Replace("System.", ""); } }
        public bool   IsLongText      { get; set; }
        public bool   IsRoot          { get { return AliasName == "root"; } }

        public List<IFieldDefinition> FieldDefinitions { get; private set; }

        public FieldGroupDefinition()
        {
            FieldDefinitions = new List<IFieldDefinition>();
        }
    }
}
