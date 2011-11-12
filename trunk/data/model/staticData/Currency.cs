using munimji.core.persistance;
using munimji.core.persistance.annoations;

namespace munimji.data.model.staticData {
    public class Currency : EntityBase {
        [Nullable]
        public virtual string ShortCode { get; set; }

        [Nullable]
        public virtual string LongName { get; set; }

        [Nullable]
        public virtual int Precision { get; set; }

        [Nullable]
        public virtual string Symbol { get; set; }

        public virtual LegalJurisdiction LegalJurisdiction { get; set; }
    }
}