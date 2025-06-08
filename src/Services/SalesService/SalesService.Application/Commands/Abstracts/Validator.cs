using System.Text.RegularExpressions;

namespace SalesService.Application.Commands.Abstracts
{
    public abstract class Validator
    {
        public static bool BeValidString(string? str)
        {
            return Regex.IsMatch(str, @"^[\p{L}\d]+$");
        }
    }
}
