using FluentValidation;
using System.ComponentModel;

namespace Songer.Core.Validation
{
    public class BaseValidator<T> : AbstractValidator<T>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
