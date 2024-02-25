using AutoMapper;
using MessageTrack.BLL.DTOs;
using MessageTrack.DAL.Entities;
using MessageTrack.PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageTrack.BLL.Infrastructure.Profiles
{
    public class OutboxMessageProfile : Profile
    {
        public OutboxMessageProfile()
        {
            CreateMap<OutboxMessageDto, OutboxMessage>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                .ForMember(dest => dest.ExternalRecipientId, opt => opt.MapFrom(src => src.ExternalRecipientId))
                .ForMember(dest => dest.RegNumber, opt => opt.MapFrom(src => src.RegNumber))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ReverseMap();

            CreateMap<OutboxMessageModel, OutboxMessageDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                .ForMember(dest => dest.ExternalRecipientId, opt => opt.MapFrom(src => src.ExternalRecipientId))
                .ForMember(dest => dest.RegNumber, opt => opt.MapFrom(src => src.RegNumber))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes));

            CreateMap<OutboxMessageDto, OutboxMessageModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated.ToString("dd MMMM yyyy")))
                .ForMember(dest => dest.ExternalRecipientId, opt => opt.MapFrom(src => src.ExternalRecipientId))
                .ForMember(dest => dest.RegNumber, opt => opt.MapFrom(src => src.RegNumber))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes));
        }
    }
}
