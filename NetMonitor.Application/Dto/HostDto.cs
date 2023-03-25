namespace NetMonitor.Dto;

public record HostDto(Guid Guid, string Hostname, string Description, string? LongDescription, string IPAddress,
    List<ServiceDto> Services);