using AutoMapper;
using NetMonitor.Dto;
using NetMonitor.Model;
using NetMonitor.Webapp.Dto;
using Host = NetMonitor.Model.Host;

namespace NetMonitor.Webapp.wwwroot.Dto;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Service,ServiceDto>();
        
        CreateMap<Host,HostDto>();

    }
}