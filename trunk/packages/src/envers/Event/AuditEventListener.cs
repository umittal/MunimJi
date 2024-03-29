﻿using System.Collections.Generic;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Entities;
using NHibernate.Envers.Synchronization;
using NHibernate.Envers.Synchronization.Work;
using NHibernate.Envers.Tools;
using NHibernate.Event;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;

namespace NHibernate.Envers.Event
{
	public class AuditEventListener : IPostInsertEventListener,
									IPostUpdateEventListener,
									IPostDeleteEventListener,
									IPreCollectionUpdateEventListener,
									IPreCollectionRemoveEventListener,
									IPostCollectionRecreateEventListener,
									IInitializable
	{
		public AuditConfiguration VerCfg { get; private set; }

		private void GenerateBidirectionalCollectionChangeWorkUnits(AuditProcess auditProcess,
																	IEntityPersister entityPersister,
																	string entityName,
																	IList<object> newState,
																	IList<object> oldState,
																	ISessionImplementor session)
		{
			// Checking if this is enabled in configuration ...
			if (!VerCfg.GlobalCfg.GenerateRevisionsForCollections)
				return;

			// Checks every property of the entity, if it is an "owned" to-one relation to another entity.
			// If the value of that property changed, and the relation is bi-directional, a new revision
			// for the related entity is generated.
			var propertyNames = entityPersister.PropertyNames;

			for (var i = 0; i < propertyNames.GetLength(0); i++)
			{
				var propertyName = propertyNames[i];
				var relDesc = VerCfg.EntCfg.GetRelationDescription(entityName, propertyName);
				if (relDesc != null &&
					relDesc.Bidirectional &&
					relDesc.RelationType == RelationType.ToOne &&
					relDesc.Insertable)
				{
					// Checking for changes
					var oldValue = oldState == null ? null : oldState[i];
					var newValue = newState == null ? null : newState[i];

					if (!Toolz.EntitiesEqual(session, oldValue, newValue))
					{
						// We have to generate changes both in the old collection (size decreses) and new collection
						// (size increases).
						if (newValue != null)
						{
							addCollectionChangeWorkUnitToAuditProcess(session, auditProcess, newValue);
						}
						if (oldValue != null)
						{
							addCollectionChangeWorkUnitToAuditProcess(session, auditProcess, oldValue);
						}
					}
				}
			}
		}

		private void addCollectionChangeWorkUnitToAuditProcess(ISessionImplementor session, AuditProcess auditProcess, object value)
		{
			// relDesc.getToEntityName() doesn't always return the entity name of the value - in case
			// of subclasses, this will be root class, no the actual class. So it can't be used here.
			string toEntityName;
			object id;

			var newValueAsProxy = value as INHibernateProxy;
			if (newValueAsProxy != null)
			{
				toEntityName = session.BestGuessEntityName(value);
				id = newValueAsProxy.HibernateLazyInitializer.Identifier;
				// We've got to initialize the object from the proxy to later read its state.   
				value = Toolz.GetTargetFromProxy((ISession) session, newValueAsProxy);
			}
			else
			{
				toEntityName = session.GuessEntityName(value);

				var idMapper = VerCfg.EntCfg[toEntityName].IdMapper;
				id = idMapper.MapToIdFromEntity(value);
			}

			auditProcess.AddWorkUnit(new CollectionChangeWorkUnit(session, toEntityName, VerCfg, id, value));
		}

		public virtual void OnPostInsert(PostInsertEvent evt)
		{
			var entityName = evt.Persister.EntityName;
			if (!VerCfg.EntCfg.IsVersioned(entityName)) return;

			var auditProcess = VerCfg.AuditProcessManager.Get(evt.Session);
			var workUnit = new AddWorkUnit(evt.Session, evt.Persister.EntityName, VerCfg,
																		evt.Id, evt.Persister, evt.State);
			auditProcess.AddWorkUnit(workUnit);
			if (workUnit.ContainsWork())
			{
				GenerateBidirectionalCollectionChangeWorkUnits(auditProcess, evt.Persister, entityName, evt.State,
																		null, evt.Session);
			}
		}

		public virtual void OnPostUpdate(PostUpdateEvent evt)
		{
			var entityName = evt.Persister.EntityName;
			if (!VerCfg.EntCfg.IsVersioned(entityName)) return;

			var verSync = VerCfg.AuditProcessManager.Get(evt.Session);
			var workUnit = new ModWorkUnit(evt.Session, evt.Persister.EntityName, VerCfg,
																		 evt.Id, evt.Persister, evt.State, evt.OldState);
			verSync.AddWorkUnit(workUnit);
			if (workUnit.ContainsWork())
			{
				GenerateBidirectionalCollectionChangeWorkUnits(verSync, evt.Persister, entityName, evt.State,
																	evt.OldState, evt.Session);
			}
		}

		public virtual void OnPostDelete(PostDeleteEvent evt)
		{
			var entityName = evt.Persister.EntityName;
			if (!VerCfg.EntCfg.IsVersioned(entityName)) return;

			var verSync = VerCfg.AuditProcessManager.Get(evt.Session);
			var workUnit = new DelWorkUnit(evt.Session, evt.Persister.EntityName, VerCfg,
											evt.Id, evt.Persister, evt.DeletedState);
			verSync.AddWorkUnit(workUnit);
			if (workUnit.ContainsWork())
			{
				GenerateBidirectionalCollectionChangeWorkUnits(verSync, evt.Persister, entityName, null,
																	evt.DeletedState, evt.Session);
			}
		}

		private void GenerateBidirectionalCollectionChangeWorkUnits(AuditProcess auditProcess,
																	IDatabaseEventArgs evt,
																	PersistentCollectionChangeWorkUnit workUnit,
																	RelationDescription rd)
		{
			// Checking if this is enabled in configuration ...
			if (!VerCfg.GlobalCfg.GenerateRevisionsForCollections)
				return;

			// Checking if this is not a bidirectional relation - then, a revision needs also be generated for
			// the other side of the relation.
			// relDesc can be null if this is a collection of simple values (not a relation).
			if (rd != null && rd.Bidirectional)
			{
				var relatedEntityName = rd.ToEntityName;
				var relatedIdMapper = VerCfg.EntCfg[relatedEntityName].IdMapper;

				foreach (var changeData in workUnit.CollectionChanges)
				{
					var relatedObj = changeData.GetChangedElement();
					var relatedId = relatedIdMapper.MapToIdFromEntity(relatedObj);

					auditProcess.AddWorkUnit(new CollectionChangeWorkUnit(evt.Session,
																							evt.Session.BestGuessEntityName(relatedObj), 
																							VerCfg, 
																							relatedId, 
																							relatedObj));
				}
			}
		}

		private void GenerateFakeBidirecationalRelationWorkUnits(AuditProcess auditProcess,
																IPersistentCollection newColl,
																object oldColl,
																string collectionEntityName,
																string referencingPropertyName,
																AbstractCollectionEvent evt,
																RelationDescription rd)
		{
			// First computing the relation changes
			var collectionChanges = VerCfg.EntCfg[collectionEntityName].PropertyMapper
					.MapCollectionChanges(referencingPropertyName, newColl, oldColl, evt.AffectedOwnerIdOrNull);

			// Getting the id mapper for the related entity, as the work units generated will corrspond to the related
			// entities.
			var relatedEntityName = rd.ToEntityName;
			var relatedIdMapper = VerCfg.EntCfg[relatedEntityName].IdMapper;

			// For each collection change, generating the bidirectional work unit.
			foreach (var changeData in collectionChanges)
			{
				var relatedObj = changeData.GetChangedElement();
				var relatedId = relatedIdMapper.MapToIdFromEntity(relatedObj);
				var revType = (RevisionType)changeData.Data[VerCfg.AuditEntCfg.RevisionTypePropName];

				// This can be different from relatedEntityName, in case of inheritance (the real entity may be a subclass
				// of relatedEntityName).
				var realRelatedEntityName = evt.Session.BestGuessEntityName(relatedObj);

				// By default, the nested work unit is a collection change work unit.
				var nestedWorkUnit = new CollectionChangeWorkUnit(evt.Session, realRelatedEntityName, VerCfg,
						relatedId, relatedObj);

				auditProcess.AddWorkUnit(new FakeBidirectionalRelationWorkUnit(evt.Session, realRelatedEntityName, VerCfg,
						relatedId, referencingPropertyName, evt.AffectedOwnerOrNull, rd, revType,
						changeData.GetChangedElementIndex(), nestedWorkUnit));
			}

			// We also have to generate a collection change work unit for the owning entity.
			auditProcess.AddWorkUnit(new CollectionChangeWorkUnit(evt.Session, collectionEntityName, VerCfg,
					evt.AffectedOwnerIdOrNull, evt.AffectedOwnerOrNull));
		}

		private void OnCollectionAction(AbstractCollectionEvent evt,
										IPersistentCollection newColl,
										object oldColl,
										CollectionEntry collectionEntry)
		{
			if (!VerCfg.GlobalCfg.GenerateRevisionsForCollections)
				return;
			var entityName = evt.GetAffectedOwnerEntityName();
			if (!VerCfg.EntCfg.IsVersioned(entityName)) return;

			var verSync = VerCfg.AuditProcessManager.Get(evt.Session);
			var ownerEntityName = ((AbstractCollectionPersister)collectionEntry.LoadedPersister).OwnerEntityName;
			var referencingPropertyName = collectionEntry.Role.Substring(ownerEntityName.Length + 1);

			// Checking if this is not a "fake" many-to-one bidirectional relation. The relation description may be
			// null in case of collections of non-entities.
			var rd = VerCfg.EntCfg[entityName].GetRelationDescription(referencingPropertyName);
			if (rd != null && rd.MappedByPropertyName != null)
			{
				GenerateFakeBidirecationalRelationWorkUnits(verSync, newColl, oldColl, entityName,
															referencingPropertyName, evt, rd);
			}
			else
			{
				var workUnit = new PersistentCollectionChangeWorkUnit(evt.Session, entityName, VerCfg, newColl, 
																collectionEntry, oldColl, evt.AffectedOwnerIdOrNull,
																referencingPropertyName);
				verSync.AddWorkUnit(workUnit);

				if (workUnit.ContainsWork())
				{
					// There are some changes: a revision needs also be generated for the collection owner
					verSync.AddWorkUnit(new CollectionChangeWorkUnit(evt.Session, evt.GetAffectedOwnerEntityName(),
																VerCfg, evt.AffectedOwnerIdOrNull, evt.AffectedOwnerOrNull));

					GenerateBidirectionalCollectionChangeWorkUnits(verSync, evt, workUnit, rd);
				}
			}
		}

		private static CollectionEntry GetCollectionEntry(AbstractCollectionEvent evt)
		{
			return evt.Session.PersistenceContext.GetCollectionEntry(evt.Collection);
		}

		public virtual void OnPreUpdateCollection(PreCollectionUpdateEvent evt)
		{
			var collectionEntry = GetCollectionEntry(evt);
			if (!collectionEntry.LoadedPersister.IsInverse)
			{
				OnCollectionAction(evt, evt.Collection, collectionEntry.Snapshot, collectionEntry);
			}
		}

		public virtual void OnPreRemoveCollection(PreCollectionRemoveEvent evt)
		{
			var collectionEntry = GetCollectionEntry(evt);
			if (collectionEntry != null && !collectionEntry.LoadedPersister.IsInverse)
			{
				OnCollectionAction(evt, null, collectionEntry.Snapshot, collectionEntry);
			}
		}

		public virtual void OnPostRecreateCollection(PostCollectionRecreateEvent evt)
		{
			var collectionEntry = GetCollectionEntry(evt);
			if (!collectionEntry.LoadedPersister.IsInverse)
			{
				OnCollectionAction(evt, evt.Collection, null, collectionEntry);
			}
		}

		public virtual void Initialize(Cfg.Configuration cfg)
		{
			VerCfg = AuditConfiguration.GetFor(cfg);
		}
	}
}