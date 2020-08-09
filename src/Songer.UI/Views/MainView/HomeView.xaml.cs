using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Songer.Core.ViewModels;

namespace Songer.UI.Views
{
    [MvxMasterDetailPagePresentation(MasterDetailPosition.Detail, NoHistory = true)]
    public partial class HomeView : MvxContentPage<HomeViewModel>
    {
        public HomeView()
        {
            InitializeComponent();
        }
    }
}