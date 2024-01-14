using AutoMapper;
using MessageTrack.BLL.DTOs;
using MessageTrack.DAL.Entities;

namespace MessageTrack.BLL.Infrastructure.Profiles
{
    public class ExternalRecipientProfile : Profile
    {
        public ExternalRecipientProfile()
        {
            CreateMap<ExternalRecipientDto, ExternalRecipient>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();
        }
    }
}
