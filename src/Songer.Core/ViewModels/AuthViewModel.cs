using MvvmCross.Commands;
using MvvmCross.Navigation;
using Songer.Core.Services;
using Songer.Core.Validation;

namespace Songer.Core.ViewModels
{
    sealed public class AuthViewModel : BaseVM
    {
        public AuthViewModel(
            IMvxNavigationService navigationService,
            IAuthenticateService authenticateService,
            IPopupService popupService,
            AuthValidator validator)
            : base(navigationService)
        {
            _authenticateService = authenticateService;
            _popupService = popupService;

            Validator = validator;
        }

        #region Fields and properties

        private string _login;
        public string Login
        {
            get => _login;
            set
            {
                _login = value;

                Validator.ValidateLogin(_login);
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;

                Validator.ValidatePassword(_password);
            }
        }

        public AuthValidator Validator { get; }
        private readonly IAuthenticateService _authenticateService;
        private readonly IPopupService _popupService;

        #endregion

        #region Commands

        public IMvxCommand SignInCommand => new MvxCommand(SignIn);
        public IMvxCommand ToRegistrationCommand => new MvxCommand(ToRegistrationPage);
        public IMvxCommand ToSettingsCommand => new MvxCommand(ToSettings);

        private async void SignIn()
        {
            if (Validator.IsValid)
            {
                await _popupService.PushSpinnerAsync();

                if (await _authenticateService.Auth(_login, _password))
                {
                    await _navigationService.Navigate<RootViewModel>();
                    await _navigationService.Close(this);
                }

                await _popupService.PopAsync();
            }
        }

        private async void ToRegistrationPage()
        {
            await _navigationService.Navigate<RegViewModel>();
        }

        private async void ToSettings()
        {
            await _navigationService.Navigate<SettingsViewModel>();
        }

        #endregion
    }
}