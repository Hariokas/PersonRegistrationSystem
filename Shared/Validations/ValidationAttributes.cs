using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace Shared.Validations;

public class DateOfBirthValidation : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (!DateTime.TryParse((string)value, out var date))
            return new ValidationResult("Invalid Date of Birth format.");

        return date < DateTime.Today
            ? ValidationResult.Success
            : new ValidationResult("Date of Birth must be a past date.");
    }
}

public class PhoneNumberValidation : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var phoneNumber = value as string;
        if (!string.IsNullOrEmpty(phoneNumber) && Regex.IsMatch(phoneNumber, @"^\+?[1-9]\d{1,14}$"))
            return ValidationResult.Success;

        return new ValidationResult("Invalid phone number format.");
    }
}

public class ProfilePictureValidation : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is not IFormFile file) return new ValidationResult("Profile picture is required.");

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var extension = Path.GetExtension(file.FileName).ToLower();

        if (!allowedExtensions.Contains(extension))
            return new ValidationResult("Invalid file type. Only .jpg, .jpeg, and .png files are allowed.");

        return file.Length > 5 * 1024 * 1024
            ? // 5MB limit
            new ValidationResult("File size cannot exceed 5MB.")
            : ValidationResult.Success;
    }
}

public class PersonalCodeValidation : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var personIdCode = value as string;
        if (string.IsNullOrEmpty(personIdCode)) return ValidationResult.Success;

        if (!IsValidPersonalCode(personIdCode)) return new ValidationResult("Invalid personal code.");

        return ValidationResult.Success;
    }

    private bool IsValidPersonalCode(string personIdCode)
    {
        return IsLengthValid(personIdCode) &&
               IsFormatValid(personIdCode) &&
               IsBirthDateValid(personIdCode) &&
               IsCheckDigitValid(personIdCode);
    }

    private bool IsLengthValid(string personIdCode)
    {
        return personIdCode.Length == 11;
    }

    private bool IsFormatValid(string personIdCode)
    {
        return Regex.IsMatch(personIdCode, @"^\d{11}$");
    }

    private bool IsBirthDateValid(string personIdCode)
    {
        try
        {
            var centuryAndGender = int.Parse(personIdCode.Substring(0, 1));
            var year = int.Parse(personIdCode.Substring(1, 2));
            var month = int.Parse(personIdCode.Substring(3, 2));
            var day = int.Parse(personIdCode.Substring(5, 2));
            var fullYear = GetFullYear(centuryAndGender, year);

            var birthDate = new DateTime(fullYear, month, day);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private int GetFullYear(int centuryAndGender, int year)
    {
        return centuryAndGender switch
        {
            1 or 2 => 1800 + year,
            3 or 4 => 1900 + year,
            5 or 6 => 2000 + year,
            7 or 8 => 2100 + year,
            _ => throw new ArgumentException("Invalid century and gender indicator in personal code.")
        };
    }

    private bool IsCheckDigitValid(string personIdCode)
    {
        int[] weights1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1 };
        int[] weights2 = { 3, 4, 5, 6, 7, 8, 9, 1, 2, 3 };

        var checkDigit = CalculateCheckDigit(personIdCode, weights1);
        if (checkDigit == 10)
        {
            checkDigit = CalculateCheckDigit(personIdCode, weights2);
            if (checkDigit == 10) checkDigit = 0;
        }

        return checkDigit == int.Parse(personIdCode.Substring(10, 1));
    }

    private int CalculateCheckDigit(string personIdCode, int[] weights)
    {
        var sum = 0;
        for (var i = 0; i < 10; i++) sum += (personIdCode[i] - '0') * weights[i];
        return sum % 11;
    }
}

public class NonEmptyStringAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var str = value as string;
        if (string.IsNullOrWhiteSpace(str?.Trim()))
            return new ValidationResult("The field cannot be empty or contain only whitespaces.");

        return ValidationResult.Success;
    }
}