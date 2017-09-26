using System;
using System.Text.RegularExpressions;

namespace Rhym
{
    public static class Validater
    {
        public static bool EmailValidator(string input)
        {
            var emailRegex = "[A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,6}";
            var isValid = Regex.Match (input, emailRegex).Success;
            return isValid;
        }
    }
}
