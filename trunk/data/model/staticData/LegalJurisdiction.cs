using munimji.core.persistance;

namespace munimji.data.model.staticData {
    public class LegalJurisdiction : EntityBase {
        public virtual string ShortCode { get; set; }
        public virtual string LongName { get; set; }
    }
}