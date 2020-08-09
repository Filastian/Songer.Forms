using System;
using System.Threading.Tasks;

namespace Songer.Core.Services
{
    public interface IPopupService
    {
        Task Wait(Action action);
        Task Wait<T>(Action<T> action, T value);

        Task PushSpinnerAsync();
        Task PopAsync();
    }
}