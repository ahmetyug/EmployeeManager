using Core;

namespace Services.Reporting.ReportGenerators
{
    public class ReportGeneratorFactory : IReportGeneratorFactory
    {
        public IReportGenerator GetReportGenerator(ReportType reportType)
        {
            switch (reportType)
            {
                case ReportType.CSV:
                    return new ReportGeneratorCSV();
                default:
                    throw new NotSupportedException($"{reportType.ToString()} is not supported.");
            }
        }
    }
}
