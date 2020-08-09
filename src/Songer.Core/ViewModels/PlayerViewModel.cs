using MvvmCross.Commands;
using MvvmCross.Navigation;
using Songer.Core.Models;
using Songer.Core.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Songer.Core.ViewModels
{
    sealed public class PlayerViewModel : BaseVM, INotifyPropertyChanged
    {
        public PlayerViewModel(
            IMvxNavigationService navigationService,
            IPlayerService playerService,
            ISongService songService)
            : base(navigationService)
        {
            PlayerService = playerService;
            _songService = songService;
        }

        public override async Task Initialize()
        {
            await UpdatePlaylist();
        }

        private async Task UpdatePlaylist()
        {
            var mySongs = await _songService.GetUserSongs();
            Songs = new ObservableCollection<SongViewItem>();

            foreach (var song in PlayerService.PlayedSonglist)
            {
                Songs.Add(new SongViewItem { Song = song, Checked = mySongs.Any(c => c.Id == song.Id) });
            }
        }

        #region Fields and properties

        public IPlayerService PlayerService { get; }

        private readonly ISongService _songService;

        public ObservableCollection<SongViewItem> Songs { get; set; }

        public int ChangeablePlaybackTime
        {
            set
            {
                if (value != -1.0)
                    PlayerService.SeekTo(value);
            }
        }

        public SongViewItem SelectedSong
        {
            set
            {
                if (value != null &&
                    value.Song != PlayerService.PlayedSong)
                    PlayerService.Load(value.Song, PlayerService.PlayedSonglist);
            }
        }

        #endregion

        #region Commands

        public IMvxCommand PrevSongCommand => new MvxCommand(PlayerService.PrevSong);
        public IMvxCommand ResumePauseCommand => new MvxCommand(ResumePause);
        public IMvxCommand NextSongCommand => new MvxCommand(PlayerService.NextSong);

        public IMvxCommand LoopPlayingCommand => new MvxCommand(PlayerService.ChangeLoop);
        public IMvxCommand ShuffleCommand => new MvxCommand(Shuffle);
        public IMvxCommand AddOrDeleteSongCommand => new MvxCommand<SongViewItem>(p => AddOrDeleteSong(p));

        private void ResumePause()
        {
            if (PlayerService.IsPause)
                PlayerService.Resume();
            else
                PlayerService.Pause();
        }

        private async void Shuffle()
        {
            PlayerService.ShuffleList();
            await UpdatePlaylist();
        }

        private async void AddOrDeleteSong(SongViewItem item)
        {
            if (item.Checked)
                await _songService.DeleteFromUserSongs(item.Song);
            else
                await _songService.AddToUserSongs(item.Song);

            item.Checked = !item.Checked;
        }

        #endregion
    }
}
