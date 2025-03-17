using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Radin.Application.Services.Contents.Commands.ContentCategoryEdit;
using Radin.Application.Services.Contents.Commands.ContentCategoryRemove;
using Radin.Application.Services.Contents.Commands.ContentCategorySet;
using Radin.Application.Services.Contents.Commands.ContentEdit;
using Radin.Application.Services.Contents.Commands.ContentRemove;
using Radin.Application.Services.Contents.Commands.ContentSet;
using Radin.Application.Services.Contents.Queries.CategoryGet;
using Radin.Application.Services.Contents.Queries.ContentCategoryGet;
using Radin.Application.Services.Contents.Queries.ContentGet;
using Radin.Application.Services.ProductItems.Commands.EdgeSizeSet;
using Radin.Application.Services.ProductItems.Queries.TablesGet.EdgeSizeGet;
using Radin.Application.Services.ProductItems.Queries.TitleGet;
using Radin.Common.Dto;
using System.Text.Json;
using static Radin.Application.Services.Product.Commands.ChallPrice.ChallPriceService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Endpoint.Site.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductItemsController : ControllerBase
    {



        private readonly IEdgeSizeSetService _edgeSizeSetService;
        private readonly IEdgeSizeGetService _edgeSizeGetService;
        private readonly IEdgeSizeSetService _edgeSizeEditService;
        private readonly IEdgeSizeGetService _edgeSizeRemoveService;
        private readonly ITitleGetService _titleGetService;



        public ProductItemsController(
        IEdgeSizeSetService edgeSizeSetService,
        IEdgeSizeGetService edgeSizeGetService,
        IEdgeSizeSetService edgeSizeEditService,
        IEdgeSizeGetService edgeSizeRemoveService,
        ITitleGetService titleGetService




        )

        {
            _edgeSizeSetService = edgeSizeSetService;
            _edgeSizeGetService = edgeSizeGetService;
            _edgeSizeEditService = edgeSizeEditService;
            _edgeSizeRemoveService = edgeSizeRemoveService;
            _titleGetService = titleGetService;

            //_logger = logger;
        }

        [HttpGet]
        [Route("GetTitle")]

        public IActionResult GetTitle()
        {
            return Ok(_titleGetService.ExistTitles());
        }
        [HttpGet]
        [Route("GET")]

        public IActionResult GetEdgeSize(string type)
        {
            return Ok(_edgeSizeGetService.Execute(type));
        }
        // GET: api/<ProductItemsController>




        [HttpPost]
        [Route("POST")]
        public IActionResult SetEdgeSize(RequestEdgeSizeSetDto request)
        {
            var result = _edgeSizeSetService.Execute(new RequestEdgeSizeSetDto
            {
                Title = request.Title,
                EdgeSize = request.EdgeSize,


            });
            return Ok(result);
        }

        [HttpPost]
        [Route("Test")]

        public IActionResult Index([FromBody] JObject jsonData)
        {
            //dynamic Info = jsonData;
            ////float LRealPvc, float ARealPvc, float AConsumptionPvc, float AConsumptionM1
            ////    , RequestNfpInfoDto request2
            //var Result1 = (new Test
            //{
            //    Title = Info.boardType.label,
            //    EdgeSize = Info.data.edgesSize.label,
            //    //EdgeColor = request1.EdgeColor,
            //    //EdgeModel = request1.EdgeModel,
            //    PvcCheckPoint = Info.data.needPVC.value,
            //    FirstLayerColor = Info.data.edgesSize.label,
            //    SecondLayerColor = Info.data.edgesSize.label,

            //});
            var boardTypeLabel = jsonData["boardType"]?["label"]?.Value<string>();

            // Assuming the 'Test' class has a property named 'Title'
            var result = new Test
            {
                Title = boardTypeLabel,
                // Initialize other properties of 'Test' as needed
            };



            return Ok(result);
        }
        [HttpPost]
        [Route("Echo")]
        public IActionResult Echo([FromBody] JsonElement jsonData)
        {
            // System.Text.Json.JsonElement does not convert directly to an action result,
            // so we serialize it back to JSON string.
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(jsonData, options);

            // You can return the string directly with ContentResult to control the content-type
            return Content(jsonString, "application/json");
        }
        [HttpPost]
        [Route("EchoRawJson")]
        public async Task<IActionResult> EchoRawJson()
        {
            using var reader = new StreamReader(Request.Body);
            var rawJson = await reader.ReadToEndAsync();

            // Optional: Parse the raw JSON string to a JSON object for manipulation
            // Using Newtonsoft.Json
            var jsonObject = JObject.Parse(rawJson);

            // Or using System.Text.Json
            // var jsonObject = JsonDocument.Parse(rawJson);

            // Directly return the raw JSON string
            // Note: To return as application/json, wrap the string in an OkObjectResult
            return Ok(rawJson);
        }

        [HttpPost]
        [Route("ProcessJson")]
        public async Task<IActionResult> ProcessJson()
        {
            using var reader = new StreamReader(Request.Body);
            var rawJson = await reader.ReadToEndAsync();

            // Parse the raw JSON string to JObject
            var jsonData = JObject.Parse(rawJson);

            // Access properties dynamically
            var boardTypeLabel = jsonData["boardType"]?["label"]?.Value<string>();

            // Return just the boardTypeLabel
            if (boardTypeLabel == null)
            {
                // If boardTypeLabel is not found, return a BadRequest or a default response
                return BadRequest("boardTypeLabel not found.");
            }

            // If you just want to return the text without any JSON formatting:
            return Content(boardTypeLabel, "text/plain");
        }

        [HttpPost]
        [Route("ProcessJson1")]
        public IActionResult ProcessJson1()
        {
            // Synchronously read the request body to a string
            var rawJson = new StreamReader(Request.Body).ReadToEnd();

            // Parse the raw JSON string to JObject (for Newtonsoft.Json)
            var jsonData = JObject.Parse(rawJson);

            // Access properties dynamically and safely
            var boardTypeLabel = jsonData["boardType"]?["label"]?.Value<string>();

            // Check if the boardTypeLabel was successfully extracted
            if (boardTypeLabel == null)
            {
                // Respond with BadRequest or another appropriate status if the expected data is missing
                return BadRequest("boardTypeLabel not found.");
            }

            // Return just the boardTypeLabel as plain text
            return Content(boardTypeLabel, "text/plain");

            // Or, if you prefer to return it as a JSON object:
            // return Ok(new { boardTypeLabel = boardTypeLabel });
        }
    }
}
