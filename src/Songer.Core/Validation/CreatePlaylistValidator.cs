using FluentValidation;
using System;
using System.Linq;

namespace Songer.Core.Validation
{
    public class CreatePlaylistValidator : BaseValidator<string>
    {
        public CreatePlaylistValidator()
        {
            RuleSet("Title", () => {
                RuleFor(x => x).Length(6, 30)
                               .WithMessage("length must be from 6 to 30 chars");
            });
        }

        public string TitleError { get; set; } = null;

        public bool IsValid => TitleError == String.Empty;

        public void ValidateTitle(string title)
        {
            var res = this.Validate(title, ruleSet: "Title");

            TitleError = res.IsValid ? String.Empty :
                res.Errors.FirstOrDefault().ToString();
        }
    }
}
