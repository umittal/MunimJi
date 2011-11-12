using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Envers.Configuration;
using NHibernate.Envers.Entities.Mapper;
using NHibernate.Envers.Entities.Mapper.Relation;
using NHibernate.Envers.Query;
using NHibernate.Envers.Synchronization;
using NHibernate.Envers.Tools.Query;
using NHibernate.Properties;

namespace NHibernate.Envers.Strategy
{
	/// <summary>
	/// Audit strategy which persists and retrieves audit information using a validity algorithm, based on the 
	/// start-revision and end-revision of a row in the audit tables. 
	/// <p>This algorithm works as follows:
	/// <ul>
	/// <li>For a <strong>new row</strong> that is persisted in an audit table, only the <strong>start-revision</strong> column of that row is set</li>
	/// <li>At the same time the <strong>end-revision</strong> field of the <strong>previous</strong> audit row is set to this revision</li>
	/// <li>Queries are retrieved using 'between start and end revision', instead of a subquery.</li>
	/// </ul>
	/// </p>
	/// 
	/// <p>
	/// This has a few important consequences that need to be judged against against each other:
	/// <ul>
	/// <li>Persisting audit information is a bit slower, because an extra row is updated</li>
	/// <li>Retrieving audit information is a lot faster</li>
	/// </ul>
	/// </p>
	/// </summary>
	public class ValidityAuditStrategy : IAuditStrategy
	{
		//getter for the revision entity field annotated with @RevisionTimestamp 
		private IGetter revisionTimestampGetter;

		public void Perform(ISession session, string entityName, AuditConfiguration auditCfg, object id, object data, object revision)
		{
			var audEntCfg = auditCfg.AuditEntCfg;
			var auditedEntityName = audEntCfg.GetAuditEntityName(entityName);

			// Update the end date of the previous row if this operation is expected to have a previous row
			if (revisionType(auditCfg, data) != RevisionType.Added)
			{
				/*
				 Constructing a query:
				 select e from audited_ent e where e.end_rev is null and e.id = :id
				*/
				var qb = new QueryBuilder(auditedEntityName, "e");

				// e.id = :id
				var idMapper = auditCfg.EntCfg[entityName].IdMapper;
				idMapper.AddIdEqualsToQuery(qb.RootParameters, id, auditCfg.AuditEntCfg.OriginalIdPropName, true);

				addEndRevisionNullRestriction(auditCfg, qb);

				var l = qb.ToQuery(session).SetLockMode("e", LockMode.Upgrade).List();

				updateLastRevision(session, auditCfg, l, id, auditedEntityName, revision);
			}

			// Save the audit data
			session.Save(auditedEntityName, data);
			SessionCacheCleaner.ScheduleAuditDataRemoval(session, data);
		}

		public void PerformCollectionChange(ISession session, AuditConfiguration auditCfg, PersistentCollectionChangeData persistentCollectionChangeData, object revision)
		{
			var qb = new QueryBuilder(persistentCollectionChangeData.EntityName, "e");

			// Adding a parameter for each id component, except the rev number
			var originalIdPropName = auditCfg.AuditEntCfg.OriginalIdPropName;
			var originalId = (IDictionary)persistentCollectionChangeData.Data[originalIdPropName];
			foreach (DictionaryEntry originalIdKeyValue in originalId)
			{
				if (!auditCfg.AuditEntCfg.RevisionFieldName.Equals(originalIdKeyValue.Key))
				{
					qb.RootParameters.AddWhereWithParam(originalIdPropName + "." + originalIdKeyValue.Key, true, "=", originalIdKeyValue.Value);
				}
			}

			addEndRevisionNullRestriction(auditCfg, qb);

			var l = qb.ToQuery(session).SetLockMode("e", LockMode.Upgrade).List();

			if (l.Count > 0)
			{
				updateLastRevision(session, auditCfg, l, originalId, persistentCollectionChangeData.EntityName, revision);
			}

			// Save the audit data
			var data = persistentCollectionChangeData.Data;
			session.Save(persistentCollectionChangeData.EntityName, data);
			SessionCacheCleaner.ScheduleAuditDataRemoval(session, data);
		}

