using System;
using munimji.data.model.instruments;

namespace munimji.data.model.events {
    public class BondTrade : Event {
        public BondTrade()
            : base(EventTypes.BondTrade) {}

        public virtual Security Security { get; set; }
        public virtual DateTime PayDate { get; set; }
        public virtual decimal AccruedInterest { get; set; }
    }
}