namespace Auth.Helpers.Interfaces;

public interface IDataValidationService
{
    bool IsMailSyntaxValid(string mail, out string message);
    bool IsPasswordValid(string password, out string message);
}