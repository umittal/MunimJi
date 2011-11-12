namespace munimji.data.model.instruments {
    public class Security : Instrument {
        public Security() : base(InstrumentTypes.Security) {}

        public virtual string Sedol { get; set; }
        public virtual string Isin { get; set; }
        public virtual string BbTicker { get; set; }
        public virtual string Cusip { get; set; }
    }
}