using munimji.core.persistance.conventions;

namespace munimji.data.model {
    public sealed class DatabaseConvention : IDatabaseConvention {
        public string Catalog {
            get { return "munimjidb"; }
        }

        public string Schema {
            get { return "dbo"; }
        }

        public string TablePrefix {
            get { return "mj_"; }
        }

        public string TablePostfix {
            get { return ""; }
        }
    }
}