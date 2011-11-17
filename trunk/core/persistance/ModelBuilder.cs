using System;
using System.Configuration;
using System.Reflection;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using munimji.core.persistance.annoations;
using munimji.core.persistance.conventions;
using munimji.core.persistance.listners;
using munimji.core.persistance.mapping;
using NHibernate.Cfg;
using NHibernate.Envers.Configuration;
using NHibernate.Event;
using NHibernate.Mapping;
using NHibernate.Tool.hbm2ddl;
using Configuration = NHibernate.Cfg.Configuration;
using Environment = NHibernate.Cfg.Environment;
using Fluently = NHibernate.Envers.Configuration.Fluent.Fluently;

namespace munimji.core.persistance {
    internal static class ModelBuilder {
        internal static Model Build(string connectionStringName, Assembly[] mappedAssemplies) {
            if (EntityFrameworkGlobals.DefaultConvention == null) {
                throw new ApplicationException("Please set default for EntityFrameworkGlobals.DefaultConvention");
            }
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
            var dataProvider = MsSqlConfiguration.MsSql2008.ConnectionString(connectionString);

            var enversConfig = Fluently.Configure();
            var pocoConfig = FluentNHibernate.Cfg.Fluently.Configure()
                .Database(dataProvider);

#if false
            pocoConfig = AttachePocoEvents(pocoConfig);
#endif
            pocoConfig.Mappings(m => m.AutoMappings
                                         .Add(AutoMap.AssemblyOf<EntityBase>(new AutomappingConfiguration())
                                                  .Conventions.AddFromAssemblyOf<ColumnNameConvention>()
                                                  .OverrideAll(x => x.IgnoreProperties(y =>
                                                                                       y.MemberInfo.IsDefined(
                                                                                           typeof (IgnoreAttribute),
                                                                                           false))))
                                         .ExportTo(@"\mappings"));

            enversConfig.AutoMapAssemblyOf<EntityBase>();

            foreach (var assembly in mappedAssemplies) {
                var mappintAssembly = assembly;
                pocoConfig.Mappings(m => m.AutoMappings
                                             .Add(AutoMap.Assembly(mappintAssembly, new AutomappingConfiguration())
                                                      //.IgnoreBase(typeof(EntityBase))
                                                      .Conventions.AddFromAssemblyOf<ColumnNameConvention>()
                                                      .OverrideAll(x => x.IgnoreProperties(y =>
                                                                                           y.MemberInfo.IsDefined(
                                                                                               typeof (IgnoreAttribute),
                                                                                               false)))));
                enversConfig.AutoMapAssembly(mappintAssembly);
            }

            var nhConfig = pocoConfig.BuildConfiguration();

#if DEBUG

            nhConfig.Properties.Add(Environment.ShowSql, "true");
            nhConfig.Properties.Add(Environment.FormatSql, "true");
            nhConfig.Properties.Add(Environment.GenerateStatistics, "true");
            nhConfig.Properties.Add(Environment.Hbm2ddlKeyWords, "auto-quote");
#endif


            nhConfig.Properties.Add(ConfigurationKey.AuditTablePrefix, string.Empty); // default
            nhConfig.Properties.Add(ConfigurationKey.AuditTableSuffix, "_history"); // default _AUD
            nhConfig.Properties.Add(ConfigurationKey.RevisionFieldName, "revision"); // default
            nhConfig.Properties.Add(ConfigurationKey.RevisionTypeFieldName, "revisionType"); // default
            nhConfig.Properties.Add(ConfigurationKey.DoNotAuditOptimisticLockingField, "false"); // default true
            nhConfig.Properties.Add(ConfigurationKey.StoreDataAtDelete, "true"); // default false
            nhConfig.Properties.Add(ConfigurationKey.RevisionTableName, "revision_metadata");
            nhConfig.Properties.Add(ConfigurationKey.RevisionTableIdFieldName, "revision");
            nhConfig.Properties.Add(ConfigurationKey.RevisionTableTimestampFieldName, "timestamp");

            nhConfig = enversConfig.IntegrateWithNHibernate(nhConfig);

            //BuildSchema(nhConfig, connectionStringName, SchemaAction.Export, true);
            
            return new Model(nhConfig);
        }

        private static FluentConfiguration AttachePocoEvents(FluentConfiguration config) {
            return config.ExposeConfiguration(x => x.EventListeners.PreInsertEventListeners =
                                                   new IPreInsertEventListener[] {new AuditListener()})
                .ExposeConfiguration(x => x.EventListeners.PreUpdateEventListeners =
                                          new IPreUpdateEventListener[] {new AuditListener()});
        }

        private static void BuildSchema(Configuration config, string connectionStringName, SchemaAction action, bool execute) {
            if ((action & SchemaAction.Drop) == SchemaAction.Drop) {}

            if ((action & SchemaAction.Update) == SchemaAction.Update) {
                new SchemaUpdate(config).Execute(true, execute);
            }

            if ((action & SchemaAction.Export) == SchemaAction.Export) {
                new SchemaExport(config)
                    .SetOutputFile(connectionStringName + "schema.sql")
                    .Create(false, execute);
            }

            if ((action & SchemaAction.Validate) == SchemaAction.Validate) {
                new SchemaValidator(config).Validate();
            }
        }
    }
}