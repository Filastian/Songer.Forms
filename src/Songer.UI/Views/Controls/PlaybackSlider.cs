using System;
using Xamarin.Forms;

namespace Songer.UI.Views.Controls
{
    public class PlaybackSlider : Slider
    {
        public PlaybackSlider()
        {
            DragCompleted += PlaybackSlider_DragCompleted;
        }

        public static readonly BindableProperty ChangeablePlaybackTimeProperty = BindableProperty.Create(nameof(ChangeablePlaybackTime),
          typeof(double), typeof(PlaybackSlider), -1.0);

        public double ChangeablePlaybackTime
        {
            get { return (double)GetValue(ChangeablePlaybackTimeProperty); }
            set { SetValue(ChangeablePlaybackTimeProperty, value); }
        }

        private void PlaybackSlider_DragCompleted(object sender, EventArgs e)
        {
            var slider = (PlaybackSlider)sender;

            ChangeablePlaybackTime = slider.Value;
        }
    }
}