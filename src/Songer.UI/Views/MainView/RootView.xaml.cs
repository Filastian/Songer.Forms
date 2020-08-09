using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Songer.Core.ViewModels;

namespace Songer.UI.Views
{
    [MvxMasterDetailPagePresentation(MasterDetailPosition.Root, WrapInNavigationPage = false)]
    public partial class RootView : MvxMasterDetailPage<RootViewModel>
    {
        public RootView()
        {
            InitializeComponent();
        }
    }
}