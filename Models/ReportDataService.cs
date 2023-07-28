using System.Collections.Generic;
using ReportGenerator.Models;

public class ReportDataService
{
    public List<ReportData> GetReportData()
    {
        // Replace this with your actual database retrieval logic
        // For demonstration purposes, we'll return sample data
        var sampleData = new List<ReportData>
        {
            new ReportData { Id = 1, Name = "Ram Sharma", Address = "Ktm" },
            new ReportData { Id = 2, Name = "Hari Poudel", Address = "Pokhera" },
            // Add more data as needed
        };

        return sampleData;
    }
}
