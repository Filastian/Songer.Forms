using MvvmCross.Commands;
using MvvmCross.Navigation;
using Songer.Core.Services;
using Songer.Core.Validation;

namespace Songer.Core.ViewModels
{
    sealed public class ChangePasswordViewModel : BaseVM
    {
        public ChangePasswordViewModel(
            IMvxNavigationService navigationService,
            IChangePasswordService changePasswordService,
            IPopupService popupService,
            ChangePasswordValidator validator)
            : base(navigationService)
        {
            _changePasswordService = changePasswordService;
            _popupService = popupService;

            Validator = validator;
        }

        #region Fields and propertys

        private readonly IChangePasswordService _changePasswordService;
        private readonly IPopupService _popupService;

        public ChangePasswordValidator Validator { get; }

        private string _oldPassword;
        public string OldPassword
        {
            get => _oldPassword;
            set
            {
                _oldPassword = value;

                CheckOldPassword();
            }
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;

                Validator.ValidateNewPassword(_newPassword);
            }
        }

        private string _returnedPassword;
        public string ReturnedPassword
        {
            get => _returnedPassword;
            set
            {
                _returnedPassword = value;

                Validator.ValidateReturnedPassword(_newPassword, _returnedPassword);
            }
        }

        #endregion

        private async void CheckOldPassword()
        {
            Validator.ValidateOldPassword(await _changePasswordService.IsCorrectPassword(_oldPassword));
        }

        #region Commands

        public IMvxCommand SaveCommand => new MvxCommand(Save);
        public IMvxCommand CancelCommand => new MvxCommand(Cancel);

        private async void Save()
        {
            if (Validator.IsValid)
            {
                await _popupService.PushSpinnerAsync();

                await _changePasswordService.ChangePassword(NewPassword);

                await _navigationService.Close(this);

                await _popupService.PopAsync();
            }
        }

        private async void Cancel()
        {
            await _navigationService.Close(this);
        }

        #endregion

    }
}
