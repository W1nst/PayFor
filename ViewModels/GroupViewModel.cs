using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayFor.ViewModels
{
    public class GroupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
        public string AuthorName { get; set; }
        public List<PaymentViewModel> Payments { get; set; }
        public List<UserRowViewModel> Users{ get; set; }
    }
}
