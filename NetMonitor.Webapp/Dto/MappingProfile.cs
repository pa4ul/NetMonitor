using AutoMapper;
using NetMonitor.Dto;
using NetMonitor.Model;
using Index = NetMonitor.Webapp.Pages.allServices.Index;

namespace NetMonitor.Webapp.Dto;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<Index.EditServiceCmd, Service>();
        CreateMap<Service, Index.EditServiceCmd>();
        CreateMap<ServiceDto, Service>()
            .ForMember(o => o.Guid, opt => opt.MapFrom(o => o.Guid == default ? Guid.NewGuid() : o.Guid));
        CreateMap<Service, ServiceDto>()
            .ForCtorParam("Description", opt => opt.MapFrom(src => src.Description.description))
            .ForCtorParam("LongDescription", opt => opt.MapFrom(src => src.Description.longdescription));

        CreateMap<Message, MessageDto>()
            .ForCtorParam(nameof(MessageDto.Description), opt => opt.MapFrom(src => src.Description.description))
            .ForCtorParam(nameof(MessageDto.LongDescription),
                opt => opt.MapFrom(src => src.Description.longdescription))
            .ForCtorParam(nameof(MessageDto.Type), opt => opt.MapFrom(src => src.MessageType));
    }
}