using MvvmCross.Commands;
using MvvmCross.Navigation;
using Songer.Core.Client;
using Songer.Core.Services;
using System;

namespace Songer.Core.ViewModels
{
    sealed public class SettingsViewModel : BaseVM
    {
        public SettingsViewModel(
            IMvxNavigationService navigationService, 
            IRestClient client,
            IPopupService popupService)
            : base(navigationService)
        {
            _client = client;
            _popupService = popupService;

            Url = _client.BaseAddress.ToString();
        }

        #region Fields and properties

        public string Url { get; set; }

        private readonly IRestClient _client;
        private readonly IPopupService _popupService;

        #endregion

        #region Commands

        public IMvxCommand SaveCommand => new MvxCommand(async () => await _popupService.Wait(Save));
        public IMvxCommand CancelCommand => new MvxCommand(Cancel);

        private async void Save()
        {
            _client.BaseAddress = new Uri(Url);

            await _navigationService.Close(this);
        }

        private async void Cancel()
        {
            await _navigationService.Close(this);
        }

        #endregion
    }
}