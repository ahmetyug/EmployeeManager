using Core;

namespace Services.Reporting.ReportGenerators
{
    public interface IReportGeneratorFactory
    {
        /// <summary>
        /// Factory method creating <see cref="IReportGenerator"/> depending on <see cref="ReportType"/>
        /// </summary>
        /// <param name="reportType"><see cref="ReportType"/></param>
        /// <returns><see cref="IReportGenerator"/></returns>
        IReportGenerator GetReportGenerator(ReportType reportType);
    }
}
