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

        public async Task CreateOutboxMessage(OutboxMessageDto outboxMessageDto)
        {
            outboxMessageDto.RegNumber = await GenerateRegNumber();
            var message = _mapper.Map<OutboxMessage>(outboxMessageDto);
            await _unitOfWork.OutboxMessageRepository.CreateOutboxMessage(message);
        }

        private async Task<OutboxMessageDto> GetLastOutboxMessage()
        {
            var lastMessage = await _unitOfWork.OutboxMessageRepository.GetLastOutboxMessage();
            var lastMessageDto = _mapper.Map<OutboxMessageDto>(lastMessage);

            return lastMessageDto;
        }

        private async Task<string> GenerateRegNumber()
        {
            var regexPattern = "\\s*(?<Day>\\d*)-(?<Month>\\d*)/(?<UniqueNumberOnMonth>\\d*)";
            int uniqueNumberOnMonth = 1;

            var lastMessage = await GetLastOutboxMessage();

            if (lastMessage != default)
            {
                var match = new Regex(regexPattern).Match(lastMessage.RegNumber);
                var lastMessageUniqueNumber = int.Parse(match.Groups["UniqueNumberOnMonth"].Value);

                if (DateTime.Now.Month > lastMessage.DateCreated.Month)
                    uniqueNumberOnMonth = ++lastMessageUniqueNumber;
            }
            
            string regNumber = $"{DateTime.Now.Day}-{DateTime.Now.Month}/{uniqueNumberOnMonth}";

            return regNumber;
        }
    }
}
