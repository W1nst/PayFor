using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PayFor.ViewModels
{
    public class GroupRowViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100,MinimumLength = 3)]
        public string Name { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
        public string AuthorName { get; set; }
    }
}
