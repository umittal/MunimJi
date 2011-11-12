using System.Linq;
using munimji.core.persistance;
using munimji.core.persistance.annoations;
using munimji.data.model.entities;
using munimji.data.model.events;
using munimji.data.model.instruments;
using munimji.data.model.persons;
using munimji.data.model.staticData;
using NHibernate;
using NHibernate.Linq;

namespace munimji.data.context {
    [ContextDefination(ConnectionStringName = "munimjiContext")]
    public sealed class MunimJiContext : EntityContextBase {

        public MunimJiContext(ISession session) : base(session) {}


        public IQueryable<Person> Persons {
            get { return Session.Query<Person>(); }
        }

        public IQueryable<SystemUser> SystemUsers {
            get { return Session.Query<SystemUser>(); }
        }

        public IQueryable<Instrument> Instruments {
            get { return Session.Query<Instrument>(); }
        }

        public IQueryable<Security> Securities {
            get { return Session.Query<Security>(); }
        }

        public IQueryable<Event> Events {
            get { return Session.Query<Event>(); }
        }

        public IQueryable<BondTrade> BondTrades {
            get { return Session.Query<BondTrade>(); }
        }

        public IQueryable<EquityTrade> EquityTrades {
            get { return Session.Query<EquityTrade>(); }
        }

        public IQueryable<Invoice> Invoices {
            get { return Session.Query<Invoice>(); }
        }

        public IQueryable<BankAccount> BankAccounts {
            get { return Session.Query<BankAccount>(); }
        }

        public IQueryable<Currency> Currencies {
            get { return Session.Query<Currency>(); }
        }

        public IQueryable<LegalJurisdiction> LegalJurisdictions {
            get { return Session.Query<LegalJurisdiction>(); }
        }

        public IQueryable<LegalEntity> LegalEntities {
            get { return Session.Query<LegalEntity>(); }
        }

    }
}