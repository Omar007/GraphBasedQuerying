using System.Collections.Generic;
using System.IO;
using System.Linq;
using DbTestBase.Tests;
using PerformanceFramework;

namespace DbTest.Core.Writers
{
    internal class XmlWriter : IWriter
    {
        public void Write(DirectoryInfo dir, string filePrefix, IEnumerable<TestBase> tests)
        {
            using (var output = new StreamWriter(dir.FullName + filePrefix + "xml"))
            {
                output.WriteLine(@"<?xml version=""1.0""?>");
                output.WriteLine(@"<?mso-application progid=""Excel.Sheet""?>");
                output.WriteLine(@"<Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet""");
                output.WriteLine(@"  xmlns:o=""urn:schemas-microsoft-com:office:office""");
                output.WriteLine(@"  xmlns:x=""urn:schemas-microsoft-com:office:excel""");
                output.WriteLine(@"  xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet""");
                output.WriteLine(@"  xmlns:html=""http://www.w3.org/TR/REC-html40""");
                output.WriteLine(@"  xmlns:x2=""http://schemas.microsoft.com/office/excel/2003/xml"">");
                output.WriteLine(@" <Styles>");
                output.WriteLine(@"  <Style ss:ID=""s62"">");
                output.WriteLine(@"   <NumberFormat ss:Format=""mm:ss.000""/>");
                output.WriteLine(@"  </Style>");
                output.WriteLine(@" </Styles>");

                foreach (var testGroup in tests.GroupBy(t => (t as PopulationTest).ModelName).OrderBy(g => g.Key))
                {
                    output.WriteLine(@" <Worksheet ss:Name=""{0}"">", testGroup.Key);
                    output.WriteLine(@"  <Table>");
                    output.WriteLine(@"   <Column ss:Width=""100""/>");
                    output.WriteLine(@"   <Column ss:Width=""100""/>");
                    output.WriteLine(@"   <Column ss:Width=""100""/>");
                    output.WriteLine(@"   <Column ss:Width=""100""/>");
                    output.WriteLine(@"   <Column ss:Width=""100""/>");

                    output.WriteLine(@"   <Row>");
                    output.WriteLine(@"     <Cell><Data ss:Type=""String"">Population</Data></Cell>");
                    output.WriteLine(@"     <Cell><Data ss:Type=""String"">Minimum</Data></Cell>");
                    output.WriteLine(@"     <Cell><Data ss:Type=""String"">Maximum</Data></Cell>");
                    output.WriteLine(@"     <Cell><Data ss:Type=""String"">Average</Data></Cell>");
                    output.WriteLine(@"     <Cell><Data ss:Type=""String"">Median</Data></Cell>");
                    output.WriteLine(@"     <Cell><Data ss:Type=""String"">Standard Deviation</Data></Cell>");
                    output.WriteLine(@"     <Cell><Data ss:Type=""String"">Thread Minimum</Data></Cell>");
                    output.WriteLine(@"     <Cell><Data ss:Type=""String"">Thread Maximum</Data></Cell>");
                    output.WriteLine(@"     <Cell><Data ss:Type=""String"">Thread Average</Data></Cell>");
                    output.WriteLine(@"     <Cell><Data ss:Type=""String"">Thread Median</Data></Cell>");
                    output.WriteLine(@"     <Cell><Data ss:Type=""String"">Thread Standard Deviation</Data></Cell>");
                    output.WriteLine(@"   </Row>");

                    foreach (var resultGroup in testGroup.GroupBy(t => (t as PopulationTest).Population.ECount).OrderBy(g => g.Key))
                    {
                        output.WriteLine(@"   <Row>");
                        output.WriteLine(@"    <Cell><Data ss:Type=""Number"">{0}</Data></Cell>", resultGroup.Key);

                        var testResult = resultGroup.Single();
                        if (testResult.HasResults)
                        {
                            output.WriteLine(@"    <Cell ss:StyleID=""s62""><Data ss:Type=""DateTime"">1899-12-31T{0}</Data></Cell>", testResult.Minimum);
                            output.WriteLine(@"    <Cell ss:StyleID=""s62""><Data ss:Type=""DateTime"">1899-12-31T{0}</Data></Cell>", testResult.Maximum);
                            output.WriteLine(@"    <Cell ss:StyleID=""s62""><Data ss:Type=""DateTime"">1899-12-31T{0}</Data></Cell>", testResult.Average);
                            output.WriteLine(@"    <Cell ss:StyleID=""s62""><Data ss:Type=""DateTime"">1899-12-31T{0}</Data></Cell>", testResult.Median);
                            output.WriteLine(@"    <Cell ss:StyleID=""s62""><Data ss:Type=""DateTime"">1899-12-31T{0}</Data></Cell>", testResult.StandardDeviation);
                            output.WriteLine(@"    <Cell ss:StyleID=""s62""><Data ss:Type=""DateTime"">1899-12-31T{0}</Data></Cell>", testResult.ThreadMinimum);
                            output.WriteLine(@"    <Cell ss:StyleID=""s62""><Data ss:Type=""DateTime"">1899-12-31T{0}</Data></Cell>", testResult.ThreadMaximum);
                            output.WriteLine(@"    <Cell ss:StyleID=""s62""><Data ss:Type=""DateTime"">1899-12-31T{0}</Data></Cell>", testResult.ThreadAverage);
                            output.WriteLine(@"    <Cell ss:StyleID=""s62""><Data ss:Type=""DateTime"">1899-12-31T{0}</Data></Cell>", testResult.ThreadMedian);
                            output.WriteLine(@"    <Cell ss:StyleID=""s62""><Data ss:Type=""DateTime"">1899-12-31T{0}</Data></Cell>", testResult.ThreadStandardDeviation);
                        }

                        output.WriteLine(@"   </Row>");
                    }

                    output.WriteLine(@"  </Table>");
                    output.WriteLine(@" </Worksheet>");
                }
                output.WriteLine(@"</Workbook>");
                output.Close();
            }
        }
    }
}
