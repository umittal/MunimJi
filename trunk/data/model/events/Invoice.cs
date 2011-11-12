using System;
using munimji.data.model.entities;

namespace munimji.data.model.events {
    public class Invoice : Event {
        public Invoice()
            : base(EventTypes.Invoice) {}

        public virtual LegalEntity LegalEntity { get; set; }
        public virtual string InvoiceNumber { get; set; }
        public virtual DateTime InvoinceDate { get; set; }
    }
}