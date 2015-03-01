namespace CSharpCodeSamples.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;

    using Common.Interfaces.Models.Definitions;

    [Serializable]
    internal class EntityDefinition : IEntityDefinition
    {
        //* Field Alias & Field Definitions *
        private readonly Dictionary<string, IFieldGroupDefinition> _fieldGroups; //field alias, group def

        public string                                    EntityName           { get; private set; }

        public List<IFieldDefinition>                    CrossFieldTargets
        {
            get
            {
                List<IFieldDefinition> result = new List<IFieldDefinition>();
                foreach (IFieldDefinition fd in _fieldGroups.Values
                                                            .SelectMany(fgd => fgd.FieldDefinitions
                                                                                  .Where(i => i.IsCrossField)
                                                                                  .Where(fd => !result.Contains(fd))))
                {
                    result.Add(fd);
                }
                return result;
            }
        }
        public IFieldDefinition                          DefaultDate
        {
            get
            {
                IFieldDefinition result = null;
                foreach (IFieldGroupDefinition fgd in _fieldGroups.Values.Where(fgd => fgd.FieldDefinitions.Any(f => f.IsDefaultDate)))
                {
                    if (fgd.FieldDefinitions.Count > 1)
                    {
                        throw new ConfigurationErrorsException("Invalid scope configuration.  More than on field marked as default date within fieldgroup");
                    }
                    result = fgd.FieldDefinitions.First();
                }

                return result;
            }
        }
        public List<IFieldDefinition>                    DefaultDecimals
        {
            get {
                return _fieldGroups.Values.Where(fgd => fgd.DataType == typeof(decimal))
                                          .SelectMany(fgd => fgd.FieldDefinitions)
                                          .ToList();
            }
        }
        public List<IFieldDefinition>                    DefaultInts
        {
            get
            {
                return _fieldGroups.Values.Where(fgd => fgd.DataType == typeof(int))
                                          .SelectMany(fgd => fgd.FieldDefinitions)
                                          .ToList();
            }
        }
        public SortedList<int, IDisplayColumn>           DisplayColumnsSorted { get; private set; }
        public IEnumerable<IDisplayColumn>               DisplayColumns       { get { return DisplayColumnsSorted.Values; } }
        public Dictionary<string, IFieldGroupDefinition> FieldGroups          { get { return _fieldGroups; } }
        public IEnumerable<IResizeSetting>               ResizeSettings
        {
            get
            {
                return (from dc in DisplayColumns
                        where !string.IsNullOrWhiteSpace(dc.ColumnMinWidth)
                        select new ResizeSetting(dc.Key, int.Parse(dc.ColumnMinWidth)));
            }
        }

        public bool   HasAnyFieldWithDefaultDate { get { return _fieldGroups.Values.Any(fad => fad.FieldDefinitions.Any(fd => fd.IsDefaultDate)); } }
        public bool   HasAnyFieldThatIsDecimal   { get { return _fieldGroups.Values.Any(fad => fad.DataType == typeof(decimal)); } }
        public bool   HasAnyFieldThatIsInt       { get { return _fieldGroups.Values.Any(fad => fad.DataType == typeof(int)); } }
        public bool   IsAddable                  { get; private set; }
        public bool   IsDeletable                { get; private set; }
        public string MenuTemplate               { get; private set; }
        public string PermName                   { get; private set; }
        
        
        public EntityDefinition(IEntityTypeBrief entityType)
        {
            Debug.Assert(entityType != null && !string.IsNullOrWhiteSpace(entityType.Name), "Invalid entity type specified.");

            EntityName           = entityType.Name;
            PermName             = entityType.PermName;
            IsAddable            = entityType.IsAddable;
            IsDeletable          = entityType.IsDeletable;
            MenuTemplate         = entityType.MenuTemplate;

            _fieldGroups         = new Dictionary<string, IFieldGroupDefinition>();

            DisplayColumnsSorted = new SortedList<int, IDisplayColumn>();
        }


        /// <summary>
        /// Returns the default cross field search definition.
        /// </summary>
        /// <returns>Returns the default <see cref="IFieldGroupDefinition"/> used for cross field search with the current Entity Type.</returns>
        public IFieldGroupDefinition CrossFieldDefinition()
        {
            IFieldGroupDefinition fgd = new FieldGroupDefinition
            {
                AliasName = "root",
                IsLongText = false
            };
            fgd.FieldDefinitions.Add(
                new FieldDefinition
                {
                    DataType = typeof(string),
                    IsDefaultDate = false,
                    IsUpdatable = false,
                    Name = ""
                });
            return fgd;
        }
        /// <summary>
        /// Determine if the supplied field name alias exists for the entity type.
        /// </summary>
        /// <param name="fieldAliasName">Alias of the field name to validate.</param>
        /// <returns>true if the entity type has the specified field name alias, false otherwise.</returns>
        public bool DoesEntityHaveFieldAlias(string fieldAliasName)
        {
            return _fieldGroups.ContainsKey(fieldAliasName);
        }
        /// <summary>
        /// Returns Field Group specific configuration for the specified field name.
        /// </summary>
        /// <param name="fieldAliasName">Alias of the field name to retrieve.</param>
        /// <returns><see cref="IFieldGroupDefinition"/> containing field group configuration data for specified field alias name.</returns>
        public IFieldGroupDefinition WithSearchField(string fieldAliasName)
        {
            return DoesEntityHaveFieldAlias(fieldAliasName) ? _fieldGroups[fieldAliasName] : null;
        }
    }
}
