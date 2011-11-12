using System;
using munimji.data.model.instruments;

namespace munimji.data.model.events {
    public class EquityTrade : Event {
        public EquityTrade() : base(EventTypes.EquityTrade) {}

        public virtual Security Security { get; set; }
        public virtual DateTime SettlementDate { get; set; }
        public virtual decimal Commission { get; set; }
    }
}