using FakeXrmEasy;
using FakeXrmEasy.Extensions;
using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;

namespace PwC.Base.Tests.Plugins.Helpers
{
    /// <summary>
    /// Easier initalization of fake context metadata
    /// </summary>
    public class MetadataHelper
    {
        private readonly Dictionary<string, EntityMetadata> entitiesMetadata;
        private readonly XrmFakedContext fakedContext;

        public MetadataHelper(XrmFakedContext fakedContext)
        {
            this.fakedContext = fakedContext;
            entitiesMetadata = new Dictionary<string, EntityMetadata>();
        }

        public MetadataHelper AddAlternateKey(string entityName, string[] fieldNames)
        {
            var entityMetadata = GetEntityMetadata(entityName);
            var alternateKeyMetadata = new EntityKeyMetadata
            {
                KeyAttributes = fieldNames
            };
            entityMetadata.SetFieldValue("_keys", new EntityKeyMetadata[] { alternateKeyMetadata });

            return this;
        }

        public void InitializeMetadata()
        {
            foreach (var entityMetadata in entitiesMetadata)
            {
                fakedContext.InitializeMetadata(entityMetadata.Value);
            }
        }

        private EntityMetadata GetEntityMetadata(string entityName)
        {
            if (entitiesMetadata.ContainsKey(entityName))
            {
                return entitiesMetadata[entityName];
            }
            else
            {
                var entityMetadata = new EntityMetadata { LogicalName = entityName };
                entitiesMetadata.Add(entityName, entityMetadata);

                return entityMetadata;
            }
        }
    }
}