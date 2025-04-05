//using OfficeOpenXml;
//using Radin.Domain.Entities.Products;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Excelloading
//{
//    public class EdgePropertiesLoading
//    {
//        public ExcelTablesResultDto ReadDataFromExcel(string filePath, int WorksheetNumber)
//        {

            
//            var edgeProperties = new List<EdgeProperty>();

//            using (var package = new ExcelPackage(new FileInfo(filePath)))
//            {



//                var EdgePropertyTable = package.Workbook.Worksheets[WorksheetNumber]; // Assuming the data is in the 2nd worksheet
//                for (int row = 2; row <= EdgePropertyTable.Dimension.End.Row; row++)
//                {
//                    if (string.IsNullOrWhiteSpace(EdgePropertyTable.Cells[row, 2].Value?.ToString()))
//                    {
//                        continue;
//                    }
//                    var edgeProperty = new EdgeProperty
//                    {
//                        EdgeTitle = EdgePropertyTable.Cells[row, 2].Value.ToString(),
//                        EdgeColor = EdgePropertyTable?.Cells[row, 4].Value?.ToString(),
//                        EdgeSecondColor = EdgePropertyTable?.Cells[row, 6].Value?.ToString(),
//                        ImplementationModel = EdgePropertyTable?.Cells[row, 7].Value?.ToString(),
//                        QualityFactor = EdgePropertyTable?.Cells[row, 8].Value?.ToString(),

//                        //IsDefault = Convert.ToBoolean(EdgePropertyTable.Cells[row, 12].Value),
//                    };

//                    //......................................................................................................
//                    float edgeSize;
//                    ExcelHelper.TryConvertToFloat(EdgePropertyTable.Cells[row, 3].Value, out edgeSize, "en-US", "de-DE");
//                    edgeProperty.EdgeSize = edgeSize;
//                    //......................................................................................................
//                    float edgeWorkerFee;
//                    ExcelHelper.TryConvertToFloat(EdgePropertyTable.Cells[row, 9].Value, out edgeWorkerFee, "en-US", "de-DE");
//                    edgeProperty.EdgeWorkerFee = edgeWorkerFee;

//                    //......................................................................................................
//                    float EdgeFee;
//                    ExcelHelper.TryConvertToFloat(EdgePropertyTable.Cells[row, 10].Value, out EdgeFee, "en-US", "de-DE");
//                    edgeProperty.EdgeFee = EdgeFee;
//                    //......................................................................................................
//                    float EdgeHardnessFactor;
//                    ExcelHelper.TryConvertToFloat(EdgePropertyTable.Cells[row, 11].Value, out EdgeHardnessFactor, "en-US", "de-DE");
//                    edgeProperty.EdgeHardnessFactor = EdgeHardnessFactor;

//                    var V_IsDefault = EdgePropertyTable.Cells[row, 12].Value;
//                    edgeProperty.IsDefault = V_IsDefault != null ? Convert.ToBoolean(V_IsDefault) : false;
//                    //......................................................................................................
//                    var V_edgeThickness = EdgePropertyTable.Cells[row, 5].Value;
//                    if (V_edgeThickness == null || string.IsNullOrWhiteSpace(V_edgeThickness.ToString()))
//                    {
//                        edgeProperty.EdgeThickness = null;
//                    }
//                    else
//                    {
//                        // Proceed with conversion
//                        ExcelHelper.TryConvertToFloat(V_edgeThickness, out float edgeThickness, "en-US", "de-DE");
//                        edgeProperty.EdgeThickness = edgeThickness;
//                    }
//                    edgeProperties.Add(edgeProperty);



//                }
//            }
        
//            return new ExcelTablesResultDto
//            {
//                EdgeProperties = edgeProperties,

//            };
//        }
//    }
//    public class ExcelTablesResultDto
//    {
//    public List<EdgeProperty> EdgeProperties { get; set; }
//    }

//}