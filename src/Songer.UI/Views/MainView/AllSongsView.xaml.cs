using MvvmCross.Forms.Presenters.Attributes;
using MvvmCross.Forms.Views;
using Songer.Core.ViewModels;
using Xamarin.Forms;

namespace Songer.UI.Views.MainView
{
    [MvxMasterDetailPagePresentation(MasterDetailPosition.Detail, NoHistory = true)]
    public partial class AllSongsView : MvxContentPage<AllSongsViewModel>
    {
        public AllSongsView()
        {
            InitializeComponent();
        }

        private void ViewCell_BindingContextChanged(object sender, System.EventArgs e)
        {
            base.OnBindingContextChanged();

            if (BindingContext == null)
                return;

            ViewCell viewCell = ((ViewCell)sender);
            viewCell.ContextActions.Clear();

            if (ViewModel.IsAdmin)
            {
                viewCell.ContextActions.Add(new MenuItem()
                {
                    Text = "Delete",
                    Command = ViewModel.DeleteSongCommand,
                    CommandParameter = viewCell.BindingContext
                });;

                viewCell.ContextActions.Add(new MenuItem()
                {
                    Text = "Edit",
                    Command = ViewModel.EditSongCommand,
                    CommandParameter = viewCell.BindingContext
                });
            }
        }
    }
}