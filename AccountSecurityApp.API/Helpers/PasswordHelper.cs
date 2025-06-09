using System;
using System.Text.RegularExpressions;

namespace AccountSecurityApp.API.Helpers
{
    public class PasswordHelper
    {
        private static readonly Regex PasswordPattern = new Regex(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+{}\[\]:;,.<>?]).{8,}$");

        public static bool IsPasswordValid(string password, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(password))
            {
                errorMessage = "Password cannot be empty.";
                return false;
            }

            if (!PasswordPattern.IsMatch(password))
            {
                errorMessage = "Password must be at least 8 characters long and include at least one uppercase letter, one lowercase letter, one digit, and one special character.";
                return false;
            }

            return true;
        }
    }
}


   