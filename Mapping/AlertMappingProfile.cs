using AutoMapper;
using DPAS.Api.Dtos;
using DPAS.Api.Models.Data;

namespace DPAS.Api.Mapping
{
    public class AlertMappingProfile : Profile
    {
        public AlertMappingProfile()
        {
            CreateMap<AlertModel, GetAlertDto>()
                .ForMember(dest => dest.RegionID, opt => opt.MapFrom(src => src.Region.RegionID));
            CreateMap<CreateAlertDto, AlertModel>()
                .ForMember(dest => dest.RegionId, opt => opt.Ignore())
                .ForMember(dest => dest.Region, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            CreateMap<UpdateAlertDto, AlertModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RegionId, opt => opt.Ignore())
                .ForMember(dest => dest.Region, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}