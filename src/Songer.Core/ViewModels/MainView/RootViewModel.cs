using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.Threading.Tasks;

namespace Songer.Core.ViewModels
{
    sealed public class RootViewModel : BaseVM
    {
        public RootViewModel(IMvxNavigationService navigationService)
            : base(navigationService) { }

        public override void ViewAppearing()
        {
            base.ViewAppearing();

            MvxNotifyTask.Create(async () => await InitializeViewModels());
        }

        private async Task InitializeViewModels()
        {
            await _navigationService.Navigate<RootMenuViewModel>();
            await _navigationService.Navigate<HomeViewModel>();
        }
    }
}
