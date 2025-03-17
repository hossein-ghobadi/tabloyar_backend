using OfficeOpenXml;
using Radin.Application.Services.Excelloading;
using Radin.Domain.Entities.Customers;
using Radin.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.OtherExcelloading
{
    public class CustomerExcelLoading
    {
        private string? ConvertToNullableString(object value)
        {
            var stringValue = value?.ToString();
            return string.IsNullOrWhiteSpace(stringValue) ? null : stringValue;
        }
        public ExcelTablesResultDto ReadDataFromExcel(string filePath, int WorksheetNumber)
        {

            var customers = new List<CustomerInfo>();
            
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {


                ////////////////////////////////////////////////////////____________________________________________________________________________________________________
                var MaterialTable = package.Workbook.Worksheets[WorksheetNumber]; // Assuming the data is in the first worksheet
                for (int row = 2; row <= MaterialTable.Dimension.End.Row; row++)
                {
                    var customer = new CustomerInfo
                    {
                        //Id = Convert.ToInt32(MaterialTable.Cells[row, 0].Value.ToString()),
                        Name = ConvertToNullableString(MaterialTable.Cells[row, 3].Value),
                        LastName = ConvertToNullableString(MaterialTable.Cells[row, 4].Value),
                        Address = ConvertToNullableString(MaterialTable.Cells[row, 16].Value),
                        phone = ConvertToNullableString(MaterialTable.Cells[row, 15].Value)
                    };

                    var V_customerId = MaterialTable.Cells[row, 2].Value;
                    if (V_customerId == null || string.IsNullOrWhiteSpace(V_customerId.ToString()))
                    {
                        customer.CustomerID = null;
                    }
                    else
                    {
                        // Proceed with conversion
                        ExcelHelper.TryConvertToInt(V_customerId, out int CustomerId, "en-US", "de-DE");
                        customer.CustomerID = Convert.ToInt64(CustomerId);
                    }

                    var V_gender = MaterialTable.Cells[row, 5].Value;
                    if (V_gender == null || string.IsNullOrWhiteSpace(V_gender.ToString()))
                    {
                        customer.Gender = null;
                    }
                    else
                    {
                        // Proceed with conversion
                        ExcelHelper.TryConvertToInt(V_gender, out int gender, "en-US", "de-DE");
                        customer.Gender = gender;
                    }


                    var V_jobCategory = MaterialTable.Cells[row, 6].Value;
                    if (V_jobCategory == null || string.IsNullOrWhiteSpace(V_jobCategory.ToString()))
                    {
                        customer.JobCategory = null;
                    }
                    else
                    {
                        // Proceed with conversion
                        ExcelHelper.TryConvertToInt(V_jobCategory, out int jobCategory, "en-US", "de-DE");
                        customer.JobCategory = jobCategory;
                    }

                    var birthday = MaterialTable.Cells[row, 7]?.Value;
                    customer.Birtday = birthday != null && DateTime.TryParse(birthday.ToString(), out DateTime result) ? result : (DateTime?)null;



                    var V_AgeCategory = MaterialTable.Cells[row, 8].Value;
                    if (V_AgeCategory == null || string.IsNullOrWhiteSpace(V_AgeCategory.ToString()))
                    {
                        customer.AgeCategory = null;
                    }
                    else
                    {
                        // Proceed with conversion
                        ExcelHelper.TryConvertToInt(V_AgeCategory, out int AgeCategory, "en-US", "de-DE");
                        customer.AgeCategory = AgeCategory;
                    }



                    var V_CharacterType = MaterialTable.Cells[row, 9].Value;
                    if (V_CharacterType == null || string.IsNullOrWhiteSpace(V_CharacterType.ToString()))
                    {
                        customer.CharacterType = null;
                    }
                    else
                    {
                        // Proceed with conversion
                        ExcelHelper.TryConvertToInt(V_CharacterType, out int CharacterType, "en-US", "de-DE");
                        customer.CharacterType = CharacterType;
                    }


                    var V_acquaintance = MaterialTable.Cells[row, 10].Value;
                    if (V_acquaintance == null || string.IsNullOrWhiteSpace(V_acquaintance.ToString()))
                    {
                        customer.acquaintance = null;
                    }
                    else
                    {
                        // Proceed with conversion
                        ExcelHelper.TryConvertToInt(V_acquaintance, out int acquaintance, "en-US", "de-DE");
                        customer.acquaintance = acquaintance;
                    }

                    var V_MarketOriented = MaterialTable.Cells[row, 11].Value;
                    if (V_MarketOriented == null || string.IsNullOrWhiteSpace(V_MarketOriented.ToString()))
                    {
                        customer.MarketOriented = null;
                    }
                    else
                    {
                        // Proceed with conversion
                        ExcelHelper.TryConvertToInt(V_MarketOriented, out int MarketOriented, "en-US", "de-DE");
                        customer.MarketOriented = MarketOriented;
                    }




                    var V_Country = MaterialTable.Cells[row, 12].Value;
                    if (V_Country == null || string.IsNullOrWhiteSpace(V_Country.ToString()))
                    {
                        customer.Country = null;
                    }
                    else
                    {
                        // Proceed with conversion
                        ExcelHelper.TryConvertToInt(V_Country, out int Country, "en-US", "de-DE");
                        customer.Country = Country;
                    }

                    var V_Province = MaterialTable.Cells[row, 13].Value;
                    if (V_Province == null || string.IsNullOrWhiteSpace(V_Province.ToString()))
                    {
                        customer.Province = null;
                    }
                    else
                    {
                        // Proceed with conversion
                        ExcelHelper.TryConvertToInt(V_Province, out int Province, "en-US", "de-DE");
                        customer.Province = Province;
                    }

                    var V_city = MaterialTable.Cells[row, 14].Value;
                    if (V_city == null || string.IsNullOrWhiteSpace(V_city.ToString()))
                    {
                        customer.city = null;
                    }
                    else
                    {
                        // Proceed with conversion
                        ExcelHelper.TryConvertToInt(V_city, out int city, "en-US", "de-DE");
                        customer.city = city;
                    }


                    var V_Latitude = MaterialTable.Cells[row, 17].Value;
                    if (V_Latitude == null || string.IsNullOrWhiteSpace(V_Latitude.ToString()))
                    {
                        customer.Latitude = null;
                    }
                    else
                    {
                        // Proceed with conversion
                        ExcelHelper.TryConvertToFloat(V_Latitude, out float Latitude, "en-US", "de-DE");
                        customer.Latitude = Convert.ToDouble(Latitude);
                    }


                    var V_Longitude = MaterialTable.Cells[row, 18].Value;
                    if (V_Longitude == null || string.IsNullOrWhiteSpace(V_Longitude.ToString()))
                    {
                        customer.Longitude = null;
                    }
                    else
                    {
                        // Proceed with conversion
                        ExcelHelper.TryConvertToFloat(V_Longitude, out float Longitude, "en-US", "de-DE");
                        customer.Longitude = Convert.ToDouble(Longitude);
                    }

                    //int powerType;
                    //ExcelHelper.TryConvertToInt(PowersTable.Cells[row, 2].Value, out powerType, "en-US", "de-DE");
                    //power.PowerType = powerType;
                    //// nullable float
                    ////......................................................................................................

                    ////non nullable float
                    ////......................................................................................................
                    //float materialSizeX;
                    //ExcelHelper.TryConvertToFloat(MaterialTable.Cells[row, 6].Value, out materialSizeX, "en-US", "de-DE");
                    //material.MaterialSizeX = materialSizeX;

                    customers.Add(customer);
                }



                return new ExcelTablesResultDto
                {
                    CustomerInfos = customers

                };
            }
        }


    }

    public class ExcelTablesResultDto
    {
        public List<CustomerInfo> CustomerInfos { get; set; }

    }

}
