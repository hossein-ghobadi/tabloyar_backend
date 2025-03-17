using OfficeOpenXml;
using Radin.Domain.Entities.Products;
using Radin.Domain.Entities.Products.Aditional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Excelloading
{
    public class TitlesLoading
    {
        public ExcelTablesResultDto ReadDataFromExcel(string filePath, int WorksheetNumber)
        {
            var titles = new List<Title>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var TitlesTable = package.Workbook.Worksheets[WorksheetNumber]; // Assuming the data is in the 10th worksheet
                for (int row = 2; row <= TitlesTable.Dimension.End.Row; row++)
                {
                    if (string.IsNullOrWhiteSpace(TitlesTable.Cells[row, 2].Value?.ToString()))
                    {
                        continue;
                    }
                    var title = new Title
                    {
                        TitleName = TitlesTable.Cells[row, 2].Value.ToString(),
                        

                    };
                    var V_IsDefault = TitlesTable.Cells[row, 3].Value;
                    title.IsDefault = V_IsDefault != null ? Convert.ToBoolean(V_IsDefault) : false;
                    titles.Add(title);
                }


                return new ExcelTablesResultDto
                {
                    Titles = titles

                };
            }
        }
        public class ExcelTablesResultDto
        {
            public List<Title> Titles { get; set; }
        }

    }
}

