using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace PayFor.Models
{
    public class User : IdentityUser
    {
        public List<UserGroup> UserGroups { get; set; }
        public List<Payment> Payments { get; set; }
        [StringLength(100,MinimumLength = 2)]
        public string FirstName { get; set; }
        [StringLength(100, MinimumLength = 2)]
        public string LastName { get; set; }
    }
}