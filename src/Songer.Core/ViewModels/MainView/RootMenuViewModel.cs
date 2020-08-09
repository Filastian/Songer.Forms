using MvvmCross.Commands;
using MvvmCross.Navigation;
using Songer.Core.Models;
using Songer.Core.Services;
using System.Collections.ObjectModel;

namespace Songer.Core.ViewModels
{
    sealed public class RootMenuViewModel : BaseVM
    {
        public RootMenuViewModel(
            IMvxNavigationService navigationService,
            IAuthenticateService authenticateService,
            ILogOutService logOutService) 
            : base(navigationService)
        {
            _authenticateService = authenticateService;
            _logOutService = logOutService;

            MenuItems = new ObservableCollection<RootViewMenuItem>()
            {
                new RootViewMenuItem { Title = "Home", TargetType = typeof(HomeViewModel) },
                new RootViewMenuItem { Title = "All songs", TargetType = typeof(AllSongsViewModel) },
                new RootViewMenuItem { Title = "My songs", TargetType = typeof(MySongsViewModel) },
                new RootViewMenuItem { Title = "My playlists", TargetType = typeof(MyPlaylistsViewModel) }
            };
        }

        #region Fields and properties

        private readonly IAuthenticateService _authenticateService;
        private readonly ILogOutService _logOutService;

        public User User => _authenticateService.LoggedUser;

        public ObservableCollection<RootViewMenuItem> MenuItems { get; set; }

        public RootViewMenuItem Detail
        {
            set
            {
                if (value != null)
                    _navigationService.Navigate(value.TargetType);
            }
        }

        #endregion

        #region Commands

        public IMvxCommand LogOutCommand => new MvxCommand(LogOut);
        public IMvxCommand ToUserInfoCommand => new MvxCommand(ToUserInfo);

        private async void LogOut()
        {
            _logOutService.LogOut();

            await _navigationService.Navigate<AuthViewModel>();
        }

        private async void ToUserInfo()
        {
            await _navigationService.Navigate<UserInfoViewModel>();
        }

        #endregion
    }
}
