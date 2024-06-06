using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Shared.Validations;

public static class UserValidationHelper
{
    private static readonly Regex UsernameSpecialCharRegex = new(@"^[a-zA-Z0-9]*$");

    public static void ValidateUser(string username, string password)
    {
        try
        {
            IsUsernameValid(username);
            IsPasswordComplex(password);
        }
        catch (ValidationException e)
        {
            throw new ValidationException(e.Message);
        }
    }

    public static bool IsUsernameValid(string username)
    {
        if (username.Length < 6)
            throw new ValidationException("Username must be longer than 6 characters");

        if (username.Length > 20)
            throw new ValidationException("Username must be shorter than 20 characters");

        if (ContainsSpecialCharacters(username))
            throw new ValidationException("Username must not contain special characters");

        return true;
    }

    public static bool IsPasswordComplex(string password)
    {
        var hasUpperCase = new Regex(@"[A-Z]");
        var hasLowerCase = new Regex(@"[a-z]");
        var hasDigit = new Regex(@"\d");
        var hasSpecialChar = new Regex(@"[\W_]");
        var hasMinimum8Chars = new Regex(@".{8,}");

        if (!hasUpperCase.IsMatch(password))
            throw new ValidationException("Password must contain at least one uppercase letter");

        if (!hasLowerCase.IsMatch(password))
            throw new ValidationException("Password must contain at least one lowercase letter");

        if (!hasDigit.IsMatch(password))
            throw new ValidationException("Password must contain at least one digit");

        if (!hasSpecialChar.IsMatch(password))
            throw new ValidationException("Password must contain at least one special character");

        if (!hasMinimum8Chars.IsMatch(password))
            throw new ValidationException("Password must be at least 8 characters long");

        return true;
    }

    private static bool ContainsSpecialCharacters(string input)
    {
        return !UsernameSpecialCharRegex.IsMatch(input);
    }
}