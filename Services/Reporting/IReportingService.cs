using Core;

namespace Services.Reporting
{
    /// <summary>
    /// Service which is responsible for reporting operations.
    /// </summary>
    public interface IReportingService
    {
        Task<FileInfo> GenerateReportAsync(IReadOnlyList<IEmployee> employees, ReportType reportType, string dirPath);
    }
}
