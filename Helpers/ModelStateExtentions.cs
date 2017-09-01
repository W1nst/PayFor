using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PayFor.ExtensionMethods
{
    public static class ModelStateDictionaryExtensions
    {
        public static string ErrorsToString(this ModelStateDictionary model)
        {
            return string.Join(" | ", model.Values
                                    .SelectMany(v => v.Errors)
                                    .Select(e => e.ErrorMessage));
        }
    }   
}