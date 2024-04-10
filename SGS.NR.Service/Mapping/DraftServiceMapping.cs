using AutoMapper;
using SGS.NR.Repository.DTO.Condition;
using SGS.NR.Service.DTO.Info;

namespace SGS.NR.Service.Mapping
{
    public class DraftServiceMapping : Profile
    {
        public DraftServiceMapping()
        {
            // info > condition
            CreateMap<DraftInfo, TestRecordCondition>()
                .ForMember(dest => dest.ImportPath, opt => opt.MapFrom(src => src.ImportPath));
        }
    }
}
