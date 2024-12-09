using System.Net.Mail;
using System.Text.RegularExpressions;
using Auth.Helpers.Interfaces;

namespace Auth.Helpers;

public class DataValidationService : IDataValidationService
{
    //This only check the syntax, we should validate emails by sending them!
    public bool IsMailSyntaxValid(string mail, out string message)
    {
        if (string.IsNullOrWhiteSpace(mail) || mail.Length < 6 || mail.Length > 254)
        {
            message = "Invalid mail length.";
            return false;
        }

        try
        {
            var address = new MailAddress(mail);
            var isValid = (address.Address == mail);
            if (!isValid)
            {
                message = "Invalid email format!";
                return false;
            }
            message = "Valid mail";
            return true;
        }
        catch (FormatException)
        {
            message = "Invalid email format.";
            return false;
        }
    }

    public bool IsPasswordValid(string password, out string message)
    {
        switch (password.Length)
        {
            case < 8:
                message = "Password is too short.";
                return false;
            case > 64:
                message = "Password is too long.";
                return false;
        }

        var hasUpperCaseRegex = new Regex("[A-Z]");
        var hasLowerCaseRegex = new Regex("[a-z]");
        var hasDigitRegex = new Regex(@"\d");
        var hasSpecialCharactersRegex = new Regex(@"[^a-zA-Z0-9]");

        if (!hasUpperCaseRegex.IsMatch(password))
        {
            message = "Password must have an upper case letter.";
            return false;
        }

        if (!hasLowerCaseRegex.IsMatch(password))
        {
            message = "Password must have a lower case letter.";
            return false;
        }

        if (!hasDigitRegex.IsMatch(password))
        {
            message = "Password must have a digit.";
            return false;
        }

        if (!hasSpecialCharactersRegex.IsMatch(password))
        {
            message = "Password must have a special character.";
            return false;
        }

        message = "";
        return true;
    }
}