namespace CSharpCodeSamples.Definitions.Configuration
{
    using System.Configuration;
    using System.Linq;

    public class DisplayDefinitionsSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true, IsKey = false, IsRequired = true)]
        public DisplayEntityFieldDefinitionCollection DisplayFieldDefinitions
        {
            get { return (DisplayEntityFieldDefinitionCollection)this[""]; }
            set { this[""] = value; }
        }
    }

    public class DisplayEntityFieldDefinitionCollection : ConfigurationElementCollection
    {
        /// <exception cref="ConfigurationErrorsException">Missing display definition in configuration file</exception>
        public DisplayEntityFieldDefinitionElement GetDefinition(string entityName)
        {
            if (BaseGetAllKeys().All(item => item.ToString() != entityName))
            {
                throw new ConfigurationErrorsException("Missing display definition in configuration file");
            }
            return (DisplayEntityFieldDefinitionElement)BaseGet(entityName);
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new DisplayEntityFieldDefinitionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DisplayEntityFieldDefinitionElement)element).EntityName;
        }

        protected override bool IsElementName(string elementName)
        {
            return !string.IsNullOrWhiteSpace(elementName) && elementName.ToUpperInvariant() == "DISPLAYENTITYFIELDDEFINITION";
        }

        public DisplayFieldDefinitionElement this[int index] { get { return BaseGet(index) as DisplayFieldDefinitionElement; } }
        protected override string ElementName { get { return "DisplayEntityFieldDefinition"; } }
    }

    public class DisplayEntityFieldDefinitionElement : ConfigurationElement
    {
        [ConfigurationProperty("entityname", IsRequired = true, IsKey = true)]
        public string EntityName
        {
            get { return this["entityname"].ToString(); }
            set { this["entityname"] = value; }
        }

        [ConfigurationProperty("", IsDefaultCollection = true, IsKey = false, IsRequired = true)]
        public DisplayFieldDefinitionCollection DisplayFields
        {
            get { return (DisplayFieldDefinitionCollection)this[""]; }
            set { this[""] = value; }
        }
    }

    public class DisplayFieldDefinitionCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new DisplayFieldDefinitionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DisplayFieldDefinitionElement)element).Name;
        }

        protected override bool IsElementName(string elementName)
        {
            return !string.IsNullOrWhiteSpace(elementName) && elementName.ToUpperInvariant() == "DISPLAYFIELDDEFINITION";
        }


        public DisplayFieldDefinitionElement this[int index] { get { return BaseGet(index) as DisplayFieldDefinitionElement; } }
        protected override string ElementName { get { return "DisplayFieldDefinition"; } }
    }

    public class DisplayFieldDefinitionElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return this["name"].ToString(); }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("datatype", IsRequired = false, DefaultValue = "string")]
        public string DataType
        {
            get { return this["datatype"].ToString(); }
            set { this["datatype"] = value; }
        }

        [ConfigurationProperty("ishidden", IsRequired = false, DefaultValue = false)]
        public bool IsHidden
        {
            get { return (bool)this["ishidden"]; }
            set { this["ishidden"] = value; }
        }

        [ConfigurationProperty("textalign", IsRequired = false, DefaultValue = "left")]
        public string TextAlign
        {
            get { return this["textalign"].ToString(); }
            set { this["textalign"] = value; }
        }

        [ConfigurationProperty("textalignheader", IsRequired = false, DefaultValue = "left")]
        public string TextAlignHeader
        {
            get { return this["textalignheader"].ToString(); }
            set { this["textalignheader"] = value; }
        }

        [ConfigurationProperty("displaylabel", IsRequired = true)]
        public string DisplayLabel
        {
            get { return this["displaylabel"].ToString(); }
            set { this["displaylabel"] = value; }
        }

        [ConfigurationProperty("width", IsRequired = false, DefaultValue = "")]
        public string ColumnWidth
        {
            get { return this["width"].ToString(); }
            set { this["width"] = value; }
        }

        [ConfigurationProperty("minwidth", IsRequired = false, DefaultValue = "")]
        public string ColumnMinWidth
        {
            get { return this["minwidth"].ToString(); }
            set { this["minwidth"] = value; }
        }

        [ConfigurationProperty("columntemplate", IsRequired = false, DefaultValue = "")]
        public string ColumnTemplate
        {
            get { return this["columntemplate"].ToString(); }
            set { this["columntemplate"] = value; }
        }

        [ConfigurationProperty("groupheadertext", IsRequired = false, DefaultValue = "")]
        public string GroupHeaderText
        {
            get { return this["groupheadertext"].ToString(); }
            set { this["groupheadertext"] = value; }
        }

        [ConfigurationProperty("isupdatable", IsRequired = false, DefaultValue = false)]
        public bool IsUpdatable
        {
            get { return (bool)this["isupdatable"]; }
            set { this["isupdatable"] = value; }
        }

    }
}
