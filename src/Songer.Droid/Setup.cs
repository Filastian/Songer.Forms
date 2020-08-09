using MvvmCross;
using MvvmCross.Forms.Platforms.Android.Core;
using Songer.Core.Player;
using Songer.Core.Services;
using Songer.Droid.Audio;
using Songer.UI.Services;

namespace Songer.Droid
{
    public class Setup : MvxFormsAndroidSetup<Core.App, UI.App>
    {
        public override void InitializePrimary()
        {
            base.InitializePrimary();

            Mvx.IoCProvider.RegisterSingleton<IPopupService>(new PopupService());
            Mvx.IoCProvider.RegisterSingleton<IPlayer>(new Player());
        }
    }
}