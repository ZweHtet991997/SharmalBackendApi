namespace SharmalRealEstateSystem.Shared.Services.ValidationServices.PasswordPolicy;

public class PasswordPolicyChecker
{
    public static string ValidatePassword(string password)
    {
        if (password.Length < 6)
        {
            return MessageResource.MinLengthRequired();
        }

        if (password.Length >= 15)
        {
            return MessageResource.MaxLengthExceeded();
        }

        if (!password.Any(char.IsUpper))
        {
            return MessageResource.UpperCaseRequired();
        }

        if (!password.Any(char.IsDigit))
        {
            return MessageResource.DigitRequired();
        }

        if (!password.Any(c => !char.IsLetterOrDigit(c)))
        {
            return MessageResource.SpecialCharRequired();
        }

        return string.Empty; // Return empty string if the password is valid
    }
}
