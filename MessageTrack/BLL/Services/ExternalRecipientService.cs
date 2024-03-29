﻿using AutoMapper;
using MessageTrack.BLL.DTOs;
using MessageTrack.BLL.Interfaces;
using MessageTrack.DAL.Entities;
using MessageTrack.DAL.Interfaces.UnitOfWork;

namespace MessageTrack.BLL.Services
{
    public class ExternalRecipientService : BaseService, IExternalRecipientService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ExternalRecipientService(IMapper mapper, IUnitOfWork unitOfWork) : base(unitOfWork)
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

        public async Task DeleteExternalRecipientById(int id)
        {
            await _unitOfWork.ExternalRecipientRepository.DeleteExternalRecipientById(id);
        }

        public async Task<bool> CheckUniqueExternalRecipient(string name)
        {
            var externalRecipient = await _unitOfWork.ExternalRecipientRepository.GetExternalRecipientByName(name);
            var isUnique = externalRecipient == default;

            return isUnique;
        }

        public async Task<int> GetExternalRecipientIdByName(string externalRecipientName)
        {
            var isUniqueExternalRecipient = await CheckUniqueExternalRecipient(externalRecipientName);

            if (isUniqueExternalRecipient)
            {
                var newExternalRecipient = new ExternalRecipientDto(externalRecipientName);
                var id = await CreateExternalRecipient(newExternalRecipient);

                return id;
            }

            var externalRecipientDto = await GetExternalRecipientByName(externalRecipientName);

            return externalRecipientDto.Id;
        }

        public async Task<ExternalRecipientDto> GetExternalRecipientByName(string name)
        {
            var externalRecipient = await _unitOfWork.ExternalRecipientRepository.GetExternalRecipientByName(name);
            var externalRecipientDto =
                _mapper.Map<ExternalRecipient, ExternalRecipientDto>(externalRecipient);

            return externalRecipientDto;
        }

        public async Task<int> CreateExternalRecipient(ExternalRecipientDto externalRecipientDto)
        {
            var externalRecipient = _mapper.Map<ExternalRecipient>(externalRecipientDto);
            var externalRecipientId = await _unitOfWork.ExternalRecipientRepository.CreateExternalRecipient(externalRecipient);

            if (!externalRecipientId.HasValue)
            {
                //todo в ресурсы вынести
                throw new ArgumentNullException(nameof(externalRecipientId),"При создании получателя, возникла ошибка и не удалось получить его id.");
            }

            return externalRecipientId.Value;
        }

        public async Task<ExternalRecipientDto> GetExternalRecipientById(int id)
        {
            var externalRecipient = await _unitOfWork.ExternalRecipientRepository.GetExternalRecipientById(id);
            var externalRecipientDto =
                _mapper.Map<ExternalRecipient, ExternalRecipientDto>(externalRecipient);

            return externalRecipientDto;
        }
    }
}
