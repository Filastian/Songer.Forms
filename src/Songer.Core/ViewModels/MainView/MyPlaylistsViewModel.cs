using MvvmCross.Commands;
using MvvmCross.Navigation;
using Songer.Core.Models;
using Songer.Core.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Songer.Core.ViewModels
{
    sealed public class MyPlaylistsViewModel : BaseVM
    {
        public MyPlaylistsViewModel(
              IMvxNavigationService navigationService,
              IPlaylistService playlistService,
              IPlayerService playerService)
              : base(navigationService)
        {
            _playlistService = playlistService;

            PlayerService = playerService;
        }

        public override async Task Initialize()
        {
            await UpdatePlaylists();
        }

        private async Task UpdatePlaylists()
        {
            Playlists = new ObservableCollection<Playlist>(await _playlistService.GetUserPlaylists());

            IsRefreshing = false;
        }

        #region Fields and properties

        private readonly IPlaylistService _playlistService;

        public IPlayerService PlayerService { get; }

        public bool IsRefreshing { get; set; }

        public ObservableCollection<Playlist> Playlists { get; set; }

        public Playlist SelectedPlaylist
        {
            set
            {
                if(value != null)
                    _navigationService.Navigate<PlaylistDetailViewModel, Playlist>(value);
            }
        }

        #endregion

        #region Commands

        public IMvxCommand ToCreatePlaylistPageCommand => new MvxCommand(ToCreatePlaylistPage);
        public IMvxCommand RefreshCommand => new MvxCommand(async () => await UpdatePlaylists());
        public IMvxCommand ToPlayerPageCommand => new MvxCommand(ToPlayerPage);

        private async void ToCreatePlaylistPage()
        {
            await _navigationService.Navigate<CreatePlaylistViewModel>();
        }

        private async void ToPlayerPage()
        {
            await _navigationService.Navigate<PlayerViewModel>();
        }

        #endregion
    }
}
