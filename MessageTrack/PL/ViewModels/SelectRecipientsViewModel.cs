using MessageTrack.BLL.DTOs;
using MessageTrack.BLL.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MessageTrack.PL.ViewModels
{
    public class SelectRecipientsViewModel : INotifyPropertyChanged
    {
        private readonly IExternalRecipientService _externalRecipientService;
        private ObservableCollection<ExternalRecipientDto> _externalRecipients;

        public ObservableCollection<ExternalRecipientDto> ExternalRecipients
        {
            get => _externalRecipients;
            set
            {
                _externalRecipients = value;
                OnPropertyChanged();
            }
        }

        public ExternalRecipientDto SelectedExternalRecipient { get; set; }

        public ICommand CancelCommand { get; set; }
        public ICommand ChooseCommand { get; set; }
        public ICommand LoadDataCommand { get; set; }
       
        public SelectRecipientsViewModel(IExternalRecipientService externalRecipientService)
        {
            _externalRecipientService = externalRecipientService;

            CancelCommand = new RelayCommand(() => CancelModal());
            ChooseCommand = new RelayCommand(() => ChooseExternalRecipient());
            LoadDataCommand = new RelayCommand(async () => await LoadData());
        }

        public async Task LoadData()
        {
            IEnumerable<ExternalRecipientDto> externalRecipients = await _externalRecipientService.GetExternalRecipients();
            ExternalRecipients = new ObservableCollection<ExternalRecipientDto>(externalRecipients);
        }

        public void CancelModal()
        {
            DialogResult = false;
        }

        public void ChooseExternalRecipient()
        {
            DialogResult = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        private bool? dialogResult;

        public bool? DialogResult { get => dialogResult; set => SetProperty(ref dialogResult, value); }
    }
}
