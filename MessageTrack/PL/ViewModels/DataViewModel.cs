using AutoMapper;
using MessageTrack.BLL.DTOs;
using MessageTrack.BLL.Interfaces;
using MessageTrack.PL.Models;
using MessageTrack.PL.Pages;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MessageTrack.PL.ViewModels
{
    public class DataViewModel : INotifyPropertyChanged
    {
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;
        private readonly IBaseService _baseService;
        private readonly IExternalRecipientService _externalRecipientService;
        private readonly IOutboxMessageService _outboxMessageService;

        private bool _isEditForm = true;
        private Visibility _editFormVisibility;
        private OutboxMessageModel _message;


        public OutboxMessageModel Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public bool IsEditForm
        {
            get => _isEditForm;
            set
            {
                _isEditForm = value;
                EditFormVisibility = value ? Visibility.Visible : Visibility.Collapsed;
                OnPropertyChanged();
            }
        }
       
        public Visibility EditFormVisibility
        {
            get { return _editFormVisibility; }
            set
            {
                _editFormVisibility = value;
                OnPropertyChanged();
            }
        }

        public ICommand SelectExternalRecipientCommand { get; private set; }
        public ICommand BackCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }

        public DataViewModel(IServiceProvider provider, IMapper mapper, IBaseService baseService, IExternalRecipientService externalRecipientService, IOutboxMessageService outboxMessageService)
        {
            _serviceProvider = provider;
            _mapper = mapper;
            _baseService = baseService;
            _externalRecipientService = externalRecipientService;
            _outboxMessageService = outboxMessageService;

            Message = new OutboxMessageModel();
            BackCommand = new RelayCommand(async () => await Back());
            CancelCommand = new RelayCommand(async () => await Cancel());
            SelectExternalRecipientCommand = new RelayCommand(async () => await SelectExternalRecipient());
            SaveCommand = new RelayCommand(async () => await Save());
        }

        private async Task Back()
        {
            var frame = Application.Current.MainWindow.FindName("MainFrame") as Frame;

            if (frame != null && frame.CanGoBack)
            {
                frame.GoBack();
            }
        }

        private async Task SelectExternalRecipient()
        {
            var selectRecipientsModal = _serviceProvider.GetRequiredService<SelectRecipientsModal>();
            var result = selectRecipientsModal.ShowDialog();

            if (result.Value && selectRecipientsModal.SelectRecipientsViewModel.SelectedExternalRecipient is ExternalRecipientDto)
            {
                Message.NameExternalRecipient =
                    selectRecipientsModal.SelectRecipientsViewModel.SelectedExternalRecipient.Name;
                Message.ExternalRecipientId = selectRecipientsModal.SelectRecipientsViewModel.SelectedExternalRecipient.Id;
                OnPropertyChanged(nameof(Message));
            }
        }

        private async Task Cancel()
        {
            if (!Message.Id.HasValue)
            {
                await Back();
                return;
            }

            IsEditForm = false;
        }

        private async Task Save()
        {
            string externalRecipientName = Message.NameExternalRecipient.Trim();

            if (string.IsNullOrEmpty(externalRecipientName))
            {   
                //todo в ресурсы вынести
                MessageBox.Show("Поле \"Получатель\" не заполнено.","Ошибка!", MessageBoxButton.OK);
                return;
            }

            var outboxMessage = _mapper.Map<OutboxMessageModel, OutboxMessageDto>(Message);
            outboxMessage.ExternalRecipientId =
                await _externalRecipientService.GetExternalRecipientIdByName(externalRecipientName);

            if (!outboxMessage.Id.HasValue)
                outboxMessage.RegNumber = await _outboxMessageService.GenerateRegNumber();

            await _outboxMessageService.SaveOutboxMessage(outboxMessage);
            _baseService.Commit();

            var savedOutboxMessage = _mapper.Map<OutboxMessageDto, OutboxMessageModel>(outboxMessage);
            savedOutboxMessage.NameExternalRecipient = externalRecipientName;

            Message = savedOutboxMessage;
            IsEditForm = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
