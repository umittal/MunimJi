using System;
using System.Runtime.Serialization;
using munimji.data.model.events;

namespace munimji.services.wcf.entities.events {
    /// <summary>
    /// DateTime should always have a value
    /// </summary>
    [DataContract]
    public class InvoiceDto {
        [DataMember]
        public long Id { get; set; }
        [DataMember]
        public long Revision { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public string Creator { get; set; }
        [DataMember]
        public DateTime Timestamp { get; set; }
        [DataMember]
        public DateTime AsOfDate { get; set; }
        [DataMember]
        public long CurrencyId { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public long EntityId { get; set; }
        [DataMember]
        public string InvoiceNumber { get; set; }
        [DataMember]
        public DateTime InvoinceDate { get; set; }

        private InvoiceDto() {
            Timestamp = DateTime.UtcNow;
            AsOfDate = DateTime.UtcNow;
            InvoinceDate = DateTime.UtcNow;
        }

        public static InvoiceDto Create(Invoice invoice) {
            var invoiceDto = new InvoiceDto
                                 {
                                     Id = invoice.Id,
                                     //Revision = invoice.Revision,
                                     //IsDeleted = invoice.IsDeleted,
                                     //Creator = invoice.Creator,
                                     Timestamp = invoice.Timestamp,
                                     AsOfDate = invoice.AsOfDate,
                                     CurrencyId = invoice.Currency.Id,
                                     Amount = invoice.Amount,
                                     EntityId = invoice.LegalEntity.Id,
                                     InvoiceNumber = invoice.InvoiceNumber,
                                     InvoinceDate = invoice.InvoinceDate
                                 };
            return invoiceDto;
        }
    }
}