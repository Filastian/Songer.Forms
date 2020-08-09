using FluentValidation;
using System;
using System.Linq;

namespace Songer.Core.Validation
{
    public class ChangePasswordValidator : BaseValidator<string>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x).Length(6, 30)
                           .WithMessage("length must be from 6 to 30 chars");

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

        public string OldPasswordError { get; set; } = null;
        public string NewPasswordError { get; set; } = null;
        public string ReturnedPasswordError { get; set; } = null;

        public bool IsValid => (OldPasswordError == String.Empty) && (NewPasswordError == String.Empty) && (ReturnedPasswordError == String.Empty);

        public void ValidateOldPassword(bool isCorrect)
        {
            OldPasswordError = isCorrect ? String.Empty :
                "wrong password";
        }

        public void ValidateNewPassword(string password)
        {
            var res = this.Validate(password, ruleSet: "default, Password");

            NewPasswordError = res.IsValid ? String.Empty :
                    res.Errors.FirstOrDefault().ToString();
        }

        public void ValidateReturnedPassword(string firstPassword, string secondPassword)
        {
            ReturnedPasswordError = firstPassword == secondPassword ?
                                    String.Empty :
                                    "password mismatch";
        }
    }
}
