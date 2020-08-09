using PropertyChanged;
using Songer.Core.Models;
using Songer.Core.Player;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Songer.Core.Services
{
    public class PlayerService : IPlayerService
    {
        public PlayerService(
            ISongService songService,
            IPlayer player)
        {
            _songService = songService;
            _player = player;

            _player.SongIsOver += async () =>
            {
                if (PlayedSong == null) return;

                if (IsLoop)
                    await Play();
                else
                    NextSong();
            };

            StartPlayer();
        }

        #region Fields and properties

        private readonly ISongService _songService;
        private readonly IPlayer _player;

        public bool IsPause { get; private set; }
        public bool IsLoop { get; private set; }
        public bool IsShuffle { get; private set; }

        [AlsoNotifyFor(nameof(PlayedSongTitle))]
        public Song PlayedSong { get; private set; }
        private IList<Song> _savedSonglist;
        public IList<Song> PlayedSonglist { get; private set; }

        [AlsoNotifyFor(nameof(PlayerTimeTitle))]
        public int CurrentPlaybackTime { get; private set; }
        [AlsoNotifyFor(nameof(PlayerTimeTitle))]
        public int TotalPlaybackTime { get; private set; }

        public string PlayedSongTitle => PlayedSong != null ?
                $"{PlayedSong.Title} - {PlayedSong.Performer}" :
                String.Empty;

        public string PlayerTimeTitle => PlayedSong != null ?
                String.Format("{0} - {1}",
                    TimeSpan.FromMilliseconds(CurrentPlaybackTime).ToString(@"mm\:ss"),
                    TimeSpan.FromMilliseconds(TotalPlaybackTime).ToString(@"mm\:ss")) :
                String.Empty;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Business logic

        public async Task Load(Song song)
        {
            Stop();

            PlayedSonglist = new List<Song> { PlayedSong };
            PlayedSong = song;

            await Play();
        }

        public async Task Load(IList<Song> list)
        {
            Stop();

            if (list != PlayedSonglist)
                PlayedSonglist = new List<Song>(list);

            PlayedSong = PlayedSonglist[0];

            await Play();
        }

        public async Task Load(Song song, IList<Song> list)
        {
            Stop();

            if(list != PlayedSonglist)
                PlayedSonglist = new List<Song>(list);

            PlayedSong = song;

            await Play();
        }

        public void SeekTo(int msec)
        {
            _player.PlaybackPosition = msec;
        }

        private async Task Play()
        {
            var stream = await _songService.GetSong(PlayedSong.Id);

            await _player.Play(stream);

            //await _player.Play(new MemoryStream());

            IsPause = false;

            TotalPlaybackTime = _player.TotalPlayback;
        }

        private void StartPlayer()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(1000);

                    if (PlayedSong == null) continue;

                    CurrentPlaybackTime = _player.PlaybackPosition;
                }
            });
        }

        public async void PrevSong()
        {
            if (PlayedSong != PlayedSonglist.First())
            {
                PlayedSong = PlayedSonglist[PlayedSonglist.IndexOf(PlayedSong) - 1];
            }
            else
            {
                PlayedSong = PlayedSonglist.Last();
            }

            await Play();
        }

        public void Resume()
        {
            IsPause = false;
            _player.Resume();
        }

        public void Pause()
        {
            IsPause = true;
            _player.Pause();
        }

        public void Stop()
        {
            IsPause = true;
            _player.Stop();
        }

        public async void NextSong()
        {
            if (PlayedSong != PlayedSonglist.Last())
            {
                PlayedSong = PlayedSonglist[PlayedSonglist.IndexOf(PlayedSong) + 1];
            }
            else
            {
                PlayedSong = PlayedSonglist.First();
            }

            await Play();
        }

        public void ChangeLoop()
        {
            // TODO: 3 states of loop:
            // * play only one song
            // * play list in a circle
            // * play list and stop

            IsLoop = !IsLoop;
        }

        public async void ShuffleList()
        {
            Random rnd = new Random();

            if(!IsShuffle)
            {
                _savedSonglist = new List<Song>(PlayedSonglist);
                PlayedSonglist = PlayedSonglist.OrderBy(x => rnd.Next()).ToList();
            }
            else
            {
                PlayedSonglist = _savedSonglist;
                _savedSonglist = null;
            }

            await Load(PlayedSonglist);

            IsShuffle = !IsShuffle;
        }

        public void Reset()
        {
            Stop();

            PlayedSong = null;
            PlayedSonglist = null;
        }

        public void OnSongRemoved(Song song)
        {
            if (PlayedSonglist == null) return;

            var tempSong = PlayedSonglist.Where(s => s.Id == song.Id)
                                         .FirstOrDefault();

            if (tempSong != null)
            {
                if (PlayedSong == tempSong)
                {
                    if (PlayedSonglist.Count > 1)
                        NextSong();
                    else
                    {
                        Stop();
                        PlayedSong = null;
                    }
                }

                PlayedSonglist.Remove(tempSong);
            }
        }

        #endregion
    }
}
