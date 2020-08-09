using MvvmCross.Commands;
using MvvmCross.Navigation;
using Songer.Core.Models;
using Songer.Core.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Songer.Core.ViewModels
{
    sealed public class PlaylistDetailViewModel : BaseVM<Playlist>
    {
        public PlaylistDetailViewModel(
              IMvxNavigationService navigationService,
              IPlaylistService playlistService,
              IPlayerService playerService,
              IPopupService popupService)
              : base(navigationService)
        {
            _playlistService = playlistService;
            _popupService = popupService;

            PlayerService = playerService;
        }

        public override Task Initialize()
        {
            Title = Entity.Title;
            Songs = new ObservableCollection<Song>(Entity.Songs);

            return Task.CompletedTask;
        }

        #region Fields and properties

        private readonly IPlaylistService _playlistService;
        private readonly IPopupService _popupService;

        public IPlayerService PlayerService { get; }

        public ObservableCollection<Song> Songs { get; set; }

        public string Title { get; private set; }
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

        public IMvxCommand DeletePlaylistCommand => new MvxCommand(async () => await _popupService.Wait(DeletePlaylist));
        public IMvxCommand EditPlaylistCommand => new MvxCommand(EditPlaylist);
        public IMvxCommand ToPlayerPageCommand => new MvxCommand(ToPlayerPage);

        private async void DeletePlaylist()
        {
            await _playlistService.DeletePlaylist(Entity);

            await _navigationService.Close(this);
        }

        private async void EditPlaylist()
        {
            await _navigationService.Navigate<EditPlaylistViewModel, Playlist>(Entity);

            await _navigationService.Close(this);
        }

        private async void ToPlayerPage()
        {
            await _navigationService.Navigate<PlayerViewModel>();
        }

        #endregion
    }
}
