using System.ComponentModel.DataAnnotations;

namespace PayFor.ViewModels
{
    public class TokenResponseViewModel
    {
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}