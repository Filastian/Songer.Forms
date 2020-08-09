using MvvmCross.Commands;
using MvvmCross.Navigation;
using Songer.Core.Models;
using Songer.Core.Services;
using Songer.Core.Validation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Songer.Core.ViewModels
{
    sealed public class EditPlaylistViewModel : BaseVM<Playlist>
    {
        public EditPlaylistViewModel(
            IMvxNavigationService navigationService,
            IPlaylistService playlistService,
            ISongService songService,
            IPlayerService playerService,
            IPopupService popupService,
            CreatePlaylistValidator validator)
            : base(navigationService)
        {
            _playlistService = playlistService;
            _songService = songService;
            _popupService = popupService;

            PlayerService = playerService;
            Validator = validator;
        }

        public override async Task Initialize()
        {
            Title = Entity.Title;

            var allSongs = await _songService.GetAllSongs();
            Songs = new ObservableCollection<SongViewItem>();

            foreach (var song in allSongs)
            {
                Songs.Add(new SongViewItem { Song = song, Checked = Entity.Songs.Any(c => c.Id == song.Id) });
            }
        }

        #region Fields and properties

        private readonly IPlaylistService _playlistService;
        private readonly ISongService _songService;
        private readonly IPopupService _popupService;

        public IPlayerService PlayerService { get; }
        public CreatePlaylistValidator Validator { get; }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;

                Validator.ValidateTitle(_title);
            }
        }

        public ObservableCollection<SongViewItem> Songs { get; set; }

        public SongViewItem SelectedSong
        {
            set
            {
                if (value != null)
                    PlayerService.Load(value.Song, Songs.Select(s => s.Song).ToList());
            }
        }

        #endregion

        #region Commands

        public IMvxCommand SaveCommand => new MvxCommand(Save);
        public IMvxCommand CancelCommand => new MvxCommand(Cancel);
        public IMvxCommand ToPlayerPageCommand => new MvxCommand(ToPlayerPage);

        private async void Save()
        {
            if (Validator.IsValid)
            {
                await _popupService.PushSpinnerAsync();

                var checkedSongsIds = Songs.Where(c => c.Checked == true)
                                           .Select(s => s.Song)
                                           .Select(i => i.Id)
                                           .ToList();

                await _playlistService.EditPlaylist(_title, checkedSongsIds, Entity.Id);

                await _navigationService.Close(this);

                await _popupService.PopAsync();
            }
        }

        private async void Cancel()
        {
            await _navigationService.Close(this);
        }

        private async void ToPlayerPage()
        {
            await _navigationService.Navigate<PlayerViewModel>();
        }

        #endregion
    }
}
