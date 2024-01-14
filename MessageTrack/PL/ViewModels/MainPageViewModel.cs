using MessageTrack.BLL.DTOs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MessageTrack.BLL.Interfaces;

namespace MessageTrack.PL.ViewModels
{
    public class MainPageViewModel
    {
        private string searchText;
        private int selectedYear;
        private int selectedMonth;
        private ObservableCollection<OutboxMessageDto> messages;

        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                OnPropertyChanged();
                FilterMessages();
            }
        }

        public int SelectedYear
        {
            get => selectedYear;
            set
            {
                selectedYear = value;
                OnPropertyChanged();
                FilterMessages();
            }
        }

        public int SelectedMonth
        {
            get => selectedMonth;
            set
            {
                selectedMonth = value;
                OnPropertyChanged();
                FilterMessages();
            }
        }

        public ObservableCollection<OutboxMessageDto> Messages
        {
            get => messages;
            set
            {
                messages = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddCommand { get; private set; }
        public ICommand ViewCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public MainPageViewModel(IOutboxMessageService outboxMessageService, IExternalRecipientService externalRecipientService)
        {
            AddCommand = new RelayCommand(async () => await Add());
            ViewCommand = new RelayCommand(async () => await View());
            EditCommand = new RelayCommand(async () => await Edit());
            DeleteCommand = new RelayCommand(async () => await Delete());

            // Получите сообщения из вашего сервиса асинхронно
            Task.Run(async () => Messages = new ObservableCollection<OutboxMessageDto>(await outboxMessageService.GetOutboxMessages()));
        }

        private async Task FilterMessages()
        {
            // Выполните фильтрацию сообщений асинхронно
            //Messages = new ObservableCollection<OutboxMessageDto>(await service.FilterMessagesAsync(SearchText, SelectedYear, SelectedMonth));
        }

        private async Task Add()
        {
            // Откройте новую страницу для добавления записи асинхронно
        }

        private async Task View()
        {
            // Просмотр сообщения асинхронно
        }

        private async Task Edit()
        {
            // Изменение сообщения асинхронно
        }

        private async Task Delete()
        {
            // Удаление сообщения асинхронно
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
