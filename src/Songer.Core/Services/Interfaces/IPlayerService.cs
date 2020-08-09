using Songer.Core.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Songer.Core.Services
{
    public interface IPlayerService : INotifyPropertyChanged
    {
        bool IsPause { get; }
        bool IsLoop { get; }
        bool IsShuffle { get; }

        Song PlayedSong { get; }
        IList<Song> PlayedSonglist { get; }

        string PlayedSongTitle { get; }
        string PlayerTimeTitle { get; }

        int TotalPlaybackTime { get; }
        int CurrentPlaybackTime { get; }

        Task Load(Song song);
        Task Load(Song song, IList<Song> list);

        void SeekTo(int msec);
        void ChangeLoop();
        void ShuffleList();
        void Reset();

        void PrevSong();
        void Resume();
        void Stop();
        void Pause();
        void NextSong();

        void OnSongRemoved(Song song);
    }
}
