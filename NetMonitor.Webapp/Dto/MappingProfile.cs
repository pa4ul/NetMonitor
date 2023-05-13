using AutoMapper;
using NetMonitor.Dto;
using NetMonitor.Model;

namespace NetMonitor.Webapp.Dto;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ServiceDto, Service>();
        CreateMap<Service, ServiceDto>()
            .ForCtorParam("Description", opt => opt.MapFrom(src => src.Description.description))
            .ForCtorParam("LongDescription",opt=>opt.MapFrom(src=>src.Description.longdescription));

        CreateMap<Message, MessageDto>()
            .ForCtorParam("Description", opt => opt.MapFrom(src => src.Description));
    }
}