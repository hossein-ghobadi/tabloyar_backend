using OfficeOpenXml;
using Radin.Domain.Entities.Others;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.StateInfoLoadingExcel
{
    public class ExcelloadingForCities
    {
        public ExcelTablesResultCityDto ReadDataFromExcel(string filePath, int WorksheetNumber)
        {
            var infos = new List<CityInfo>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var CityTable = package.Workbook.Worksheets[WorksheetNumber]; // Assuming the data is in the 10th worksheet
                for (int row = 2; row <= CityTable.Dimension.End.Row; row++)
                {
                    var info = new CityInfo
                    { 
                        //Id = Convert.ToInt16(CityTable.Cells[row, 1].Value),
                        city = CityTable.Cells[row, 2].Value.ToString(),
                        point = CityTable.Cells[row, 3].Value.ToString(),
                        CountryId = Convert.ToInt16(CityTable.Cells[row, 4].Value),
                        Country = CityTable.Cells[row, 5].Value.ToString(),
                        ProvinceId = Convert.ToInt16(CityTable.Cells[row, 6].Value),
                        province = CityTable.Cells[row, 7].Value.ToString(),
                        county = CityTable.Cells[row, 8].Value?.ToString(),
                        district = CityTable.Cells[row, 9].Value?.ToString(),
                        polygon = CityTable.Cells[row, 10].Value?.ToString()
                    };

                    infos.Add(info);
                }


                return new ExcelTablesResultCityDto
                {
                    Infos = infos

                };
            }
        }
        public class ExcelTablesResultCityDto
        {
            public List<CityInfo> Infos { get; set; }
        }
    }
}
