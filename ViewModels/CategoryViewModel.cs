using System.ComponentModel.DataAnnotations;

namespace PayFor.ViewModels
{
    public class CategoryViewModel
    {
        public string Id { get; set; }
        [Required]
        [StringLength(100,MinimumLength = 3)]
        public string Name { get; set; }
    }
}