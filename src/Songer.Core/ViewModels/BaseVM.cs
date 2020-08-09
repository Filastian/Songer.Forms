using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace Songer.Core.ViewModels
{
    public class BaseVM : MvxViewModel
    {
        protected IMvxNavigationService _navigationService;

        public BaseVM(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }

    public class BaseVM<T> : MvxViewModel<T>
    {
        protected IMvxNavigationService _navigationService;

        public BaseVM(IMvxNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public override void Prepare(T parameter)
        {
            Entity = parameter;
        }

        protected T Entity;
    }
}
