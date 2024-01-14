using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MessageTrack.BLL.DTOs;
using MessageTrack.BLL.Interfaces;
using MessageTrack.DAL.Entities;
using MessageTrack.DAL.Interfaces.UnitOfWork;

namespace MessageTrack.BLL.Services
{
    public class OutboxMessageService : IOutboxMessageService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OutboxMessageService(IMapper mapper, IUnitOfWork unitOfWork)
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
    }
}
