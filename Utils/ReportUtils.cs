using AspNetCore.Reporting;
using System.Reflection;
using System.Text;

namespace project_backend.Utils
{
    public static class ReportUtils
    {
        public static LocalReport GetReport(string reportName)
        {
            string fileDirPath = Assembly.GetExecutingAssembly().Location.Replace("project_backend.dll", string.Empty);
            string rdlcFilePath = string.Format("{0}Reports\\{1}.rdlc", fileDirPath, reportName);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("utf-8");

            LocalReport report = new(rdlcFilePath);

            return report;
        }
    }
}
