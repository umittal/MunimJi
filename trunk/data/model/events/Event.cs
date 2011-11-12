using System;
using munimji.core.persistance;
using munimji.data.model.staticData;

namespace munimji.data.model.events {
    public class Event : EntityBase {
        public Event() {}

        protected Event(EventTypes eventType) {
            EventType = eventType;
        }

        public virtual EventTypes EventType { get; set; }
        public virtual DateTime AsOfDate { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual decimal Amount { get; set; }
    }

    public enum EventTypes {
        Unspecified,
        EquityTrade,
        BondTrade,
        Invoice
    }
}