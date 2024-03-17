using MessageTrack.BLL.DTOs;
using MessageTrack.BLL.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace MessageTrack.PL.ViewModels
{
    public class SelectRecipientsViewModel : INotifyPropertyChanged
    {
        private readonly IBaseService _baseService;
        private readonly IExternalRecipientService _externalRecipientService;
        private readonly IOutboxMessageService _outboxMessageService;

        private string _searchText;
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

        private ICollectionView _view;

        public ICollectionView View
        {
            get => _view;
            set
            {
                _view = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                View.Refresh();
            }
        }


        public ExternalRecipientDto SelectedExternalRecipient { get; set; }

        public ICommand CancelCommand { get; set; }
        public ICommand ChooseCommand { get; set; }
        public ICommand LoadDataCommand { get; set; }
        public ICommand DeleteCommand { get; private set; }

        public SelectRecipientsViewModel(IBaseService baseService, IExternalRecipientService externalRecipientService, IOutboxMessageService outboxMessageService)
        {
            _baseService = baseService;
            _externalRecipientService = externalRecipientService;
            _outboxMessageService = outboxMessageService;

            CancelCommand = new RelayCommand(async () => await CancelModal());
            ChooseCommand = new RelayCommand(async () => await ChooseExternalRecipient());
            LoadDataCommand = new RelayCommand(async () => await LoadData());
            DeleteCommand = new RelayCommand<ExternalRecipientDto>(async (externalRecipient) => await Delete(externalRecipient));
        }

        private bool FilterRecipients(object obj) => 
            obj is ExternalRecipientDto recipient && 
                (
                string.IsNullOrWhiteSpace(_searchText) || 
                recipient.Name.Contains(_searchText, StringComparison.OrdinalIgnoreCase)
                );
    

        private async Task LoadData()
        {
            IEnumerable<ExternalRecipientDto> externalRecipients = await _externalRecipientService.GetExternalRecipients();
            ExternalRecipients = new ObservableCollection<ExternalRecipientDto>(externalRecipients);
            View = CollectionViewSource.GetDefaultView(ExternalRecipients);
            View.Filter = FilterRecipients;
        }

        private async Task CancelModal()
        {
            DialogResult = false;
        }

        private async Task ChooseExternalRecipient()
        {
            DialogResult = true;
        }

        private async Task Delete(ExternalRecipientDto externalRecipient)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить выбранную запись?", "Внимание!", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            var messages = await _outboxMessageService.GetAllMessagesByExternalRecipientId(externalRecipient.Id);
            var regNumbers = messages.Select(message => message.RegNumber);

            if (messages.Any())
            {
                var errorText = string.Format("Данного получателя невозможно удалить, так как он выбран в следующих карточках: {0}", string.Join(',', regNumbers));

                MessageBox.Show(errorText);
                return;
            }

            await _externalRecipientService.DeleteExternalRecipientById(externalRecipient.Id);
            _baseService.Commit();
            ExternalRecipients.Remove(externalRecipient);
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
