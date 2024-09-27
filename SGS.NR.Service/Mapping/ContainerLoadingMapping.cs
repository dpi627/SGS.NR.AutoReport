using AutoMapper;
using SGS.NR.Repository.DTO.Condition;
using SGS.NR.Repository.DTO.DataModel.ContainerLoading;
using SGS.NR.Service.DTO.Info;
using SGS.NR.Service.DTO.ResultModel.ContrainerLoading;

namespace SGS.NR.Service.Mapping;

internal class ContainerLoadingMapping : Profile
{
    public ContainerLoadingMapping()
    {
        // info > condition
        CreateMap<ContainerLoadingInfo, ContainerLoadingCondition>()
            //.ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.FilePath))
            .ReverseMap();

        CreateMap<MainDataModel, MainResultModel>().ReverseMap();
        CreateMap<SingleDataModel, SingleResultModel>().ReverseMap();
        CreateMap<CollectionDataModel, CollectionResultModel>().ReverseMap();
        CreateMap<GoodsItem, GoodsItemResultModel>().ReverseMap();
        CreateMap<TimeLogItem, TimeLogItemResultModel>().ReverseMap();
        CreateMap<InspectionItem, InspectionItemResultModel>().ReverseMap();
        CreateMap<QuantityItem, QuantityItemResultModel>().ReverseMap();
    }
}
