using NetMonitor.Model;

namespace NetMonitor.Webapp.Dto;

public record MonitorInstanceDto(string Name, List<HostDto> Hosts, Guid Guid, User Manager);