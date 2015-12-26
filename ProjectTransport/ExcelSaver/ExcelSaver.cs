using Bytescout.Spreadsheet;
using GPSDataService.Models;
using ServiceLibrary.ProjectService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExcelIO
{
    public class ExcelSaver
    {
        string _filepath;

        public Color RouteHeaders { get; set; }
        public Color RouteValues { get; set; }
        public Color RouteDataHeader1 { get; set; }
        public Color RouteDataHeader2 { get; set; }
        public Color OddRow { get; set; }
        public Color EvenRow { get; set; }

        public string HeaderString { get; set; }
        public string StartPointString { get; set; }
        public string EndPointString { get; set; }
        public string RouteDataHeader { get; set; }
        public string RouteDataTimeHeader { get; set; }
        public string RouteDataLatitudeHeader { get; set; }
        public string RouteDataLongitudeHeader { get; set; }
        public string RouteDataHeightHeader { get; set; }
        public string RouteDataFuelLevelHeader { get; set; }
        public string RouteDataCostDescriptionHeader { get; set; }
        public string RouteDataCostPriceHeader { get; set; }

        public ExcelSaver(string filepath)
        {
            _filepath = filepath;
            RouteHeaders = Color.LightYellow;
            RouteValues = Color.LightSlateGray;
            RouteDataHeader1 = Color.LightSteelBlue;
            RouteDataHeader2 = Color.MediumSeaGreen;
            OddRow = Color.Beige;
            EvenRow = Color.Crimson;
            HeaderString ="Route:";
            StartPointString ="Start point:";
            EndPointString ="End point:";
            RouteDataHeader ="Route data:";
            RouteDataTimeHeader = "TIME";
            RouteDataLatitudeHeader = "LATITUDE";
            RouteDataLongitudeHeader = "LONGITUDE";
            RouteDataHeightHeader = "HEIGHT";
            RouteDataFuelLevelHeader = "FUEL LEVEL";
            RouteDataCostDescriptionHeader = "COSTS - DESCRIPTION";
            RouteDataCostPriceHeader = "COSTS - PRICE";
        }

        public bool Export(IEnumerable<Route> routes, bool openAfter)
        {
            try
            {
                Spreadsheet document = new Spreadsheet();

                foreach (var route in routes)
                {
                    Worksheet worksheet = document.Workbook.Worksheets.Add(route.RouteName);

                    worksheet.Range(0, 0, 2, 0).FillPatternForeColor = RouteHeaders;
                    worksheet.Range(0, 0, 2, 0).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                    worksheet.Range(0, 2, 2, 2).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                    worksheet.Range(0, 0, 0, 2).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                    worksheet.Range(2, 0, 2, 2).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                    worksheet.Range(0, 1, 2, 2).FillPatternForeColor = RouteValues;
                    worksheet.Range(4, 0, 4, 6).FillPatternForeColor = RouteDataHeader1;
                    worksheet.Range(5, 0, 5, 6).FillPatternForeColor = RouteDataHeader2;


                    worksheet.Range(0, 0, 2, 0).FillPattern = Bytescout.Spreadsheet.Constants.PatternStyle.Solid;
                    worksheet.Range(0, 1, 2, 2).FillPattern = Bytescout.Spreadsheet.Constants.PatternStyle.Solid;
                    worksheet.Range(4, 0, 4, 6).FillPattern = Bytescout.Spreadsheet.Constants.PatternStyle.Solid;
                    worksheet.Range(5, 0, 5, 6).FillPattern = Bytescout.Spreadsheet.Constants.PatternStyle.Solid;

                    worksheet.Cell(0, 0).Value = "Route:";
                    worksheet.Cell(0, 1).Value = route.RouteName;

                    worksheet.Cell(1, 0).Value = "Start point:";
                    worksheet.Cell(1, 1).Value = route.StartPoint.Latitude;
                    worksheet.Cell(1, 2).Value = route.StartPoint.Longitude;

                    worksheet.Cell(2, 0).Value = "End point:";
                    worksheet.Cell(2, 1).Value = route.EndPoint.Latitude;
                    worksheet.Cell(2, 2).Value = route.EndPoint.Longitude;

                    worksheet.Cell(4, 0).Value = RouteDataHeader;
                    worksheet.Cell(5, 0).Value = RouteDataTimeHeader;
                    worksheet.Cell(5, 1).Value = RouteDataLatitudeHeader;
                    worksheet.Cell(5, 2).Value = RouteDataLongitudeHeader;
                    worksheet.Cell(5, 3).Value = RouteDataHeightHeader;
                    worksheet.Cell(5, 4).Value = RouteDataFuelLevelHeader;
                    worksheet.Cell(5, 5).Value = RouteDataCostDescriptionHeader;
                    worksheet.Cell(5, 6).Value = RouteDataCostPriceHeader;
                    worksheet.Range(4, 0, 4, 6).TopBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                    worksheet.Range(4, 0, 4, 6).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.ThinDashDotted;
                    worksheet.Range(5, 0, 5, 6).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Medium;
                    worksheet.Range(4, 0, 5, 0).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                    worksheet.Range(4, 6, 5, 6).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                    int data = 0;
                    int currentRow = 6;
                    foreach (var routeData in route.RouteData)
                    {
                        int startRow = currentRow;
                        worksheet.Cell(currentRow, 0).Value = routeData.Time;
                        worksheet.Cell(currentRow, 0).NumberFormatString = "dd/mm/yyyy hh:mm:ss";
                        worksheet.Cell(currentRow, 1).Value = routeData.Position.Latitude;
                        worksheet.Cell(currentRow, 2).Value = routeData.Position.Longitude;
                        worksheet.Cell(currentRow, 3).Value = routeData.Height;
                        worksheet.Cell(currentRow, 4).Value = routeData.FuelLevel;
                        foreach (var cost in routeData.AdditionalCosts)
                        {
                            worksheet.Cell(currentRow, 5).Value = cost.Description;
                            worksheet.Cell(currentRow, 6).Value = cost.Price;
                            worksheet.Cell(currentRow, 6).NumberFormatString = "0.00";
                            currentRow++;
                        }
                        if (routeData.AdditionalCosts.Count() == 0) currentRow++;

                        if (data % 2 == 0)
                        {
                            worksheet.Range(startRow, 0, currentRow - 1, 6).FillPatternForeColor = OddRow;
                            worksheet.Range(startRow, 0, currentRow - 1, 6).FillPattern = Bytescout.Spreadsheet.Constants.PatternStyle.Solid;
                        }
                        else
                        {
                            worksheet.Range(startRow, 0, currentRow - 1, 6).FillPatternForeColor = EvenRow;
                            worksheet.Range(startRow, 0, currentRow - 1, 6).FillPattern = Bytescout.Spreadsheet.Constants.PatternStyle.Solid;
                        }

                        worksheet.Range(startRow, 0, currentRow - 1, 0).LeftBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        worksheet.Range(startRow, 6, currentRow - 1, 6).RightBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;
                        worksheet.Range(currentRow - 1, 0, currentRow - 1, 6).BottomBorderStyle = Bytescout.Spreadsheet.Constants.LineStyle.Thin;

                        data++;
                    }
                    worksheet.Columns[0].AutoFit();
                    worksheet.Columns[1].AutoFit();
                    worksheet.Columns[2].AutoFit();
                    worksheet.Columns[3].AutoFit();
                    worksheet.Columns[4].AutoFit();
                    worksheet.Columns[5].AutoFit();
                    worksheet.Columns[6].AutoFit();
                }


                if (File.Exists(_filepath))
                {
                    File.Delete(_filepath);
                }

                document.SaveAs(_filepath);
                document.Close();

                if (openAfter) Process.Start(_filepath);
                return true;

            }
            catch
            {
                return false;
            }
        }
    }
}
