using System;
using System.Collections.Generic;
using System.Linq;
using munimji.core.persistance;
using munimji.data.context;
using munimji.data.model;
using munimji.data.model.entities;
using munimji.data.model.events;
using munimji.data.model.persons;
using munimji.data.model.staticData;

namespace munimji.apps.sandbag {
    internal class EntityFramework {
        internal static void Initialize() {
            Console.WriteLine("Creating database..");
            EntityFrameworkGlobals.DefaultConvention = new DatabaseConvention();
            SchemaManager.Execute(typeof (MunimJiContext).Assembly,
                                  SchemaManager.Options.Drop | SchemaManager.Options.Create |
                                  SchemaManager.Options.Execute | SchemaManager.Options.Script |
                                  SchemaManager.Options.Verbose);
        }


        internal static void SetupForFirstTimeUse() {
            try {
                Console.WriteLine("[{0}] Creating new entities..", DateTime.Now.ToString("hh:mm:ss.fff tt"));
                using (var db = ContextService.GetContext<MunimJiContext>()) {
                    SetupData(db);
                }
                Console.WriteLine("[{0}] Done!", DateTime.Now.ToString("hh:mm:ss.fff tt"));
                Console.WriteLine("[{0}] Creating new entities..", DateTime.Now.ToString("hh:mm:ss.fff tt"));
            }
            catch (Exception ex) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Oops! Error encountered:");
                Console.WriteLine("Message:\n{0}", ex.Message);
                Console.WriteLine("Stack trace:\n{0}", ex.StackTrace);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            Console.WriteLine("Press <Enter> to end.");
            Console.ReadLine();
        }

        private static void SetupData(EntityContextBase db) {
            var person1 = new Person {FirstName = "Mayank", LastName = "Gupta"};


            var country1 = new LegalJurisdiction {
                                                     LongName = "United States of America",
                                                     ShortCode = "USA"
                                                 };

            var entity1 = new Investor {
                                           LongName = "Some Capital",
                                           LegalJurisdiction = country1
                                       };

            var currency1 = new Currency {
                                             LongName = "United States Dollar",
                                             ShortCode = "USD",
                                             LegalJurisdiction = country1
                                         };
            var invoice1 = new Invoice {
                                           InvoiceNumber = "Inv/01",
                                           InvoinceDate = DateTime.Today,
                                           LegalEntity = entity1,
                                           Amount = 1000000,
                                           Currency = currency1,
                                           AsOfDate = DateTime.Today,
                                       };
            db.Save(new List<EntityBase> {person1, country1, entity1, currency1, invoice1});

            var mayank = db.Set<Person>().Where(x => x.FirstName == "Mayank").Single();
            mayank.MiddleName = "Edited";
            db.Save(new[] {mayank});
        }
    }
}