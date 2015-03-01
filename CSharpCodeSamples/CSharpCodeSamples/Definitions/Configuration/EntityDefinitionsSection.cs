namespace CSharpCodeSamples.Definitions.Configuration
{
    using System.Configuration;

    public class EntityDefinitionsSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true, IsKey = false, IsRequired = true)]
        public EntityDefinitionCollection EntityDefinitions
        {
            get { return (EntityDefinitionCollection)this[""]; }
            set { this[""] = value; }
        }
    }

    public class EntityDefinitionCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new EntityDefinitionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EntityDefinitionElement)element).Name;
        }

        protected override bool IsElementName(string elementName)
        {
            return !string.IsNullOrWhiteSpace(elementName) && elementName.ToUpperInvariant() == "ENTITYDEFINITION";
        }

        public EntityDefinitionElement this[int index] { get { return BaseGet(index) as EntityDefinitionElement; } }
        protected override string ElementName { get { return "EntityDefinition"; } }
    }

    public class EntityDefinitionElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return this["name"].ToString(); }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("alias", IsRequired = true)]
        public string Alias
        {
            get { return this["alias"].ToString(); }
            set { this["alias"] = value; }
        }

        [ConfigurationProperty("permname", IsRequired = true)]
        public string PermName
        {
            get { return this["permname"].ToString(); }
            set { this["permname"] = value; }
        }

        [ConfigurationProperty("isaddable", IsRequired = false, DefaultValue = false)]
        public bool IsAddable
        {
            get { return (bool)this["isaddable"]; }
            set { this["isaddable"] = value; }
        }

        [ConfigurationProperty("isdeletable", IsRequired = false, DefaultValue = false)]
        public bool IsDeletable
        {
            get { return (bool)this["isdeletable"]; }
            set { this["isdeletable"] = value; }
        }

        [ConfigurationProperty("menutemplate", IsRequired = false, DefaultValue = "")]
        public string MenuTemplate
        {
            get { return this["menutemplate"].ToString(); }
            set { this["menutemplate"] = value; }
        }
        
        [ConfigurationProperty("", IsDefaultCollection = true, IsKey = false, IsRequired = false)]
        public SubEntityCollection SubEntityDefinitions
        {
            get { return (SubEntityCollection)this[""]; }
            set { this[""] = value; }
        }
    }

    public class SubEntityCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new SubEntityElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SubEntityElement)element).Name;
        }

        protected override bool IsElementName(string elementName)
        {
            return !string.IsNullOrWhiteSpace(elementName) && elementName.ToUpperInvariant() == "SUBENTITY";
        }

        public SubEntityElement this[int index] { get { return BaseGet(index) as SubEntityElement; } }
        protected override string ElementName { get { return "SubEntity"; } }
    }

    public class SubEntityElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }
    }
}
