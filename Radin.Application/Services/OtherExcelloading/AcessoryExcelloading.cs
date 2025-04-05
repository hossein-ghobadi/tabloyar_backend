//using OfficeOpenXml;
//using Radin.Application.Services.Excelloading;
//using Radin.Domain.Entities.Customers;
//using Radin.Domain.Entities.Factors;
//using Radin.Domain.Entities.Products;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml.Linq;

//namespace Radin.Application.Services.OtherExcelloading
//{
//    public class AcessoryExcelloading
//    {



//        private float? ConvertToNullableFloat(object value)
//        {
//            if (value == null || string.IsNullOrWhiteSpace(value.ToString())) return null;
//            return ExcelHelper.TryConvertToFloat(value, out float result, "en-US", "de-DE") ? result : (float?)null;
//        }

//        public AcessoryResultDto ReadDataFromExcel(string filePath, int WorksheetNumber)
//        {


//            var Accessories = new List<Accessory>();

//            using (var package = new ExcelPackage(new FileInfo(filePath)))
//            {



//                var AccessoryTable = package.Workbook.Worksheets[WorksheetNumber]; // Assuming the data is in the 2nd worksheet
//                for (int row = 2; row <= AccessoryTable.Dimension.End.Row; row++)
//                {
//                    //Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  {row}");
//                    if (string.IsNullOrWhiteSpace(AccessoryTable.Cells[row, 2].Value?.ToString()))
//                    {
//                        continue;
//                    }
//                    var AccessoryProperty = new Accessory
//                    {
//                        Name= AccessoryTable.Cells[row, 2].Value.ToString(),
//                        MinimumQuantity= ConvertToNullableFloat(AccessoryTable.Cells[row, 3].Value)??1,
//                        fee= ConvertToNullableFloat(AccessoryTable.Cells[row, 4].Value) ??0 ,
//                        Description = AccessoryTable.Cells[row, 5].Value?.ToString(),
//                        purchaseFee= ConvertToNullableFloat(AccessoryTable.Cells[row, 6].Value) ?? 0,

//                    };

//                    //Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> Fee= {AccessoryProperty.fee}");

//                    Accessories.Add(AccessoryProperty);



//                }
//            }

//            return new AcessoryResultDto
//            {
//                AccessoriesList = Accessories,

//            };
//        }


//        public class AcessoryResultDto
//        {
//            public List<Accessory> AccessoriesList { get; set; }

//        }








//    }
//}
