using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace УчетПлатежей.Models
{
    public class PaymentListViewModel
    {
        public IEnumerable<Payment> Payments { get; set; }
        public string FullName { get; set; }
    }
}
