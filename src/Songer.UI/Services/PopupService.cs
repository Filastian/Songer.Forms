using Rg.Plugins.Popup.Services;
using Songer.Core.Services;
using Songer.UI.Views.Controls;
using System;
using System.Threading.Tasks;

namespace Songer.UI.Services
{
    public class PopupService : IPopupService
    {
        public async Task Wait(Action action)
        {
            await PushSpinnerAsync();

            action.Invoke();

            await PopAsync();
        }

        public async Task Wait<T>(Action<T> action, T value)
        {
            await PushSpinnerAsync();

            action.Invoke(value);

            await PopAsync();
        }

        public async Task PushSpinnerAsync()
        {
            await PopupNavigation.Instance.PushAsync(new Spinner());
        }

        public async Task PopAsync()
        {
            await PopupNavigation.Instance.PopAsync(false);
        }
    }
}
