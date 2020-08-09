using MvvmCross.Commands;
using MvvmCross.Navigation;
using Songer.Core.Models;
using Songer.Core.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Songer.Core.ViewModels
{
    sealed public class HomeViewModel : BaseVM
    {
        public HomeViewModel(
            IMvxNavigationService navigationService,
            IPlayerService playerService,
            ISongService songService,
            IPlaylistService playlistService,
            IAuthenticateService authenticateService)
            : base(navigationService)
        {
            _songService = songService;
            _playlistService = playlistService;
            _authenticateService = authenticateService;

            PlayerService = playerService;
        }

        public async override Task Initialize()
        {
            Songs = new ObservableCollection<Song>(await _songService.GetUserSongs());
            Playlists = new ObservableCollection<Playlist>(await _playlistService.GetUserPlaylists());
        }

        #region Fields and properties

        private readonly ISongService _songService;
        private readonly IPlaylistService _playlistService;
        private readonly IAuthenticateService _authenticateService;

        public User User => _authenticateService.LoggedUser;

        public bool IsAdmin => User.Role == "Admin";

        public IPlayerService PlayerService { get; }

        public ObservableCollection<Song> Songs { get; private set; }
        public ObservableCollection<Playlist> Playlists { get; private set; }

        #endregion

        #region Commands

        public IMvxCommand ToPlayerPageCommand => new MvxCommand(ToPlayerPage);
        public IMvxCommand ToMySongsCommand => new MvxCommand(async () => await _navigationService.Navigate<MySongsViewModel>());
        public IMvxCommand ToMyPlaylsitsCommand => new MvxCommand(async () => await _navigationService.Navigate<MyPlaylistsViewModel>());

        private async void ToPlayerPage()
        {
            await _navigationService.Navigate<PlayerViewModel>();
        }

        #endregion
    }
}
