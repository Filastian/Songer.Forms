using Android.Media;
using Songer.Core.Player;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(Songer.Droid.Audio.Player))]
namespace Songer.Droid.Audio
{
    public class Player : IPlayer
    {
        public Player()
        {
            _player = new MediaPlayer();

            _player.Completion += (sender, e) => SongIsOver?.Invoke();
        }

        public int PlaybackPosition 
        { 
            get => _player.CurrentPosition;
            set => _player.SeekTo(value);
        }
        public int TotalPlayback => _player.Duration;
        public bool IsPlaying => _player.IsPlaying;

        private readonly MediaPlayer _player;

        public event SongIsOverHandler SongIsOver;

        private bool testFlag;

        public async Task Play(System.IO.Stream stream)
        {
            Stop();

            _player.Reset();

            await _player.SetDataSourceAsync(new StreamMediaDataSource(stream));
            /*var fileName = testFlag ? "test1.mp3" : "test2.mp3";
            testFlag = !testFlag;
            var fd = global::Android.App.Application.Context.Assets.OpenFd(fileName);
            await _player.SetDataSourceAsync(fd.FileDescriptor, fd.StartOffset, fd.Length);*/

            _player.Prepare();
            _player.Start();
        }

        public void Resume()
        {
            _player.Start();
        }

        public void Stop()
        {
            _player.Stop();
        }

        public void Pause()
        {
            _player.Pause();
        }
    }
}