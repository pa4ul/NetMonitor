namespace NetMonitor.Dto;

public record MonitorInstanceDto(string Name, List<HostDto> Hosts, Guid Guid);