using munimji.core.persistance;
using munimji.data.model.staticData;

namespace munimji.data.model.entities {
    public class LegalEntity : EntityBase {
        public virtual LegalEntityTypes LegalEntityType { get; set; }
        public virtual string LongName { get; set; }
        public virtual LegalJurisdiction LegalJurisdiction { get; set; }

        public LegalEntity() {}

        protected LegalEntity(LegalEntityTypes legalEntityType) {
            LegalEntityType = legalEntityType;
        }
    }

    public enum LegalEntityTypes {
        Unspecified,
        Bank,
        Investor
    }
}