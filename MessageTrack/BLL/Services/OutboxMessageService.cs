using AutoMapper;
using MessageTrack.BLL.DTOs;
using MessageTrack.BLL.Interfaces;
using MessageTrack.DAL.Entities;
using MessageTrack.DAL.Interfaces.UnitOfWork;
using System.Text.RegularExpressions;

namespace MessageTrack.BLL.Services
{
    public class OutboxMessageService : BaseService, IOutboxMessageService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OutboxMessageService(IMapper mapper, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<OutboxMessageDto>> GetOutboxMessages()
        {
            var outboxMessages = await _unitOfWork.OutboxMessageRepository.GetOutboxMessages();
            var outboxMessagesDtos =
                _mapper.Map<IEnumerable<OutboxMessage>, IEnumerable<OutboxMessageDto>>(outboxMessages);

            return outboxMessagesDtos;
        }

        public async Task<IEnumerable<OutboxMessageDto>> GetAllMessagesByExternalRecipientId(int externalRecipientId)
        {
            var outboxMessages = await _unitOfWork.OutboxMessageRepository.GetAllMessagesByExternalRecipientId(externalRecipientId);
            var outboxMessagesDtos =
                _mapper.Map<IEnumerable<OutboxMessage>, IEnumerable<OutboxMessageDto>>(outboxMessages);

            return outboxMessagesDtos;
        }

    public async Task DeleteOutboxMessageById(int id)
        {
            await _unitOfWork.OutboxMessageRepository.DeleteOutboxMessageById(id);
        }

        public async Task SaveOutboxMessage(OutboxMessageDto outboxMessageDto)
        {
            var message = _mapper.Map<OutboxMessage>(outboxMessageDto);

            if (message.Id == default)
            {
                outboxMessageDto.Id = await CreateOutboxMessage(message);
            }
            else
            {
                await UpdateOutboxMessage(message);
            }
        }

        public async Task<string> GenerateRegNumber()
        {
            var regexPattern = "\\s*(?<UniqueNumberOnMonth>\\d*)-(?<Day>\\d*)/(?<Month>\\d*)";
            int uniqueNumberOnMonth = 1;

            var lastMessage = await GetLastOutboxMessage();

            if (lastMessage != default)
            {
                var match = new Regex(regexPattern).Match(lastMessage.RegNumber);
                var lastMessageUniqueNumber = int.Parse(match.Groups["UniqueNumberOnMonth"].Value);

                if (DateTime.Now.Month <= lastMessage.DateCreated.Month)
                    uniqueNumberOnMonth = ++lastMessageUniqueNumber;
            }
            string regNumber = $"{uniqueNumberOnMonth}-{DateTime.Now.Day.ToString("00")}/{DateTime.Now.Month.ToString("00")}";

            return regNumber;
        }

        private async Task UpdateOutboxMessage(OutboxMessage message)
        {
            await _unitOfWork.OutboxMessageRepository.UpdateOutboxMessage(message);
        }

        private async Task<int> CreateOutboxMessage(OutboxMessage message)
        {
            var id = await _unitOfWork.OutboxMessageRepository.CreateOutboxMessage(message);

            if (!id.HasValue)
            {
                //todo в ресурсы вынести
                throw new ArgumentNullException(nameof(id), "При сохранении сообщения возникла ошибка. Не удалось получить id пакета");
            }

            return id.Value;
        }

        private async Task<OutboxMessageDto> GetLastOutboxMessage()
        {
            var lastMessage = await _unitOfWork.OutboxMessageRepository.GetLastOutboxMessage();
            var lastMessageDto = _mapper.Map<OutboxMessageDto>(lastMessage);

            return lastMessageDto;
        }
    }
}
