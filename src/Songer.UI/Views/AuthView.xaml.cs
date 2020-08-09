using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Songer.Core.ViewModels;

namespace Songer.UI.Views
{
    [MvxContentPagePresentation(NoHistory = true)]
    public partial class AuthView : MvxContentPage<AuthViewModel>
    {
        public AuthView()
        {
            InitializeComponent();
        }
    }
}