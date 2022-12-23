using System.ComponentModel.DataAnnotations;

namespace NetMonitor.Model;

public record Description([MaxLength(255)] string description, [MaxLength(255)] string? longdescription = "");