using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using munimji.core.lib.extentions;

namespace munimji.core.persistance {
    /// <summary>
    ///   Wraps the SchemaExport and SchemaUpdate tools from NHibernate, providing a
    ///   single easy to use class to manage database schemas.
    /// </summary>
    public static class SchemaManager {
        public static void Execute(Assembly contextAssembly, Options options) {
            var script = (options & Options.Script) == Options.Script;
            var verbose = (options & Options.Verbose) == Options.Verbose;
            var execute = (options & Options.Execute) == Options.Execute;

            var entityContextType =
                contextAssembly.GetExportedTypes().Where(x => x.IsSubclassOf(typeof (EntityContextBase))).First();
            var model = ContextService.DiscoverModelFor(entityContextType);

            using (var session = model.CreateSession()) {
                var scriptBuilder = new StringBuilder();
                using (var tran = session.BeginTransaction()) {
                    if ((options & Options.Drop) == Options.Drop) {
                        if(verbose) {
                            Console.WriteLine("Dropping database...");
                        }
                        var dropScript = model.Drop(session, execute);
                        scriptBuilder.AppendLine(dropScript);
                    }

                    if ((options & Options.Create) == Options.Create) {
                        if (verbose)
                        {
                            Console.WriteLine("Creating database...");
                        }
                        var createScript = model.Create(session, execute);
                        scriptBuilder.AppendLine(createScript);
                    }

                    if ((options & Options.Update) == Options.Update) {
                        if (verbose)
                        {
                            Console.WriteLine("Upgrading database...");
                        }
                        var updateScript = model.Update(session, execute);
                        scriptBuilder.AppendLine(updateScript);
                    }

                    //if ((options & Options.Verify) == Options.Verify) {
                    //    model.Update();
                    //}

                    tran.Commit();
                }

                if (verbose) {
                    Console.Write(scriptBuilder.ToString());
                }

                if (script) {
                    using (
                        var sr =
                            new StreamWriter(string.Format("{0}_script.sql",
                                                           entityContextType.Name.ToLowerCase(true)))) {
                        sr.Write(scriptBuilder.ToString());
                        sr.Close();
                    }
                }
            }
        }


        [Flags]
        public enum Options {
            Drop = 1 << 0,
            Create = 1 << 1,
            Update = 1 << 2,
            Verify = 1 << 3,
            Script = 1 << 4,
            Execute = 1 << 5,
            Verbose = 1 << 6
        }
    }
}