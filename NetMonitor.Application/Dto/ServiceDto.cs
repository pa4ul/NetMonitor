namespace NetMonitor.Dto;

public record ServiceDto(Guid Guid, string Description, string? LongDescription, int NormalInterval, int RetryInterval,
    string ServiceType, List<MessageDto> Messages);