using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Constraints;
using PayFor.Models;

namespace PayFor.ViewModels
{
    public class PaymentEditViewModel
    {
        public int Id { get; set; }
        [StringLength(250)]
        public string Note { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        
    }
}
