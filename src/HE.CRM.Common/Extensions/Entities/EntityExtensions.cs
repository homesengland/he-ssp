using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using HE.Base.Common.Extensions;

namespace HE.CRM.Common.Extensions.Entities
{
    public static class EntityExtensions
    {
        public static string GetOptionSetText(this Entity entity, string alias, string attributeName)
        {
            return entity.GetFormattedValue($"{alias}.{attributeName}");
        }

        public static TOptionSet GetOptionSetValue<TOptionSet>(this Entity entity, string attributeName)
        {
            var value = entity.GetAttributeValue<OptionSetValue>(attributeName).Value;

            return (TOptionSet)Enum.Parse(typeof(TOptionSet), value.ToString());
        }

        public static TOptionSet GetOptionSetValue<TOptionSet>(this Entity entity, string linkedEntityAlias, string attributeName)
        {
            var value = entity.GetAliasedAttributeValue<OptionSetValue>(linkedEntityAlias, attributeName).Value;

            return (TOptionSet)Enum.Parse(typeof(TOptionSet), value.ToString());
        }
        /// <summary>
        /// Clones the specified entity to clone.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entityToClone">The entity to clone.</param>
        /// <returns>
        /// Cloned entity without Id.
        /// </returns>
        public static TEntity CloneWithoutId<TEntity>(this Entity entityToClone) where TEntity : Entity
        {
            var entity = new Entity(entityToClone.LogicalName);

            entity.Attributes.AddRange(entityToClone.Attributes);
            entity.RemoveId();

            return entity.ToEntity<TEntity>();
        }

        public static TEntity Clone<TEntity>(this Entity entityToClone) where TEntity : Entity
        {
            var entity = new Entity(entityToClone.LogicalName);

            entity.Attributes.AddRange(entityToClone.Attributes);

            return entity.ToEntity<TEntity>();
        }

        /// <summary>
        /// Removes the identifier.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public static void RemoveId(this Entity entity)
        {
            if (entity.Attributes.Contains($"{entity.LogicalName}id"))
                entity.Attributes.Remove($"{entity.LogicalName}id");
        }

        /// <summary>
        /// Copies the specified entity to copy.
        /// </summary>
        /// <typeparam name="TEntity">Entity to process.</typeparam>
        /// <param name="entityToCopy">The entity to copy.</param>
        /// <returns>Copied object.</returns>
        public static TEntity Copy<TEntity>(this Entity entityToCopy) where TEntity : Entity
        {
            return entityToCopy.Copy().ToEntity<TEntity>();
        }

        /// <summary>
        /// To the entity safe.
        /// </summary>
        /// <typeparam name="TEntity">Entity to process.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <returns>Concrete entity implementation.</returns>
        public static TEntity ToEntitySafe<TEntity>(this Entity entity) where TEntity : Entity
        {
            return entity?.ToEntity<TEntity>();
        }

        /// <summary>
        /// To the entity reference safe.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Entity reference.</returns>
        public static EntityReference ToEntityReferenceSafe(this Entity entity)
        {
            return entity?.ToEntityReference();
        }

        public static T CloneWithoutSystemFields<T>(this T entity) where T : Entity
        {
            string[] systemFields = new[] {
               "createdby",
               "createdon",
               "createdonbehalfby",
               "importsequencenumber",
               "modifiedby",
               "modifiedon",
               "modifiedonbehalfby",
               "overriddencreatedon",
               "ownerid",
               "owningbusinessunit",
               "owningteam",
               "owninguser",
               "timezoneruleversionnumber",
               "versionnumber",
               "statecode",
               "statuscode"
            };

            var clone = new Entity(entity.LogicalName, entity.Id);

            var attributesToCopy = entity.Attributes.Keys.Except(systemFields).ToList();

            foreach (var attributeName in attributesToCopy)
                clone[attributeName] = entity[attributeName];

            return clone.ToEntity<T>();
        }


        /// <summary>
        /// Cleans the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Clean entity with only id and logical name.</returns>
        public static Entity Clean(this Entity entity)
        {
            return entity != null ? new Entity(entity.LogicalName) { Id = entity.Id } : null;
        }

        /// <summary>
        /// Clears the attributes.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="attributes">The attributes.</param>
        public static void ClearAttributes(this Entity entity, IEnumerable<string> attributes)
        {
            attributes.ToList().ForEach(attribute =>
            {
                entity.SetAttribute(attribute, null);
            });
        }

