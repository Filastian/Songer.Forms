using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace Songer.Core.Player
{
    public delegate void SongIsOverHandler();

    public interface IPlayer
    {
        int PlaybackPosition { get; set; }
        int TotalPlayback { get; }
        bool IsPlaying { get; }

        Task Play(Stream stream);

        void Resume();
        void Stop();
        void Pause();

        event SongIsOverHandler SongIsOver;
    }
}