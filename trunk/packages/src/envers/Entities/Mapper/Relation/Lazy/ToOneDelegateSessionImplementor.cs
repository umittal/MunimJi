﻿using NHibernate.Envers.Configuration;
using NHibernate.Envers.Reader;

namespace NHibernate.Envers.Entities.Mapper.Relation.Lazy
{
	public class ToOneDelegateSessionImplementor : AbstractDelegateSessionImplementor 
	{
		private readonly IAuditReaderImplementor versionsReader;
		private readonly object entityId;
		private readonly long revision;
		private readonly EntitiesConfigurations entCfg;

		public ToOneDelegateSessionImplementor(IAuditReaderImplementor versionsReader,
											   object entityId, long revision, 
											   AuditConfiguration verCfg) 
			: base(versionsReader.SessionImplementor)
		{
			this.versionsReader = versionsReader;
			this.entityId = entityId;
			this.revision = revision;
			entCfg = verCfg.EntCfg;
		}

		protected override object DoImmediateLoad(string entityName)
		{
			if (entCfg.GetNotVersionEntityConfiguration(entityName) == null)
			{
				// audited relation, look up entity with envers
				return versionsReader.Find(entityName, entityId, revision);
			}
			// notAudited relation, look up entity with hibernate
			return SessionDelegate.ImmediateLoad(entityName, entityId);
		}
	}

}


