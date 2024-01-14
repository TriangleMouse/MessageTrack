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
    public class ExternalRecipientService : IExternalRecipientService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ExternalRecipientService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ExternalRecipientDto>> GetExternalRecipients()
        {
            var externalRecipients = await _unitOfWork.ExternalRecipientRepository.GetExternalRecipients();
            var externalRecipientDtos =
                _mapper.Map<IEnumerable<ExternalRecipient>, IEnumerable<ExternalRecipientDto>>(externalRecipients);

            return externalRecipientDtos;
        }
    }
}
