using System.Text.RegularExpressions;

namespace Shared.Validators
{
    public abstract class Validator
    {
        public static bool BeValidNanoId(string? str)
        {
            return Regex.IsMatch(str, @"^[a-zA-Z0-9_-]+$");
        }
    }
}
