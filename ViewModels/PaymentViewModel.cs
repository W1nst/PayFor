using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Constraints;
using PayFor.Models;

namespace PayFor.ViewModels
{
    public class PaymentViewModel
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public Category Category { get; set; }
        public UserRowViewModel User { get; set; }
        public GroupRowViewModel Group{ get; set; }
    }
}
