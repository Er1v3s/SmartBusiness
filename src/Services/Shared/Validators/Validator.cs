using System.Text.RegularExpressions;

namespace Shared.Validators
{
    public abstract class Validator
    {
        public static bool BeValidNanoId(string? str)
        {
            if(string.IsNullOrEmpty(str)) return false;
            return Regex.IsMatch(str, @"^[a-zA-Z0-9_-]+$");
        }
    }
}
