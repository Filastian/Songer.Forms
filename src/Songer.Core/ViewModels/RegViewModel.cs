using MvvmCross.Commands;
using MvvmCross.Navigation;
using Songer.Core.Services;
using Songer.Core.Validation;

namespace Songer.Core.ViewModels
{
    sealed public class RegViewModel : BaseVM
    {
        public RegViewModel(
            IMvxNavigationService navigationService,
            IAuthenticateService authenticateService,
            IRegistrateService registrateService,
            IPopupService popupService,
            RegValidator validator)
            : base(navigationService)
        {
            _authenticateService = authenticateService;
            _registrateService = registrateService;
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

        public string Name { get; set; }

        public RegValidator Validator { get; set; }
        private readonly IAuthenticateService _authenticateService;
        private readonly IRegistrateService _registrateService;
        private readonly IPopupService _popupService;

        #endregion

        #region Commands

        public IMvxCommand SignUpCommand => new MvxCommand(SignUp);
        public IMvxCommand BackCommand => new MvxCommand(Back);

        private async void SignUp()
        {
            if (Validator.IsValid)
            {
                await _popupService.PushSpinnerAsync();

                if (!await _registrateService.Registrate(_login, _password, Name))
                {
                    await _popupService.PopAsync();
                    return;
                }

                if (await _authenticateService.Auth(_login, _password))
                {
                    await _navigationService.Navigate<RootViewModel>();
                    await _navigationService.Close(this);
                }

                await _popupService.PopAsync();
            }
        }

        private async void Back()
        {
            await _navigationService.Close(this);
        }

        #endregion
    }
}
