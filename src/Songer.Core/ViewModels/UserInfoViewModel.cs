using MvvmCross.Commands;
using MvvmCross.Navigation;
using Songer.Core.Models;
using Songer.Core.Services;

namespace Songer.Core.ViewModels
{
    sealed public class UserInfoViewModel : BaseVM
    {
        public UserInfoViewModel(
            IMvxNavigationService navigationService,
            IAuthenticateService authenticateService,
            IPlayerService playerService,
            ILogOutService logOutService)
            : base(navigationService)
        {
            _authenticateService = authenticateService;
            _logOutService = logOutService;
            PlayerService = playerService;
        }

        #region Fields and properties

        private readonly IAuthenticateService _authenticateService;
        private readonly ILogOutService _logOutService;
        public IPlayerService PlayerService { get; }

        public User User => _authenticateService.LoggedUser;

        #endregion

        #region Commands

        public IMvxCommand ExitCommand => new MvxCommand(Exit);
        public IMvxCommand ToPlayerPageCommand => new MvxCommand(ToPlayerPage);
        public IMvxCommand ChangePasswordCommand => new MvxCommand(ChangePassword);
        public IMvxCommand EditUserCommand => new MvxCommand(EditUser);

        private async void Exit()
        {
            _logOutService.LogOut();

            await _navigationService.Navigate<AuthViewModel>();
        }

        private async void ToPlayerPage()
        {
            await _navigationService.Navigate<PlayerViewModel>();
        }

        private async void ChangePassword()
        {
            await _navigationService.Navigate<ChangePasswordViewModel>();
        }

        private async void EditUser()
        {
            await _navigationService.Navigate<EditUserViewModel>();
        }

        #endregion
    }
}
