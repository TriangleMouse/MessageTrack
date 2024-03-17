using AutoMapper;
using MessageTrack.BLL.DTOs;
using MessageTrack.BLL.Interfaces;
using MessageTrack.PL.Models;
using MessageTrack.PL.Pages;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace MessageTrack.PL.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly IServiceProvider _provider;
        private readonly IMapper _mapper;
        private readonly IBaseService _baseService;
        private readonly IOutboxMessageService _outboxMessageService;
        private readonly IExternalRecipientService _externalRecipientService;
        private string _searchText;
        private string _selectedYear;
        private string _selectedMonth;
        private ObservableCollection<OutboxMessageModel> _messages;

        private ICollectionView _messagesView;
        private IEnumerable<string> _years;
        private IEnumerable<string> _months;

        public ICollectionView MessagesView
        {
            get => _messagesView;
            set
            {
                _messagesView = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<string> Years
        {
            get => _years;
            set
            {
                _years = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<string> Months
        {
            get => _months;
            set
            {
                _months = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                MessagesView.Refresh();
            }
        }

        public string SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                MessagesView.Refresh();
            }
        }

        public string SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                _selectedMonth = value;
                MessagesView.Refresh();
            }
        }

        public ObservableCollection<OutboxMessageModel> Messages
        {
            get => _messages;
            set
            {
                _messages = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadDataCommand { get; set; }
        public ICommand AddCommand { get; private set; }
        public ICommand ViewCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public MainPageViewModel(IServiceProvider provider, IMapper mapper, IBaseService baseService, IOutboxMessageService outboxMessageService, IExternalRecipientService externalRecipientService)
        {
            _mapper = mapper;
            _provider = provider;
            _baseService = baseService;
            _outboxMessageService = outboxMessageService;
            _externalRecipientService = externalRecipientService;

            LoadDataCommand = new RelayCommand(async () => await LoadData());
            AddCommand = new RelayCommand(async () => await Add());
            ViewCommand = new RelayCommand<OutboxMessageModel>(async (message) => await View(message));
            EditCommand = new RelayCommand<OutboxMessageModel>(async (message) => await Edit(message));
            DeleteCommand = new RelayCommand<OutboxMessageModel>(async (message) => await Delete(message));
        }

        public async Task LoadData()
        {
            IEnumerable<ExternalRecipientDto> externalRecipients = await _externalRecipientService.GetExternalRecipients();
            IEnumerable<OutboxMessageDto> messagesDto = await _outboxMessageService.GetOutboxMessages();
            IEnumerable<OutboxMessageModel> messages = _mapper.Map<IEnumerable<OutboxMessageDto>, IEnumerable<OutboxMessageModel>>(messagesDto);

            foreach (var outboxMessageModel in messages)
            {
                outboxMessageModel.NameExternalRecipient = externalRecipients.FirstOrDefault(externalRecipient =>
                    externalRecipient.Id == outboxMessageModel.ExternalRecipientId).Name;
            }

            Messages = new ObservableCollection<OutboxMessageModel>(messages);
            MessagesView = CollectionViewSource.GetDefaultView(Messages);
            MessagesView.Filter = FilterMessages;
            Years = FillYearsFilter(messagesDto);
            Months = FillMonthsFilter();
        }

        private List<string> FillMonthsFilter() => new(13) { "Все" , "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль","Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };

        private List<string> FillYearsFilter(IEnumerable<OutboxMessageDto> messagesDto)
        {
            var years = new List<string>() { "Все" };
            years.AddRange(messagesDto.Select(message => message.DateCreated.Year.ToString()).Distinct().OrderBy(x => x));

            return years;
        }

        private bool FilterMessages(object obj)
        {
            if (obj is not OutboxMessageModel message)
                return false;

            bool isSearchTextEmpty = string.IsNullOrWhiteSpace(SearchText);
            bool containsSearchText = !isSearchTextEmpty && 
                                      (ContainsSearchText(message.NameExternalRecipient) ||
                                      ContainsSearchText(message.RegNumber) ||
                                      ContainsSearchText(message.DateCreated) ||
                                      ContainsSearchText(message.Notes));

            bool containsSelectedYear = string.IsNullOrWhiteSpace(SelectedYear) || string.Equals(SelectedYear, "Все") || 
                                        message.DateCreated.Contains(SelectedYear, StringComparison.OrdinalIgnoreCase);
            bool containsSelectedMonth = string.IsNullOrWhiteSpace(SelectedMonth) || string.Equals(SelectedMonth, "Все") || 
                                          message.DateCreated.Contains(SelectedMonth, StringComparison.OrdinalIgnoreCase);

            return (isSearchTextEmpty || containsSearchText) && containsSelectedYear && containsSelectedMonth;
        }
        private bool ContainsSearchText(string field)
        {
            return field.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
        }

        private async Task Add()
        {
            var frame = Application.Current.MainWindow.FindName("MainFrame") as Frame;
            if (frame != null)
            {
                var dataPage = _provider.GetService<DataPage>();

                frame.Navigate(dataPage);
            }
        }

        private async Task View(OutboxMessageModel message)
        {
            var dataViewModel = _provider.GetRequiredService<DataViewModel>();
            dataViewModel.Message = message;
            dataViewModel.IsEditForm = false;
            var dataPage = _provider.GetRequiredService<DataPage>();
            
            var frame = Application.Current.MainWindow.FindName("MainFrame") as Frame;
            if (frame != null)
            {
                dataPage.DataContext = dataViewModel;

                frame.Navigate(dataPage);
            }
        }

        private async Task Edit(OutboxMessageModel message)
        {
            var dataViewModel = _provider.GetRequiredService<DataViewModel>();
            dataViewModel.Message = message;
            dataViewModel.IsEditForm = true;
            var dataPage = _provider.GetRequiredService<DataPage>();

            var frame = Application.Current.MainWindow.FindName("MainFrame") as Frame;
            if (frame != null)
            {
                dataPage.DataContext = dataViewModel;

                frame.Navigate(dataPage);
            }
        }

        private async Task Delete(OutboxMessageModel message)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить выбранную запись?", "Внимание!", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            await _outboxMessageService.DeleteOutboxMessageById(message.Id.Value);
            _baseService.Commit();
            Messages.Remove(message);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
