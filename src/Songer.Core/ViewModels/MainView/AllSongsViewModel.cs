using MvvmCross.Commands;
using MvvmCross.Navigation;
using Songer.Core.Models;
using Songer.Core.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Songer.Core.ViewModels
{
    sealed public class AllSongsViewModel : BaseVM
    {
        public AllSongsViewModel(
            IMvxNavigationService navigationService,
            ISongService songService,
            IAuthenticateService authenticateService,
            IPlayerService playerService,
            IPopupService popupService,
            IPlaylistService playlistService)
            :base(navigationService)
        {
            _songService = songService;
            _authenticateService = authenticateService;
            _popupService = popupService;
            _playlistService = playlistService;

            PlayerService = playerService;
        }

        public override async Task Initialize()
        {
            await UpdateSongs();
        }

        private async Task UpdateSongs()
        {
            Songs = new ObservableCollection<Song>(await _songService.GetAllSongs());

            IsRefreshing = false;
        }

        #region Fields and properties

        private readonly ISongService _songService;
        private readonly IAuthenticateService _authenticateService;
        private readonly IPopupService _popupService;
        private readonly IPlaylistService _playlistService;

        public IPlayerService PlayerService { get; }

        public ObservableCollection<Song> Songs { get; set; }

        public bool IsAdmin => _authenticateService.LoggedUser.Role == "Admin";

        public bool IsRefreshing { get; set; }

        public Song SelectedSong
        {
            set
            {
                if (value != null &&
                    value != PlayerService.PlayedSong)
                    PlayerService.Load(value, Songs);
            }
        }

        #endregion  

        #region Commands

        public IMvxCommand DeleteSongCommand => new MvxCommand<Song>(async (song) => await _popupService.Wait(DeleteSong, song));
        public IMvxCommand EditSongCommand => new MvxCommand<Song>(song => EditSong(song));
        public IMvxCommand ToPlayerPageCommand => new MvxCommand(ToPlayerPage);
        public IMvxCommand RefreshCommand => new MvxCommand(async () => await UpdateSongs());

        private async void DeleteSong(Song song)
        {
            await _songService.DeleteSong(song);
            _playlistService.DeleteSongFromPlaylistCashe(song);
            Songs.Remove(song);
            PlayerService.OnSongRemoved(song);
        }

        private async void EditSong(Song song)
        {
            await _navigationService.Navigate<EditSongViewModel, Song>(song);
        }

        private async void ToPlayerPage()
        {
            await _navigationService.Navigate<PlayerViewModel>();
        }

        #endregion
    }
}