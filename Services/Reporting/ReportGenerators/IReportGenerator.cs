using Core;

namespace Services.Reporting.ReportGenerators
{
    /// <summary>
    /// Interface which is responsible for creation of reports.
    /// </summary>
    public interface IReportGenerator
    {
        Task<FileInfo> GenerateReportAsync(IReadOnlyList<IEmployee> employees, string dirPath);
    }
}
