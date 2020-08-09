using MvvmCross.Commands;
using MvvmCross.Navigation;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Songer.Core.Services;
using Songer.Core.Validation;

namespace Songer.Core.ViewModels
{
    sealed public class AddSongViewModel : BaseVM
    {
        public AddSongViewModel(
            IMvxNavigationService navigationService,
            ISongService songService,
            IPopupService popupService,
            CreateSongValidator validator) 
            : base(navigationService)
        {
            _songService = songService;
            _popupService = popupService;

            Validator = validator;
        }

        #region Fields and properties

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;

                Validator.ValidateTitle(_title);
            }
        }

        private string _performer;
        public string Performer
        {
            get => _performer;
            set
            {
                _performer = value;

                Validator.ValidatePerformer(_performer);
            }
        }

        public string Path { get; private set; }

        private FileData _selectedSong;
        public FileData SelectedSong
        {
            get => _selectedSong;
            set
            {
                _selectedSong = value;

                Title = _selectedSong.FileName;
                Path = _selectedSong.FilePath;

                Validator.ValidatePath(Path);
            }
        }

        private readonly ISongService _songService;
        private readonly IPopupService _popupService;
        public CreateSongValidator Validator { get; }

        #endregion

        #region Commands

        public IMvxCommand ChooseFileCommand => new MvxCommand(ChooseFile);
        public IMvxCommand SaveCommand => new MvxCommand(Save);
        public IMvxCommand CancelCommand => new MvxCommand(Cancel);

        private async void ChooseFile()
        {
            var file = await CrossFilePicker.Current.PickFile();

            if (file != null) 
                SelectedSong = file;
        }

        private async void Save()
        {
            if (Validator.IsValid)
            {
                await _popupService.PushSpinnerAsync();

                await _songService.AddSong(Title, Performer, _selectedSong.DataArray);

                await _navigationService.Close(this);

                await _popupService.PopAsync();
            }
        }

        private async void Cancel()
        {
            await _navigationService.Close(this);
        }

        #endregion
    }
}
