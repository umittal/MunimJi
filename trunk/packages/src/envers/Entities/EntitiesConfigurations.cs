﻿using System.Collections.Generic;

namespace NHibernate.Envers.Entities
{
	/// <summary>
	/// Configuration of the user entities: property mapping of the entities, relations, inheritance.
	/// </summary>
	public class EntitiesConfigurations
	{
		private readonly IDictionary<string, EntityConfiguration> entitiesConfigurations;
		private readonly IDictionary<string, EntityConfiguration> notAuditedEntitiesConfigurations;

		// Map versions entity name -> entity name
		private IDictionary<string, string> _entityNamesForVersionsEntityNames = new Dictionary<string, string>();

		public EntitiesConfigurations(IDictionary<string, EntityConfiguration> entitiesConfigurations,
									  IDictionary<string, EntityConfiguration> notAuditedEntitiesConfigurations)
		{
			this.entitiesConfigurations = entitiesConfigurations;
			this.notAuditedEntitiesConfigurations = notAuditedEntitiesConfigurations;

			GenerateBidirectionRelationInfo();
			GenerateVersionsEntityToEntityNames();
		}

		private void GenerateVersionsEntityToEntityNames() 
		{
			_entityNamesForVersionsEntityNames = new Dictionary<string, string>();

			foreach (var entityName in entitiesConfigurations.Keys) 
			{
				_entityNamesForVersionsEntityNames.Add(entitiesConfigurations[entityName].VersionsEntityName, entityName);
			}
		}

		private void GenerateBidirectionRelationInfo() 
		{
			// Checking each relation if it is bidirectional. If so, storing that information.
			foreach (var entityName in entitiesConfigurations.Keys) 
			{
				var entCfg = entitiesConfigurations[entityName];
				// Iterating over all relations from that entity
				foreach (var relDesc in entCfg.RelationsIterator) 
				{
					// If this is an "owned" relation, checking the related entity, if it has a relation that has
					// a mapped-by attribute to the currently checked. If so, this is a bidirectional relation.
					if (relDesc.RelationType == RelationType.ToOne || relDesc.RelationType == RelationType.ToManyMiddle) 
					{
						if (entitiesConfigurations.Keys.Contains(relDesc.ToEntityName))
						{
							var entityConfiguration = entitiesConfigurations[relDesc.ToEntityName];
							foreach (var other in entityConfiguration.RelationsIterator) 
							{
								if (relDesc.FromPropertyName.Equals(other.MappedByPropertyName) &&
										(entityName.Equals(other.ToEntityName))) 
								{
									relDesc.Bidirectional = true;
									other.Bidirectional = true;
								}
							}
						}
					}
				}
			}
		}

		public EntityConfiguration this [string entityName]
		{
			get { return entitiesConfigurations.ContainsKey(entityName)? entitiesConfigurations[entityName]:null; }
		}

		public EntityConfiguration GetNotVersionEntityConfiguration(string entityName)
		{
			return notAuditedEntitiesConfigurations.ContainsKey(entityName)? notAuditedEntitiesConfigurations[entityName]:null;
		}

		public string GetEntityNameForVersionsEntityName(string versionsEntityName)
		{
			if (versionsEntityName == null)
				return null;
			string ret;
			return _entityNamesForVersionsEntityNames.TryGetValue(versionsEntityName, out ret) ? ret : null;
		}

		public bool IsVersioned(string entityName)
		{
			return entitiesConfigurations.Keys.Contains(entityName);
		}

		public RelationDescription GetRelationDescription(string entityName, string propertyName)
		{
			var entCfg = entitiesConfigurations[entityName];
			var relDesc = entCfg.GetRelationDescription(propertyName);
			if (relDesc != null)
			{
				return relDesc;
			}
			return entCfg.ParentEntityName != null ? GetRelationDescription(entCfg.ParentEntityName, propertyName) : null;
		}
	}
}