using MvvmCross.Commands;
using MvvmCross.Navigation;
using Songer.Core.Models;
using Songer.Core.Services;
using System.Threading.Tasks;

namespace Songer.Core.ViewModels
{
    sealed public class EditUserViewModel : BaseVM
    {
        public EditUserViewModel(
            IMvxNavigationService navigationService,
            IAuthenticateService authenticateService,
            IPopupService popupService,
            IUserService userService)
            : base(navigationService)
        {
            _authenticateService = authenticateService;
            _popupService = popupService;
            _userService = userService;
        }

        public override Task Initialize()
        {
            Name = User.Name;

            return Task.CompletedTask;
        }

        #region Fields and properties

        private readonly IAuthenticateService _authenticateService;
        private readonly IPopupService _popupService;
        private readonly IUserService _userService;

        public User User => _authenticateService.LoggedUser;

        public string Name { get; set; }

        #endregion

        #region Commands

        public IMvxCommand SaveCommand => new MvxCommand(async () => await _popupService.Wait(Save));
        public IMvxCommand CancelCommand => new MvxCommand(Cancel);

        private async void Save()
        {
            await _userService.EditUser(name: Name);

            User.Name = Name;

            await _navigationService.Close(this);
        }

        private async void Cancel()
        {
            await _navigationService.Close(this);
        }

        #endregion
    }
}
