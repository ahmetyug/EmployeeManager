using Core;
using Utility;

namespace Services.Reporting.ReportGenerators
{
    /// <summary>
    /// Report generator specifically generating csv reports.
    /// </summary>
    internal class ReportGeneratorCSV : IReportGenerator
    {
        public async Task<FileInfo> GenerateReportAsync(IReadOnlyList<IEmployee> employees, string dirPath)
        {
            var reportData = CSVGenerator.GenerateCsv(employees);

            var fileName = $"{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")}_Employees_{ReportType.CSV}.csv";
                
            var filePath = Path.Combine(dirPath, fileName);

            await File.WriteAllTextAsync(filePath, reportData.ToString());

            return new FileInfo(filePath);
        }
    }
}
