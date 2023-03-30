using System.ComponentModel.DataAnnotations;

namespace NetMonitor.Dto;

public record HostDto(Guid Guid,
    [StringLength(255, MinimumLength = 1, ErrorMessage = "The hostname has to be between 1 and 255 characters.")] string Hostname,
    [StringLength(255, MinimumLength = 1, ErrorMessage = "The description has to be between 1 and 255 characters.")] string Description, 
    [MaxLength(255, ErrorMessage = "The description has to be between 1 and 255 characters.")] string? LongDescription,
    [RegularExpression(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}",ErrorMessage = "The IP-Address has to be IPv4.")]
    string IPAddress,
    List<ServiceDto>? Services);