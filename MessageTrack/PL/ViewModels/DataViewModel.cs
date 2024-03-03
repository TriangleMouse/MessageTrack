using MessageTrack.BLL.DTOs;
using MessageTrack.PL.Models;
using MessageTrack.PL.Pages;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using AutoMapper;
using MessageTrack.BLL.Interfaces;
using System.Windows.Controls;

namespace MessageTrack.PL.ViewModels
{
    public class DataViewModel : INotifyPropertyChanged
    {
        private readonly IMapper _mapper;
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

        public DataViewModel(IMapper mapper, IBaseService baseService, IExternalRecipientService externalRecipientService, IOutboxMessageService outboxMessageService)
        {
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
            SelectRecipientsModal selectRecipientsModal = new SelectRecipientsModal();
            selectRecipientsModal.ShowDialog();
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
            var isUniqueExternalRecipient =
                await _externalRecipientService.CheckUniqueExternalRecipient(Message.NameExternalRecipient);

            if (isUniqueExternalRecipient)
            {
                var externalRecipientDto = new ExternalRecipientDto(Message.NameExternalRecipient);
                Message.ExternalRecipientId = await _externalRecipientService.CreateExternalRecipient(externalRecipientDto);
            }
            else
            {
                var externalRecipientDto = await _externalRecipientService
                    .GetExternalRecipientByName(Message.NameExternalRecipient);
                Message.ExternalRecipientId = externalRecipientDto.Id;
            }

            var outboxMessage = _mapper.Map<OutboxMessageModel, OutboxMessageDto>(Message);

            if (outboxMessage.Id.HasValue)
            {
                await _outboxMessageService.UpdateOutboxMessage(outboxMessage);
            }
            else
            {
                await _outboxMessageService.CreateOutboxMessage(outboxMessage);
            }

            _baseService.Commit();
            IsEditForm = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
