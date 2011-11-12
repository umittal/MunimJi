using munimji.core.persistance;

namespace munimji.data.model.instruments {
    public class Instrument : EntityBase {
        public Instrument() {}

        protected Instrument(InstrumentTypes instrumentType) {
            InstrumentType = instrumentType;
        }

        public virtual InstrumentTypes InstrumentType { get; set; }
    }

    public enum InstrumentTypes {
        Unspecified,
        Security,
        Future
    }
}