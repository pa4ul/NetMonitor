using System.ComponentModel.DataAnnotations;

namespace NetMonitor.Dto;

public record ServiceCmd(
    [StringLength(255, MinimumLength = 1, ErrorMessage = "The description has to be between 1 and 255 characters.")] string Description, 
    [StringLength(255, MinimumLength = 1, ErrorMessage = "The description has to be between 1 and 255 characters.")] string? LongDescription, 
    [Range(0, int.MaxValue, ErrorMessage = "Please enter valid positive integer Number")] int NormalInterval, 
    [Range(0, int.MaxValue, ErrorMessage = "Please enter valid positive integer Number")] int RetryInterval);