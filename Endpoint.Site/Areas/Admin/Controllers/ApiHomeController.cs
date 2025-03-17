using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Radin.Application.Services.Contents.Queries.HomePageContentGet;
using Radin.Application.Services.HomePage.Commands.HomePageSliderEdit;
using Radin.Application.Services.HomePage.Commands.HomePageSliderRemove;
using Radin.Application.Services.HomePage.Commands.HomePageSliderSet;
using Radin.Application.Services.HomePage.Queries.HomePageSlider;
using Radin.Common.Dto;
using Sprache;
using System.Text.Json;
using System;
using System.IO;
using static Radin.Application.Services.HomePage.Commands.HomePageSliderRemove.HomePageSliderRemoveService;
using System.Buffers.Text;

namespace Endpoint.Site.Areas.Admin.Controllers
{
    [Route("Admin/api/[controller]")]
    [ApiController]
    public class ApiHomeController : ControllerBase
    {

        private readonly IHomePageSliderGet _homePageSliderGet;
        private readonly IHomePageSliderEditService _homePageSliderEditService;
        private readonly IHomePageSliderRemoveService _homePageSliderRemoveService;
        private readonly IHomePageSliderSetService _homePageSliderSetService;
        private static readonly HttpClient client = new HttpClient();

        public ApiHomeController(
            IHomePageSliderGet homePageSliderGet,
            IHomePageSliderEditService homePageSliderEditService,
            IHomePageSliderSetService homePageSliderSetService,
            IHomePageSliderRemoveService homePageSliderRemoveService


         )
        {
            _homePageSliderGet = homePageSliderGet;
            _homePageSliderEditService = homePageSliderEditService;
            _homePageSliderRemoveService = homePageSliderRemoveService;
            _homePageSliderSetService = homePageSliderSetService;


        }
        [HttpGet("HomeSliderList")]
        [Authorize(Policy = "HomeSliderList")]
        public IActionResult HomeSliderList(int? id)
        {
            
            if (id == null)
            {
                var SliderData = _homePageSliderGet.AdminPageGet();
                return Ok(SliderData);
            }
            else if (id.HasValue) {
                var SliderData = _homePageSliderGet.SingleSliderGet(id);
                return Ok(SliderData);
            }
            else { return NotFound(); }

        }


     
        [HttpPost("HomeSliderEdit")]
        [Authorize(Policy = "HomeSliderEdit")]
        public async Task<IActionResult> HomeSliderEdit(RequestHomeSliderEditDto request)
        {


            var Base64 = "";
            try
            {
                //string url = $"https://flask-svg.liara.run/upload-svg?file_url={request.Image}";

                //HttpResponseMessage response = await client.GetAsync(url);
                //if (response.IsSuccessStatusCode)
                //{
                //    string responseContent = await response.Content.ReadAsStringAsync();
                //    var pythonApiResponse = JsonSerializer.Deserialize<PythonApiResponse>(responseContent);
                //    if (pythonApiResponse?.jpg_base64 != null) { Base64 = pythonApiResponse.jpg_base64; }
                //}
                using (HttpClient client = new HttpClient())
                {
                    // Download the file from the URL
                    byte[] fileBytes = await client.GetByteArrayAsync(request.Image);

                    // Convert the byte array to a Base64 string
                    Base64 = Convert.ToBase64String(fileBytes);
                }

                var sliderData = _homePageSliderEditService.Execute(new RequestHomeSliderEditDto
                {
                    Id = request.Id,
                    Title = request.Title,
                    Image = request.Image,
                    Description = request.Description,
                    Sorting = request.Sorting,
                    base64 = Base64
                });

                if (sliderData.IsSuccess)
                {
                    return Ok(sliderData);
                }
                else
                {
                    return BadRequest(sliderData);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }

        [HttpPost("HomeSliderRemove")]
        [Authorize(Policy = "HomeSliderRemove")]
        public IActionResult HomeSliderRemove(RequestHomeSliderRemoveDto request)
        {
            var SliderData = _homePageSliderRemoveService.Execute(new RequestHomeSliderRemoveDto
            {
                id=request.id
            });
            if (SliderData.IsSuccess == true)
            {
                return Ok(SliderData);
            }
            else { return BadRequest(SliderData); }

        }


        [HttpPost("HomeSliderSet")]
        [Authorize(Policy = "HomeSliderSet")]
        public async Task<IActionResult> HomeSliderSet(RequestHomeSliderSetDto request)
        {
            var Base64 = "";
            try
            {
                //string url = $"https://flask-svg.liara.run/upload-svg?file_url={request.Image}";
                //HttpResponseMessage response = await client.GetAsync(url);
                //if (response.IsSuccessStatusCode) { 
                //    string responseContent = await response.Content.ReadAsStringAsync();
                //    var pythonApiResponse = JsonSerializer.Deserialize<PythonApiResponse>(responseContent);
                //    if (pythonApiResponse?.jpg_base64 != null) { Base64 = pythonApiResponse.jpg_base64; }
                //}
                // Deserialize the Python API's JSON response
                using (HttpClient client = new HttpClient())
                {
                    // Download the file from the URL
                    byte[] fileBytes = await client.GetByteArrayAsync(request.Image);

                    // Convert the byte array to a Base64 string
                    Base64 = Convert.ToBase64String(fileBytes);
                }
                var SliderData = _homePageSliderSetService.Execute(new RequestHomeSliderSetDto
                {
                    Title = request.Title,
                    Image = request.Image,
                    Description = request.Description,
                    Sorting = request.Sorting,
                    base64 = Base64
                });
                if (SliderData.IsSuccess == true)
                {
                    return Ok(SliderData);

                }
                else
                {
                    return BadRequest(SliderData);

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }

        }
        private class PythonApiResponse
        {
            public string jpg_base64 { get; set; }
        }



    }
}
