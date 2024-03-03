using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IExternalRecipientService _externalRecipientService;
        private ObservableCollection<ExternalRecipientDto> _externalRecipients;

        public ICommand LoadDataCommand { get; set; }

        public SelectRecipientsViewModel(IMapper mapper, IExternalRecipientService externalRecipientService)
        {
            _mapper = mapper;
            _externalRecipientService = externalRecipientService;

            LoadDataCommand = new RelayCommand(async () => await LoadData());
        }


        public async Task LoadData()
        {
            IEnumerable<ExternalRecipientDto> externalRecipients = await _externalRecipientService.GetExternalRecipients();
            _externalRecipients = new ObservableCollection<ExternalRecipientDto>(externalRecipients);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
