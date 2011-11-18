using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using munimji.core.lib.extentions;
using munimji.core.persistance;
using munimji.data.context;
using munimji.data.model.entities;
using munimji.data.model.events;
using munimji.data.model.staticData;

namespace munimji.services.wcf.entities.events {
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class InvoiceService : IInvoiceService {
        public InvoiceDto GetInvoice(long id)
        {
            InvoiceDto dto = null;
            using (var db = ContextService.GetContext<MunimJiContext>())
            {
                var query = from invoice in db.Invoices where invoice.Id == id select invoice;
                if(!query.IsEmpty())
                {
                    dto = InvoiceDto.Create(query.First());
                }
            }
            return dto;
        }

        public IEnumerable<InvoiceDto> GetInvoices(int startIndex, int pageSize) {
            using (var db = ContextService.GetContext<MunimJiContext>())
            {
                var invoices = db.Invoices.Skip(startIndex).Take(pageSize);
                return invoices.Select(InvoiceDto.Create);
            }
        }

        public void SaveInvoice(InvoiceDto invoiceDto) {
            using (var db = ContextService.GetContext<MunimJiContext>())
            {
                var invoice = invoiceDto.Id > 0
                                  ? db.Invoices.Where(x => x.Id == invoiceDto.Id).First()
                                  : new Invoice();
                //invoice.IsDeleted = invoiceDto.IsDeleted;
                invoice.AsOfDate = invoiceDto.AsOfDate;
                invoice.Currency = db.LookupById<Currency>(invoiceDto.CurrencyId);
                invoice.Amount = invoiceDto.Amount;
                invoice.LegalEntity = db.LookupById<LegalEntity>(invoiceDto.EntityId);
                invoice.InvoiceNumber = invoiceDto.InvoiceNumber;
                invoice.InvoinceDate = invoiceDto.InvoinceDate;

                db.Save(new[] {invoice});
            }
        }
    }
}