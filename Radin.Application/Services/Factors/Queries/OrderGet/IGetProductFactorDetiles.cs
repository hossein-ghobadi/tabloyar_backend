using Microsoft.AspNetCore.Identity;
using Radin.Application.Interfaces.Contexts;
using Radin.Common.Dto;
using Radin.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

using System.Threading.Tasks;
using static Radin.Application.Services.Factors.Queries.OrderGet.GetProductFactorDetiles;

namespace Radin.Application.Services.Factors.Queries.OrderGet
{
    public interface IGetProductFactorDetiles
    {
        ResultDto<string> Execute(ProductFactorDetilesGetRequest request);
        Task<ResultDto<string>> SvgToBase64(string fileAddress, HttpClient request2);

    }
    //var deserializedViewModel = NewtonsoftJson.JsonConvert.DeserializeObject<ChannelliumViewModel>(jsonString);
    public class GetProductFactorDetiles : IGetProductFactorDetiles
    {
        private readonly IDataBaseContext _context;

        public GetProductFactorDetiles(IDataBaseContext context
           )
        {
            _context = context;


        }
        public ResultDto<string> Execute(ProductFactorDetilesGetRequest request)
        {

            var Details = _context.ProductFactors.Where(p => p.FactorID == request.FactorId && p.SubFactorID == request.SubFactorID && p.Id == request.ProductFactorID && p.IsRemoved == false).FirstOrDefault();
            //var deserializedViewModel = NewtonsoftJson.JsonConvert.DeserializeObject<ChannelliumViewModel>(jsonString);
            if (Details != null && !string.IsNullOrEmpty(Details.ProductDetails))
            {

                return new ResultDto<string>
                {
                    Data = Details.ProductDetails,
                    IsSuccess = true,
                    Message = "دریافت موفق"

                };
            }
            else
            {
                return new ResultDto<string>
                {
                    Data = "",
                    IsSuccess = false,
                    Message = "دریافت ناموفق"
                };
            }


        }
        public async Task<ResultDto<string>> SvgToBase64(string fileAddress, HttpClient client)
        {



            string url = $"https://flask-svg.liara.run/upload-svg?file_url={fileAddress}";
            HttpResponseMessage response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode) { return new ResultDto<string> { Message = "Error from Python API", IsSuccess = false }; }
            string responseContent = await response.Content.ReadAsStringAsync();
            // Deserialize the Python API's JSON response
            var pythonApiResponse = JsonSerializer.Deserialize<PythonApiResponse>(responseContent);
            if (pythonApiResponse?.jpg_base64 == null) { return new ResultDto<string> { IsSuccess = false, Message = "Invalid response from Python API: 'jpg_base64' field is missing." }; }



            var ImageString = pythonApiResponse.jpg_base64;

            { return new ResultDto<string> { Data= ImageString,IsSuccess = true, Message = "تبدیل موفق  اس وی جی" }; }



        }
        private class PythonApiResponse
        {
            public string jpg_base64 { get; set; }
        }






    }





    


    public class ProductFactorDetilesGetRequest
    {
        public long FactorId { get; set; }
        public long SubFactorID { get; set; }
        public long ProductFactorID { get; set;}
    }



   
}
