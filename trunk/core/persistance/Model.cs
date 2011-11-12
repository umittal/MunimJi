using System;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace munimji.core.persistance {
    internal class Model {
        private readonly Configuration nhConfig;

        public Model(Configuration nhConfig) {
            this.nhConfig = nhConfig;
        }

        internal ISession CreateSession() {
            return nhConfig.BuildSessionFactory().OpenSession();
        }

        internal string Update(ISession session, bool execute) {
            var sqlText = new StringBuilder();
            var updater = new SchemaUpdate(nhConfig);
            Action<string> lineAction = line => sqlText.AppendLine(line);

            updater.Execute(lineAction, execute);

            return sqlText.ToString();
        }

        /// <summary>
        ///   Drops the schema from the database
        /// </summary>
        /// <returns>DDL script for dropping the schema</returns>
        internal string Drop(ISession session, bool execute) {
            var sqlText = new StringBuilder();
            var schema = new SchemaExport(nhConfig);
            Action<string> lineAction = line => sqlText.AppendLine(line);

            schema.Execute(lineAction, execute, true);

            return sqlText.ToString();
        }

        /// <summary>
        ///   Creates the schema in the database
        /// </summary>
        /// <returns>DDL script used to create the schema</returns>
        internal string Create(ISession session, bool execute) {
            var sqlText = new StringBuilder();
            var schema = new SchemaExport(nhConfig);
            Action<string> lineAction = line => sqlText.AppendLine(line);

            schema.Execute(lineAction, execute, false);

            return sqlText.ToString();
        }
    }
}