using OfficeOpenXml;
using Radin.Application.Services.Excelloading;
using Radin.Domain.Entities.Customers;
using Radin.Domain.Entities.Factors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.OtherExcelloading
{
    public class JobCategoryExcelLoading
    {
        private string? ConvertToNullableString(object value)
        {
            var stringValue = value?.ToString();
            return string.IsNullOrWhiteSpace(stringValue) ? null : stringValue;
        }

       

        public JobCategoryResult ReadDataFromExcel(string filePath, int worksheetNumber)
        {
            var jobCategories = new List<JobCategoryInfo>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[worksheetNumber];
                if (worksheet == null)
                    throw new ArgumentException($"Worksheet number {worksheetNumber} does not exist in the file.");

                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    var jobCategory = new JobCategoryInfo
                    {

                        Name = ConvertToNullableString(worksheet.Cells[row, 2].Value),
                        
                    };

                    jobCategories.Add(jobCategory);
                }
            }

            return new JobCategoryResult
            {
                JobCategories = jobCategories
            };
        }
    }

    public class JobCategoryResult
    {
        public List<JobCategoryInfo> JobCategories { get; set; }
    }
}
