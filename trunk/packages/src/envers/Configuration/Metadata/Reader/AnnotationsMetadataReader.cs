﻿using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Envers.Configuration.Attributes;
using NHibernate.Envers.Configuration.Store;
using NHibernate.Envers.Tools.Reflection;
using NHibernate.Mapping;

namespace NHibernate.Envers.Configuration.Metadata.Reader
{
	/// <summary>
	/// A helper class to read versioning meta-data from annotations on a persistent class.
	/// </summary>
	public sealed class AnnotationsMetadataReader 
	{
		private readonly IMetaDataStore _metaDataStore;
		private readonly GlobalConfiguration globalCfg;
		private readonly PersistentClass pc;

		/// <summary>
		/// This object is filled with information read from annotations and returned by the <code>getVersioningData</code>
		/// method.
		/// </summary>
		private readonly ClassAuditingData _auditData;

		public AnnotationsMetadataReader(IMetaDataStore metaDataStore,
									GlobalConfiguration globalCfg, 
									PersistentClass pc)
		{
			_metaDataStore = metaDataStore;
			this.globalCfg = globalCfg;
			this.pc = pc;

			_auditData = new ClassAuditingData();
		}

		public ClassAuditingData GetAuditData()
		{
			if (pc.ClassName == null)
			{
				return _auditData;
			}
			try
			{
				var typ = pc.MappedClass;

				var defaultStore = getDefaultAudited(typ);
				if (defaultStore != ModificationStore.None)
				{
					_auditData.SetDefaultAudited(true);
				}

				var ar = new AuditedPropertiesReader(_metaDataStore, defaultStore,
													 new PersistentClassPropertiesSource(typ, this), _auditData,
													 globalCfg, string.Empty);
				ar.Read();

				addAuditTable(typ);
				addAuditSecondaryTables(typ);
			}
			catch (Exception e)
			{
				throw new MappingException(e);
			}


			return _auditData;
		}

		private ModificationStore getDefaultAudited(System.Type typ) 
		{
			var defaultAudited = _metaDataStore.ClassMeta<AuditedAttribute>(typ);

			return defaultAudited != null ? defaultAudited.ModStore : ModificationStore.None;
		}

		private void addAuditTable(System.Type typ)
		{
			var auditTable = _metaDataStore.ClassMeta<AuditTableAttribute>(typ);
			_auditData.AuditTable = auditTable ?? getDefaultAuditTable();
		}

		private void addAuditSecondaryTables(System.Type typ)
		{
			IEntityMeta entityMeta;
			if (_metaDataStore.EntityMetas.TryGetValue(typ, out entityMeta))
			{
				var joinAuditTableAttributes = entityMeta.ClassMetas.OfType<JoinAuditTableAttribute>().ToList();
				foreach (var joinAuditTableAttribute in joinAuditTableAttributes)
				{
					_auditData.JoinTableDictionary.Add(joinAuditTableAttribute.JoinTableName, joinAuditTableAttribute.JoinAuditTableName);
				}
			}
		}

		private readonly AuditTableAttribute defaultAuditTable = new AuditTableAttribute(string.Empty);
		  

		private AuditTableAttribute getDefaultAuditTable() 
		{
			return defaultAuditTable;
		}

		private class PersistentClassPropertiesSource : IPersistentPropertiesSource 
		{
			private readonly AnnotationsMetadataReader parent;
			private readonly IEnumerable<DeclaredPersistentProperty> _declaredPersistentProperties;

			public PersistentClassPropertiesSource(System.Type typ, AnnotationsMetadataReader parent) 
			{ 
				this.parent = parent;
				_declaredPersistentProperties = PropertyAndMemberInfo.PersistentInfo(typ, parent.pc.PropertyIterator, true);
			}

			public IEnumerable<DeclaredPersistentProperty> DeclaredPersistentProperties
			{
				get { return _declaredPersistentProperties; }
			}


			public Property VersionedProperty
			{
				get { return parent.pc.Version; }
			}
		}
	}
}