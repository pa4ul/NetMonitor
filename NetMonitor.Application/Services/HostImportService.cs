using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using NetMonitor.Infrastructure;
using NetMonitor.Model;

namespace NetMonitor.Services;

public class HostImportService
{
    private class CsvRow
    {
        public string Hostname { get; set; } = default!;
        public string IPAddress { get; set; } = default!;
        public Description Description { get; set; } = default!;
    }

    private class CsvRowMap : ClassMap<CsvRow>
    {
        public CsvRowMap()
        {
            Map(row => row.Hostname).Index(0); // 1st column is ean number
            Map(row => row.IPAddress).Index(1);
            Map(row => row.Description.description).Index(2);
            Map(row => row.Description.longdescription).Index(3);
        }
    }


    private readonly NetMonitorContext _db;

    public HostImportService(NetMonitorContext db)
    {
        _db = db;
    }

    public (bool success, string message) LoadCsv(Stream stream)
    {
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = "\t",
            NewLine = "\r\n",
            ReadingExceptionOccurred = (context) => false // true: rethrow exception, false: ignore
        };
        using var reader = new StreamReader(stream, new UTF8Encoding(false));
        using var csv = new CsvReader(reader, csvConfig);
        csv.Context.RegisterClassMap<CsvRowMap>();
        try
        {
            var records = csv.GetRecords<CsvRow>().ToList();
            return WriteToDatabase(records);
        }
        catch (CsvHelperException ex)
        {
            return (false, $"Fehler beim Lesen der Zeile {ex.Context.Parser.Row}: {ex.Message}");
        }
    }
    
    private (bool success, string message) WriteToDatabase(IEnumerable<CsvRow> csvRows)
    {
        //var existingIPs = _db.Hosts.Select(h => h.IPAddress).ToHashSet();
        var newHosts = csvRows.Select(h => new Host(h.Hostname, h.IPAddress, h.Description));
        _db.Hosts.AddRange(newHosts);
        try
        {
            var count = _db.SaveChanges();
            return (true, $"{count} Hosts imported.");
        }
        catch (DbUpdateException ex)
        {
            return (false, ex.InnerException?.Message ?? ex.Message);
        }
    }
}