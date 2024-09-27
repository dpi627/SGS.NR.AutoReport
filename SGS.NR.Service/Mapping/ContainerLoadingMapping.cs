using AutoMapper;
using SGS.NR.Repository.DTO.Condition;
using SGS.NR.Service.DTO.Info;

namespace SGS.NR.Service.Mapping;

internal class ContainerLoadingMapping : Profile
{
    public ContainerLoadingMapping()
    {
        // info > condition
        CreateMap<ContainerLoadingInfo, ContainerLoadingCondition>()
            //.ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.FilePath))
            .ReverseMap();
    }
}
