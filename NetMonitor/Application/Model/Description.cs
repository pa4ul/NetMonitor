using System.ComponentModel.DataAnnotations;

namespace NetMonitor.Application.Model;

public record Description([MaxLength(255)] string description, [MaxLength(255)] string? longdescription);