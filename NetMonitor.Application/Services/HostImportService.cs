using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using NetMonitor.Infrastructure;
using NetMonitor.Model;

namespace NetMonitor.Services;

public class HostImportService
{
    private class CsvRow
    {
        public string Hostname { get; set; } = default!;

        [RegularExpression(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}")]
        public string IPAddress { get; set; } = default!;

        public string DescriptionShort { get; set; } = default!;
        public string? DescriptionLong { get; set; } = default!;
    }

    private class CsvRowMap : ClassMap<CsvRow>
    {
        public CsvRowMap()
        {
            Map(row => row.Hostname).Index(0); // 1st column is ean number
            Map(row => row.IPAddress).Index(1);
            Map(row => row.DescriptionShort).Index(2);
            Map(row => row.DescriptionLong).Index(3).Optional();
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
            Delimiter = ",",
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

    public (bool success, string message) LoadExcel(Stream stream, int maxRows = 1000)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using var reader = ExcelReaderFactory.CreateReader(stream);
        reader.Read(); // Ignore header
        var csvRows = new List<CsvRow>(1024);
        int rowNumber = 0;
        while (reader.Read() && rowNumber++ < maxRows)
        {
            if (reader.FieldCount < 4)
            {
                break;
            }

            if (reader.IsDBNull(0))
            {
                break;
            }

            try
            {
                csvRows.Add(new CsvRow
                {
                    Hostname = reader.IsDBNull(0)
                        ? throw new ApplicationException("Invalid Hostname")
                        : reader.GetString(0),
                    IPAddress = reader.IsDBNull(1) ? throw new ApplicationException("Invalid IP") : reader.GetString(1),
                    DescriptionShort = reader.IsDBNull(2)
                        ? throw new ApplicationException("Invalid Description Short")
                        : reader.GetString(2),
                    DescriptionLong = reader.GetString(3),
                });
            }
            catch
            {
            } // Ignore invalid rows
        }

        return WriteToDatabase(csvRows);
    }


    private (bool success, string message) WriteToDatabase(IEnumerable<CsvRow> csvRows)
    {
        var existingHosts = _db.Hosts.Select(h => h.Hostname).ToHashSet();
        var newHosts = csvRows
            .Where(h=>!existingHosts.Contains(h.Hostname))
            .Select(h => new Host(hostname: h.Hostname, ipaddress: h.IPAddress,
                description: new Description(h.DescriptionShort, h.DescriptionLong)));

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