using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace munimji.services.wcf.entities.events {
    [ServiceContract]
    public interface IInvoiceService {
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        InvoiceDto GetInvoice(long id);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<InvoiceDto> GetInvoices(int startIndex, int pageSize);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        void SaveInvoice(InvoiceDto invoice);
    }
}