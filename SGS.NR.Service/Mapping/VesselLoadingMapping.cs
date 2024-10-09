using AutoMapper;
using SGS.NR.Repository.DTO.Condition;
using SGS.NR.Service.DTO.Info;

namespace SGS.NR.Service.Mapping;

public class VesselLoadingMapping : Profile
{
    public VesselLoadingMapping()
    {
        // info > condition
        CreateMap<VesselLoadingInfo, VesselLoadingCondition>()
            //.ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.FilePath))
            .ReverseMap();
    }
}