        /// <summary>
        /// Sets the attribute.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="value">The value.</param>
        /// <returns>Same entity.</returns>
        public static Entity SetAttribute(this Entity entity, string keyName, object value)
        {
            if (entity == null)
                return null;

            if (entity.Contains(keyName))
                entity[keyName] = value;
            else
                entity.Attributes.Add(keyName, value);

            return entity;
        }

        /// <summary>
        /// Removes the attribute.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="keyName">Name of the key.</param>
        public static void RemoveAttribute(this Entity entity, string keyName)
        {
            if (entity != null && entity.Contains(keyName))
                entity.Attributes.Remove(keyName);
        }

        /// <summary>
        /// Determines whether the specified original entity has changed.
        /// </summary>
        /// <param name="entityForUpdate">The entity for update.</param>
        /// <param name="originalEntity">The original entity.</param>
        /// <param name="excludedAttributes">The excluded arguments.</param>
        /// <returns>
        ///   <c>true</c> if the specified original entity has changed; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">At least one argument needs to be specified, e.g.: entityId.</exception>
        public static bool HasChanged(this Entity entityForUpdate, Entity originalEntity, ICollection<string> excludedAttributes)
        {
            var hasChanged = false;
            var copy = entityForUpdate.Copy();

            copy.LeaveDirtyAttributes(originalEntity, excludedAttributes);

            if (!excludedAttributes.Any())
                throw new ArgumentNullException(nameof(excludedAttributes), "At least one attribute needs to be specified eg: entityId");

            if (copy.Attributes.Count > excludedAttributes.Count)
                hasChanged = true;

            return hasChanged;
        }

        /// <summary>
        /// What has changed within two entities.
        /// </summary>
        /// <param name="entityForUpdate">The entity for update.</param>
        /// <param name="originalEntity">The original entity.</param>
        /// <param name="excludedArguments">The excluded arguments.</param>
        /// <returns>Collection of attributes.</returns>
        /// <exception cref="ArgumentNullException">At least one argument needs to be specified, e.g: entityId.</exception>
        public static AttributeCollection WhatHasChanged(this Entity entityForUpdate, Entity originalEntity, ICollection<string> excludedArguments)
        {
            var collection = new AttributeCollection();

            var copy = entityForUpdate.Copy();

            copy.LeaveDirtyAttributes(originalEntity, excludedArguments);

            if (!excludedArguments.Any())
                throw new ArgumentNullException(nameof(excludedArguments), "At least one argument needs to be specified eg: entityId");

            if (copy.Attributes.Count > excludedArguments.Count)
            {
                foreach (var attr in excludedArguments)
                    copy.Attributes.Remove(attr);

                collection = copy.Attributes;
            }

            return collection;
        }

        public static AttributeCollection WhatHasChanged(this Entity entityForUpdate, Entity originalEntity)
        {
            return entityForUpdate.WhatHasChanged(originalEntity, new[] { $"{entityForUpdate.LogicalName}id" });
        }

        /// <summary>
        /// Extracts the entity attributes.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Text with attribute values.</returns>
        public static string ExtractEntityAttributes(this Entity entity)
        {
            var sb = new StringBuilder();

            if (entity == null)
            {
                sb.AppendLine("Empty input entity");
                return sb.ToString();
            }

            var maxAttributeLength = entity.Attributes.Max(attribute => attribute.Key.Length) + 10;

            entity.Attributes.OrderBy(item => item.Key).ToList().ForEach(item =>
            {
                var formattedValue = string.Empty;

                if (item.Value != null)
                {
                    switch (item.Value.GetType().Name)
                    {
                        case "String":
                        case "Guid":
                        case "Decimal":
                        case "Int32":
                        case "Int16":
                        case "Int64":
                        case "Double":
                            formattedValue = item.Value.ToString();
                            break;
                        case "Boolean":
                            formattedValue = (item.Value as bool?).ToString();
                            break;
                        case "EntityReference":
                            var reference = item.Value as EntityReference;
                            formattedValue =
                                $"Id: {reference.Id}, LogicalName: {reference.LogicalName}, Name: {reference.Name}";
                            break;
                        case "OptionSetValue":
                            formattedValue = (item.Value as OptionSetValue).Value.ToString();
                            break;
                        case "DateTime":
                            formattedValue = (item.Value as DateTime?).ToString();
                            break;
                        case "Money":
                            formattedValue = (item.Value as Money).Value.ToString();
                            break;
                    }
                }

                sb.AppendLine($"Attribute: {item.Key.PadRight(maxAttributeLength, ' ')} Value: {formattedValue.ToString().PadRight(100, ' ')}");
            });

            return sb.ToString();
        }

