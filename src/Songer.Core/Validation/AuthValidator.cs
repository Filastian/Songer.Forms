using FluentValidation;
using System;
using System.Linq;

namespace Songer.Core.Validation
{
    public class AuthValidator : BaseValidator<string>
    {
        public AuthValidator()
        {
            RuleFor(x => x).Length(6, 30)
                           .WithMessage("length must be from 6 to 30 chars");

            RuleSet("Login", () => {
                RuleFor(x => x).Must(ContainsOnlyDigitsAndLetters)
                               .WithMessage("login can consist only of letters and numbers");
            });

            RuleSet("Password", () => {
                RuleFor(x => x).Must(ContainsOnlyDigitsAndLetters)
                               .WithMessage("password can consist only of letters and numbers");
            });
        }

        private bool ContainsOnlyDigitsAndLetters(string pattern)
        {
            foreach (char x in pattern)
            {
                if (!Char.IsDigit(x) && !Char.IsLetter(x)) return false;
            }

            return true;
        }

        public string LoginError { get; set; } = null;
        public string PasswordError { get; set; } = null;

        public bool IsValid => (LoginError == String.Empty) && (PasswordError == String.Empty);

        public void ValidateLogin(string login)
        {
            var res = this.Validate(login, ruleSet: "default, Login");

            LoginError = res.IsValid ? String.Empty : 
                res.Errors.FirstOrDefault().ToString();
        }

        public void ValidatePassword(string password)
        {
            var res = this.Validate(password, ruleSet: "default, Password");

            PasswordError = res.IsValid ? String.Empty :
                res.Errors.FirstOrDefault().ToString();
        }
    }
}
