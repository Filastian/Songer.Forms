using MvvmCross.Commands;
using MvvmCross.Navigation;
using Songer.Core.Models;
using Songer.Core.Services;
using System.Threading.Tasks;

namespace Songer.Core.ViewModels
{
    sealed public class EditSongViewModel : BaseVM<Song>
    {
        public EditSongViewModel(
            IMvxNavigationService navigationService,
            ISongService songService,
            IPlaylistService playlistService,
            IPopupService popupService)
            : base(navigationService)
        {
            _songService = songService;
            _playlistService = playlistService;
            _popupService = popupService;
        }

        public override Task Initialize()
        {
            Title = Entity.Title;
            Performer = Entity.Performer;

            return Task.CompletedTask;
        }

        #region Fields and properties

        private readonly ISongService _songService;
        private readonly IPlaylistService _playlistService;
        private readonly IPopupService _popupService;

        public string Title { get; set; }
        public string Performer { get; set; }

        #endregion

        #region Commands

        public IMvxCommand SaveCommand => new MvxCommand(async () => await _popupService.Wait(Save));
        public IMvxCommand CancelCommand => new MvxCommand(Cancel);

        private async void Save()
        {
            var song = new Song { Id = Entity.Id, Title = Title, Performer = Performer };

            await _songService.UpdateSong(song);
            _playlistService.UpdateSongInPlaylistCashe(song);
            await _navigationService.Close(this);
        }

        private async void Cancel()
        {
            await _navigationService.Close(this);
        }

        #endregion
    }
}
