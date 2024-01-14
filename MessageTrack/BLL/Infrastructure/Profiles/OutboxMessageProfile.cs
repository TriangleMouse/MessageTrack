using AutoMapper;
using MessageTrack.BLL.DTOs;
using MessageTrack.DAL.Entities;
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
                .ForMember(dest => dest.Reg_Number, opt => opt.MapFrom(src => src.Reg_Number))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Notes))
                .ReverseMap();
        }
    }
}
