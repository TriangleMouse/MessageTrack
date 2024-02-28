﻿using MessageTrack.BLL.Interfaces;
using MessageTrack.BLL.Services;
using MessageTrack.PL.Models;
using MessageTrack.PL.Pages;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using AutoMapper;
using MessageTrack.BLL.DTOs;

namespace MessageTrack.PL.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly IServiceProvider _provider;
        private readonly IMapper _mapper;
        private readonly IOutboxMessageService _outboxMessageService;
        private readonly IExternalRecipientService _externalRecipientService;
        private string _searchText;
        private int _selectedYear;
        private int _selectedMonth;
        private ObservableCollection<OutboxMessageModel> _messages;

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                FilterMessages();
            }
        }

        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                _selectedYear = value;
                OnPropertyChanged();
                FilterMessages();
            }
        }

        public int SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                _selectedMonth = value;
                OnPropertyChanged();
                FilterMessages();
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

        public MainPageViewModel(IServiceProvider provider, IMapper mapper, IOutboxMessageService outboxMessageService, IExternalRecipientService externalRecipientService)
        {
            _mapper = mapper;
            _provider = provider;
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
        }

        private async Task FilterMessages()
        {
            // Выполните фильтрацию сообщений асинхронно
            //Messages = new ObservableCollection<OutboxMessageDto>(await service.FilterMessagesAsync(SearchText, SelectedYear, SelectedMonth));
        }

        private async Task Add()
        {
            // Откройте новую страницу для добавления записи асинхронно
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
            // Изменение сообщения асинхронно
        }

        private async Task Delete(OutboxMessageModel message)
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
