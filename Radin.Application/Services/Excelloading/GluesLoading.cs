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
    public class GluesLoading
    {
        
            public ExcelTablesResultDto ReadDataFromExcel(string filePath, int WorksheetNumber)
            {
                var glues = new List<Glue>();

                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var GluesTable = package.Workbook.Worksheets[WorksheetNumber]; // Assuming the data is in the third worksheet
                    for (int row = 2; row <= GluesTable.Dimension.End.Row; row++)
                    {

                    if (string.IsNullOrWhiteSpace(GluesTable.Cells[row, 2].Value?.ToString()))
                    {
                        continue;
                    }
                    var glue = new Glue
                        {
                            GlueTitle = GluesTable.Cells[row, 2].Value.ToString(),
                            QualityFactor = GluesTable.Cells[row, 4].Value.ToString(),


                        };
                       
                        float glueFee;
                        ExcelHelper.TryConvertToFloat(GluesTable.Cells[row, 3].Value, out glueFee, "en-US", "de-DE");
                        glue.GLueFee = glueFee;



                        glues.Add(glue);
                    }



                    return new ExcelTablesResultDto
                    {
                        GLues = glues

                    };
                }
            }
            public class ExcelTablesResultDto
            {
                public List<Glue> GLues { get; set; }
            }
        }
}