        /// <summary>
        /// Updates if data changed.
        /// </summary>
        /// <param name="entityForUpdate">The entity for update.</param>
        /// <param name="originalEntity">The original entity.</param>
        /// <param name="organizationService">The organization service.</param>
        /// <param name="fakeUpdate">If set to <c>true</c> [fake update].</param>
        /// <param name="requestCollection">External request collection.</param>
        /// <returns>True if data has changed, otherwise false.</returns>
        public static bool UpdateIfDirty(this Entity entityForUpdate, Entity originalEntity, IOrganizationService organizationService, bool fakeUpdate = false, OrganizationRequestCollection requestCollection = null)
        {
            return UpdateIfDirty(entityForUpdate, originalEntity, new[] { $"{entityForUpdate.LogicalName}id" }, organizationService, requestCollection);
        }

        /// <summary>
        /// Updates the attributes.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="service">The service.</param>
        /// <param name="attributes">The attributes.</param>
        /// <returns>True if entity has been updated, otherwise false.</returns>
        public static bool UpdateAttributes(this Entity entity, IOrganizationService service, IEnumerable<KeyValuePair<string, object>> attributes)
        {
            var copy = entity.Copy();

            attributes.ToList().ForEach(item => { copy.SetAttribute(item.Key, item.Value); });

            return copy.UpdateIfDirty(entity, service);
        }

        /// <summary>
        /// Updates the attribute.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="service">The service.</param>
        /// <param name="attribute">The attribute.</param>
        /// <returns>Flag indicating if there were any changes.</returns>
        public static bool UpdateAttribute(this Entity entity, IOrganizationService service, KeyValuePair<string, object> attribute)
        {
            var copy = entity.Copy();
            copy.SetAttribute(attribute.Key, attribute.Value);

            return copy.UpdateIfDirty(entity, service);
        }

        /// <summary>
        /// Copies the specified entity automatic copy.
        /// </summary>
        /// <param name="entityToCopy">The entity automatic copy.</param>
        /// <returns>Copy of the entity.</returns>
        public static Entity Copy(this Entity entityToCopy)
        {
            var entity = new Entity
            {
                Id = entityToCopy.Id,
                LogicalName = entityToCopy.LogicalName
            };

            entity.Attributes.AddRange(entityToCopy.Attributes);

            return entity;
        }

        private static IEnumerable<KeyValuePair<string, object>> LeaveDirtyAttributes(this Entity entityForUpdate, Entity originalEntity, ICollection<string> excludedArguments)
        {
            var attributesToRemove = new Collection<KeyValuePair<string, object>>();

            foreach (var attribute in entityForUpdate.Attributes)
            {
                var attributeName = attribute.Key;

                if (excludedArguments.Contains(attributeName))
                    continue;

                if (!originalEntity.Contains(attributeName))
                    continue;

                if (originalEntity.Contains(attributeName) && attribute.Value != null && attribute.Value.Equals(originalEntity.Attributes[attributeName]))
                    attributesToRemove.Add(attribute);

                if (originalEntity.Contains(attributeName) && attribute.Value == null && attribute.Value == originalEntity.Attributes[attributeName])
                    attributesToRemove.Add(attribute);
            }

            foreach (var item in attributesToRemove)
                entityForUpdate.Attributes.Remove(item.Key);

            return attributesToRemove;
        }

        private static bool UpdateIfDirty(this Entity entityForUpdate, Entity originalEntity, ICollection<string> excludedArguments, IOrganizationService organizationService, OrganizationRequestCollection requestCollection = null)
        {
            var wasUpdated = false;
            var removedAttributes = entityForUpdate.LeaveDirtyAttributes(originalEntity, excludedArguments);

            if (!excludedArguments.Any())
                throw new ArgumentNullException(nameof(excludedArguments), "At least one argument needs to be specified eg: entityId");

            if (entityForUpdate.Attributes.Count > excludedArguments.Count)
            {
                if (requestCollection != null)
                {
                    requestCollection.Add(new UpdateRequest { Target = entityForUpdate });
                }
                else
                {
                    organizationService.Update(entityForUpdate);
                }

                wasUpdated = true;
            }

            // Restore Removed Attributes
            entityForUpdate.Attributes.AddRange(removedAttributes);

            // Re-initialize object with object reference saving which will allow to execute another changes
            originalEntity.Attributes.Clear();
            originalEntity.Attributes.AddRange(entityForUpdate.Attributes);

            return wasUpdated;
        }
    }
}
