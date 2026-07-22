using LaudaryMis.Models;
using LaundryMIS.Models;
using LaundryMIS.Models.LaudaryMis.Models;

namespace LaudaryMis.ViewModels
{
    public class InvoiceViewModel
    {
        public InvoiceMaster Invoice { get; set; }

        public List<InvoiceDetailModel> Items { get; set; }

        public List<InvoiceDocument> Documents { get; set; }
    }
}
