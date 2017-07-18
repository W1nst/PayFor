using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PayFor.Models
{
    public class Group
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100,MinimumLength = 3)]
        public string Name { get; set; }
        public DateTime CreateDateTime { get; set; }
        public List<Payment> Payments { get; set; }
        public List<UserGroup> UserGroups { get; set; }
        public string AuthorUserId { get; set; }
        public User AuthorUser { get; set; }
    }
}