		private void addEndRevisionNullRestriction(AuditConfiguration auditCfg, QueryBuilder qb)
		{
			// e.end_rev is null
			qb.RootParameters.AddWhere(auditCfg.AuditEntCfg.RevisionEndFieldName, true, "is", "null", false);
		}

		public void AddEntityAtRevisionRestriction(GlobalConfiguration globalCfg, QueryBuilder rootQueryBuilder, string revisionProperty, string revisionEndProperty, bool addAlias, MiddleIdData idData, string revisionPropertyPath, string originalIdPropertyName, string alias1, string alias2)
		{
			var rootParameters = rootQueryBuilder.RootParameters;
			addRevisionRestriction(rootParameters, revisionProperty, revisionEndProperty, addAlias);
		}

		public void AddAssociationAtRevisionRestriction(QueryBuilder rootQueryBuilder, string revisionProperty, string revisionEndProperty, bool addAlias, MiddleIdData referencingIdData, string versionsMiddleEntityName, string eeOriginalIdPropertyPath, string revisionPropertyPath, string originalIdPropertyName, params MiddleComponentData[] componentDatas)
		{
			var rootParameters = rootQueryBuilder.RootParameters;
			addRevisionRestriction(rootParameters, revisionProperty, revisionEndProperty, addAlias);
		}

		public void SetRevisionTimestampGetter(IGetter revisionTimestampGetter)
		{
			this.revisionTimestampGetter = revisionTimestampGetter;
		}

		private static void addRevisionRestriction(Parameters rootParameters, string revisionProperty, string revisionEndProperty, bool addAlias)
		{
			// e.revision <= _revision and (e.endRevision > _revision or e.endRevision is null)
			var subParm = rootParameters.AddSubParameters("or");
			rootParameters.AddWhereWithNamedParam(revisionProperty, addAlias, "<=", "revision");
			subParm.AddWhereWithNamedParam(revisionEndProperty + ".id", addAlias, ">", "revision");
			subParm.AddWhere(revisionEndProperty, addAlias, "is", "null", false);
		}

		private static RevisionType revisionType(AuditConfiguration auditCfg, object data)
		{
			return (RevisionType)((IDictionary<string, object>)data)[auditCfg.AuditEntCfg.RevisionTypePropName];
		}

		private void updateLastRevision(ISession session, AuditConfiguration auditCfg, IList l,
									object id, string auditedEntityName, object revision)
		{
			// There should be one entry
			if (l.Count == 1)
			{
				// Setting the end revision to be the current rev
				var previousData = l[0];
				var revisionEndFieldName = auditCfg.AuditEntCfg.RevisionEndFieldName;
				((IDictionary)previousData)[revisionEndFieldName] = revision;

				if (auditCfg.AuditEntCfg.IsRevisionEndTimestampEnabled)
				{
					// Determine the value of the revision property annotated with @RevisionTimestamp
					DateTime revisionEndTimestamp;
					var revEndTimestampFieldName = auditCfg.AuditEntCfg.RevisionEndTimestampFieldName;
					var revEndTimestampObj = revisionTimestampGetter.Get(revision);

					// convert to a DateTime
					if (revEndTimestampObj is DateTime)
					{
						revisionEndTimestamp = (DateTime)revEndTimestampObj;
					}
					else
					{
						revisionEndTimestamp = new DateTime((long)revEndTimestampObj);
					}

					// Setting the end revision timestamp
					((IDictionary)previousData)[revEndTimestampFieldName] = revisionEndTimestamp;
				}

				// Saving the previous version
				session.Save(auditedEntityName, previousData);
				SessionCacheCleaner.ScheduleAuditDataRemoval(session, previousData);
			}
			else
			{
				throw new InvalidOperationException("Cannot find previous revision for entity " + auditedEntityName + " and id " + id);
			}
		}
	}
}