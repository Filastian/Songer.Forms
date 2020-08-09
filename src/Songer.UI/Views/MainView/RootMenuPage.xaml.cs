using MvvmCross;
using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Songer.Core.ViewModels;
using Xamarin.Forms;

namespace Songer.UI.Views
{
    [MvxMasterDetailPagePresentation(MasterDetailPosition.Master)]
    public partial class RootMenuPage : MvxContentPage<RootMenuViewModel>
    {
        public RootMenuPage()
        {
            InitializeComponent();

            rootMenu.ItemSelected += (sender, e) =>
            {
                (Application.Current.MainPage as RootView).IsPresented = false;
            };

            userControlLayout.Tapped += (sender, e) =>
            {
                (Application.Current.MainPage as RootView).IsPresented = false;
            };
        }
    }
}