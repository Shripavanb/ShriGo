using Microsoft.AspNetCore.Identity;

namespace ShriGo.Pages.Helpers
{
    public class PasswordHelper
    {
        private readonly PasswordHasher<string> _passwordHasher;

        public PasswordHelper()
        {
            _passwordHasher = new PasswordHasher<string>();
        }

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }

        public bool VerifyPassword(string hashedPassword, string enteredPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(
                null,
                hashedPassword,
                enteredPassword
            );

            return result == PasswordVerificationResult.Success;
        }

        public bool IsHashed(string password)
        {
            return !string.IsNullOrWhiteSpace(password)
                   && password.Length > 50;
        }
    }
}