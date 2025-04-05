//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.GoesArea.Queries.StateGetService;
//using Radin.Common.Dto;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.Json;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.GoesArea.Queries.CityGetService
//{
//    public interface ICityGetService
//    {
//        ResultDto<List<GetCityCodeDto>> Execute(GetCityRequest request);
//    }


//    public class CityGetService : ICityGetService
//    {
//        private readonly IDataBaseContext _context;
//        public CityGetService(IDataBaseContext Context)
//        {
//            _context = Context;

//        }

//        public ResultDto<List<GetCityCodeDto>> Execute(GetCityRequest request)
//        {
//            var cities = _context.Cities
//            .Where(s => s.ProvinceId == request.ProvinceId)
//            .AsEnumerable() // Move to in-memory evaluation to allow C# methods
//            .Select(p => new GetCityCodeDto
//            {
//                Id = p.Id,
//                label = p.city,

//                // Use the static method to extract and swap coordinates
//                Coordinates = GetCoordinates(p.point)
//            })
//            .ToList();

//            Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>{cities.Count}");
//            if (cities.Count == 0)
//            {
//                return new ResultDto<List<GetCityCodeDto>>
//                {
//                    Data = null,
//                    IsSuccess =false,
//                    Message = "شهری برای استان مورد نظر وجود ندارد",

//                };
//            }
//            //int rowsCount = 0;
//            else
//            return new ResultDto<List<GetCityCodeDto>>
//            {
//                Data = cities,
//                IsSuccess = true,
//                Message = "",

//            };

//        }

//        public static List<double> GetCoordinates(string pointJson)
//        {
//            if (string.IsNullOrEmpty(pointJson))
//                return new List<double>();

//            try
//            {
//                // Fix JSON formatting:
//                // Step 1: Remove wrapping quotes if present
//                if (pointJson.StartsWith("\"") && pointJson.EndsWith("\""))
//                {
//                    pointJson = pointJson.Substring(1, pointJson.Length - 2); // Remove outer quotes
//                }

//                // Step 2: Replace single quotes and fix escaping issues
//                pointJson = pointJson.Replace("'", "\""); // Replace single quotes with double quotes
//                pointJson = pointJson.Replace("\"\"", "\""); // Fix double quotes to single

//                // Parse JSON
//                using JsonDocument doc = JsonDocument.Parse(pointJson);
//                var coordinatesElement = doc.RootElement.GetProperty("coordinates");

//                // Extract coordinates
//                var coordinates = coordinatesElement.EnumerateArray().Select(c => c.GetDouble()).ToList();

//                // Swap latitude and longitude
//                if (coordinates.Count == 2)
//                {
//                    return new List<double> { coordinates[1], coordinates[0] }; // Swap lat, lon
//                }

//                return coordinates; // Return as is if invalid
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error parsing coordinates: {ex.Message}");
//                return new List<double>(); // Fallback in case of errors
//            }
//        }

//    }


//    public class GetCityRequest
//    {
//        public int ProvinceId { get; set; }

//    }

//    public class GetCityCodeDto
//    {
//        public int Id { get; set; }
//        public string label { get; set; }
//        public List<double> Coordinates { get; set; }


//    }
    

//}
