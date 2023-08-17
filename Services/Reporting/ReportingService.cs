using Core;
using Services.Reporting.ReportGenerators;

namespace Services.Reporting
{
    public class ReportingService : IReportingService
    {
        private readonly IReportGeneratorFactory reportGeneratorFactory;

        public ReportingService(IReportGeneratorFactory reportGeneratorFactory)
        {
            this.reportGeneratorFactory = reportGeneratorFactory;
        }

        public async Task<FileInfo> GenerateReportAsync(IReadOnlyList<IEmployee> employees, ReportType reportType, string dirPath)
        {
            var reportGenerator = reportGeneratorFactory.GetReportGenerator(reportType);

            var fileInfo = await reportGenerator.GenerateReportAsync(employees, dirPath);

            return fileInfo;
        }
    }
}
