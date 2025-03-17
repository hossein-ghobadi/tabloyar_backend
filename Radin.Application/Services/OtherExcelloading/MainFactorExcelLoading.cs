using OfficeOpenXml;
using Radin.Application.Services.Excelloading;
using Radin.Domain.Entities.Customers;
using Radin.Domain.Entities.Factors;
using Radin.Domain.Entities.Products.Aditional;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.OtherExcelloading
{
    public class MainFactorExcelLoading
    {

        private string? ConvertToNullableString(object value)
        {
            var stringValue = value?.ToString();
            return string.IsNullOrWhiteSpace(stringValue) ? null : stringValue;
        }

        private int? ConvertToNullableInt(object value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return null;
            return ExcelHelper.TryConvertToInt(value, out int result, "en-US", "de-DE") ? result : (int?)null;
        }

        private float? ConvertToNullableFloat(object value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return null;
            return ExcelHelper.TryConvertToFloat(value, out float result, "en-US", "de-DE") ? result : (float?)null;
        }
        public DateTime? ConvertToNullableDateTime1(object excelValue)
        {
            if (excelValue == null || string.IsNullOrWhiteSpace(excelValue.ToString()))
                return null;

            DateTime parsedDate;
            if (DateTime.TryParse(excelValue.ToString(), out parsedDate))
                return parsedDate;

            return null;
        }
        private bool ConvertToBool(object value)
        {
            return value != null ? Convert.ToBoolean(value) : false;

        }

        private DateTime ConvertToDateTime(object cellValue)
        {
            // Try to parse the value as a serial date
            if (double.TryParse(cellValue.ToString(), out double serialDate))
            {
                DateTime baseDate = new DateTime(1900, 1, 1);
                return baseDate.AddDays(serialDate - 2); // Adjust for Excel's date system starting from day 1 and leap year bug
            }

            // Directly parse as a standard date string if not in serial format
            return DateTime.Parse(cellValue.ToString());
        }



        public ExcelResultDto ReadDataFromExcel(string filePath, int worksheetNumber)
        {
            var mainFactors = new List<MainFactor>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[worksheetNumber];
                if (worksheet == null)
                    throw new ArgumentException($"Worksheet number {worksheetNumber} does not exist in the file.");

                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    var mainFactor = new MainFactor
                    {

                        BranchCode = Convert.ToInt64(worksheet.Cells[row, 2].Value),
                        ConnectionCount = ConvertToNullableInt(worksheet.Cells[row, 3].Value) ?? 0,
                        ConnectionDuration = ConvertToNullableInt(worksheet.Cells[row, 4].Value),
                        InitialConnectionTime = ConvertToDateTime(worksheet.Cells[row, 5].Value),// ConvertToNullableDateTime(worksheet.Cells[row, 5].Value) ?? default,
                        LastConnectionTime= new DateTime(int.Parse(worksheet.Cells[row, 8].Value.ToString()), int.Parse(worksheet.Cells[row, 9].Value.ToString()), int.Parse(worksheet.Cells[row, 10].Value.ToString())),
                        TatilRasmi = ConvertToBool(worksheet.Cells[row, 6].Value),
                        dayofweek = ConvertToNullableString(worksheet.Cells[row, 7].Value),
                        year = ConvertToNullableString(worksheet.Cells[row, 8].Value),
                        month = ConvertToNullableString(worksheet.Cells[row, 9].Value),
                        day = ConvertToNullableString(worksheet.Cells[row, 10].Value),
                        ReasonStatus = ConvertToNullableString(worksheet.Cells[row, 11].Value),
                        WorkName = ConvertToNullableString(worksheet.Cells[row, 12].Value),
                        RecommandedDesign = ConvertToNullableInt(worksheet.Cells[row, 13].Value),
                        SelectedDesign = ConvertToNullableString(worksheet.Cells[row, 14].Value),
                        MainsellerID = ConvertToNullableString(worksheet.Cells[row, 15].Value),
                        AssistantSellerID = ConvertToNullableString(worksheet.Cells[row, 16].Value),
                        AssistantSellerPercent = ConvertToNullableInt(worksheet.Cells[row, 17].Value),
                        TotalDiscount = ConvertToNullableFloat(worksheet.Cells[row, 18].Value),
                        TotalPackingCost = ConvertToNullableFloat(worksheet.Cells[row, 19].Value),
                        count = ConvertToNullableInt(worksheet.Cells[row, 20].Value) ?? 1,
                        fee = ConvertToNullableFloat(worksheet.Cells[row, 21].Value),
                        TotalAmount = ConvertToNullableFloat(worksheet.Cells[row, 22].Value),
                        CustomerID = ConvertToNullableInt(worksheet.Cells[row, 23].Value),
                        position = ConvertToBool(worksheet.Cells[row, 24].Value),
                        ExpireTime = ConvertToDateTime(worksheet.Cells[row, 25].Value),//ConvertToNullableDateTime(worksheet.Cells[row, 25].Value) ?? DateTime.MinValue,
                        state = ConvertToNullableInt(worksheet.Cells[row, 26].Value) ?? 0,
                        status = ConvertToBool(worksheet.Cells[row, 27].Value),
                        PurchaseProbability = ConvertToNullableFloat(worksheet.Cells[row, 28].Value) ?? 0,
                        description = ConvertToNullableString(worksheet.Cells[row, 29].Value),
                        ContactType= ConvertToNullableInt(worksheet.Cells[row, 30].Value)?? 0
                    };
                    //var V_InitialConnectionTime = worksheet.Cells[row, 5].Value;
                    //if (V_InitialConnectionTime == null || string.IsNullOrWhiteSpace(V_InitialConnectionTime.ToString()))
                    //{
                    //    mainFactor.InitialConnectionTime = null;
                    //}
                    //else
                    //{
                    //    Proceed with conversion
                    //   DateTime? birthday = ConvertToNullableDateTime(V_InitialConnectionTime);
                    //    var x = DateTime.TryParse(V_InitialConnectionTime.ToString(), out zvfzsfdz);
                    //mainFactor.InitialConnectionTime = (DateTime)birthday;
                    mainFactors.Add(mainFactor);
                }
            }

                return new ExcelResultDto
                {
                    MainFactors = mainFactors
                };
            }
        }

        public class ExcelResultDto
        {
            public List<MainFactor> MainFactors { get; set; }
        }
    }
