using AutoMapper;
using DPAS.Api.Dtos;
using DPAS.Api.Models.Data;
using DPAS.Api.Extensions;

namespace DPAS.Api.Mapping
{
    public class RegionMappingProfile : Profile
    {
        public RegionMappingProfile()
        {
            CreateMap<RegionModel, GetRegionDto>()
                .ForMember(dest => dest.DisasterTypes, opt => opt.MapFrom(src => src.DisasterTypes.Select(dt => dt.GetDisplayName()).ToList()));
            CreateMap<CreateRegionDto, RegionModel>();
            CreateMap<UpdateRegionDto, RegionModel>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RegionID, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}