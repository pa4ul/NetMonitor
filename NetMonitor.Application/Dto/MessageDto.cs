using Bogus.DataSets;

namespace NetMonitor.Dto;

public record MessageDto(string Description, string LongDescription, DateTime Date, string Type);   