using Android.App;
using Android.Content.PM;
using Android.OS;
using MvvmCross.Forms.Platforms.Android.Core;
using MvvmCross.Forms.Platforms.Android.Views;

namespace Songer.Droid
{
    [Activity(Label = "Songer", 
        Icon = "@mipmap/icon",
        Theme = "@style/MainTheme", 
        MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : MvxFormsAppCompatActivity<Setup, Core.App, UI.App>
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Rg.Plugins.Popup.Popup.Init(this, bundle);
        }
    }
}