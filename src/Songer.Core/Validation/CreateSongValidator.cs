using FluentValidation;
using System;
using System.Linq;

namespace Songer.Core.Validation
{
    public class CreateSongValidator : BaseValidator<string>
    {
        public CreateSongValidator()
        {
            RuleSet("Title", () => {
                RuleFor(x => x).Length(6, 30)
                               .WithMessage("length must be from 6 to 30 chars");
            });

            RuleSet("Performer", () => {
                RuleFor(x => x).NotEmpty()
                               .WithMessage("performer must be entered");
            });

            RuleSet("Path", () => {
                RuleFor(x => x).NotEmpty()
                               .WithMessage("path must be entered");
            });
        }

        public string TitleError { get; set; } = null;
        public string PerformerError { get; set; } = null;
        public string PathError { get; set; } = null;

        public bool IsValid => (TitleError == String.Empty) && (PerformerError == String.Empty) && (PathError == String.Empty);

        public void ValidateTitle(string title)
        {
            var res = this.Validate(title, ruleSet: "Title");

            TitleError = res.IsValid ? String.Empty :
                res.Errors.FirstOrDefault().ToString();
        }

        public void ValidatePerformer(string path)
        {
            var res = this.Validate(path, ruleSet: "Performer");

            PerformerError = res.IsValid ? String.Empty :
                res.Errors.FirstOrDefault().ToString();
        }

        public void ValidatePath(string path)
        {
            var res = this.Validate(path, ruleSet: "Path");

            PathError = res.IsValid ? String.Empty :
                res.Errors.FirstOrDefault().ToString();
        }
    }
}
