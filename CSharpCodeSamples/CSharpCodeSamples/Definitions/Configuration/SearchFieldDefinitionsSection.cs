namespace CSharpCodeSamples.Definitions.Configuration
{
    using System.Configuration;

    public class SearchFieldDefinitionsSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true, IsKey = false, IsRequired = true)]
        public SearchFieldDefinitionCollection SearchFieldDefinitions
        {
            get { return (SearchFieldDefinitionCollection)this[""]; }
            set { this[""] = value; }
        }
    }

    public class SearchFieldDefinitionCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SearchFieldDefinitionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SearchFieldDefinitionElement)element).EntityName;
        }

        protected override bool IsElementName(string elementName)
        {
            return !string.IsNullOrWhiteSpace(elementName) && elementName.ToUpperInvariant() == "SEARCHFIELDDEFINITION";
        }

        public SearchFieldDefinitionElement this[int index] { get { return BaseGet(index) as SearchFieldDefinitionElement; } }
        protected override string ElementName { get { return "SearchFieldDefinition"; } }
    }

    public class SearchFieldDefinitionElement : ConfigurationElement
    {
        [ConfigurationProperty("entityname", IsRequired = true, IsKey = true)]
        public string EntityName
        {
            get { return this["entityname"].ToString(); }
            set { this["entityname"] = value; }
        }

        [ConfigurationProperty("", IsDefaultCollection = true, IsKey = false, IsRequired = true)]
        public FieldGroupDefinitionCollection SearchFieldGroups
        {
            get { return (FieldGroupDefinitionCollection)this[""]; }
            set { this[""] = value; }
        }
    }

    public class FieldGroupDefinitionCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FieldGroupDefinitionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FieldGroupDefinitionElement)element).AliasList;
        }

        protected override bool IsElementName(string elementName)
        {
            return !string.IsNullOrWhiteSpace(elementName) && elementName.ToUpperInvariant() == "FIELDGROUPDEFINITION";
        }

        public FieldGroupDefinitionElement this[int index] { get { return BaseGet(index) as FieldGroupDefinitionElement; } }
        protected override string ElementName { get { return "FieldGroupDefinition"; } }
    }

    public class FieldGroupDefinitionElement : ConfigurationElement
    {
        [ConfigurationProperty("alias", IsRequired = true, IsKey = true)]
        public string AliasList
        {
            get { return this["alias"].ToString(); }
            set { this["alias"] = value; }
        }

        [ConfigurationProperty("datatype", IsRequired = false, DefaultValue = "string")]
        public string DataType
        {
            get { return this["datatype"].ToString(); }
            set { this["datatype"] = value; }
        }

        [ConfigurationProperty("islongtext", IsRequired = false, DefaultValue = false)]
        public bool IsLongText
        {
            get { return (bool)this["islongtext"]; }
            set { this["islongtext"] = value; }
        }

        [ConfigurationProperty("", IsDefaultCollection = true, IsKey = false, IsRequired = true)]
        public FieldDefinitionCollection Fields
        {
            get { return (FieldDefinitionCollection)this[""]; }
            set { this[""] = value; }
        }
    }

    public class FieldDefinitionCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FieldDefinitionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FieldDefinitionElement)element).Name;
        }

        protected override bool IsElementName(string elementName)
        {
            return !string.IsNullOrWhiteSpace(elementName) && elementName.ToUpperInvariant() == "FIELDDEFINITION";
        }

        public FieldDefinitionElement this[int index] { get { return BaseGet(index) as FieldDefinitionElement; } }
        protected override string ElementName { get { return "FieldDefinition"; } }
    }

    public class FieldDefinitionElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return this["name"].ToString(); }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("iscrossfield", IsRequired = false, DefaultValue = false)]
        public bool IsCrossField
        {
            get { return (bool)this["iscrossfield"]; }
            set { this["iscrossfield"] = value; }
        }

        [ConfigurationProperty("isdefaultdate", IsRequired = false, DefaultValue = false)]
        public bool IsDefaultDate
        {
            get { return (bool)this["isdefaultdate"]; }
            set { this["isdefaultdate"] = value; }
        }

        [ConfigurationProperty("isupdatable", IsRequired = false, DefaultValue = false)]
        public bool IsUpdatable
        {
            get { return (bool)this["isupdatable"]; }
            set { this["isupdatable"] = value; }
        }
    }
}
