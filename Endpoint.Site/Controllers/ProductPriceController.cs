//using Endpoint.Site.Models.NestingViewModel.ChannelliumViewModel;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Interfaces.FacadPatterns;
//using Radin.Application.Services.Product.Commands;
//using Radin.Common.Dto;
//using Radin.Common;
//using static Radin.Application.Services.Product.Commands.ChallPrice.ChallPriceService;
//using Radin.Application.Services.Product.Commands.SwediPrice;
//using Endpoint.Site.Models.NestingViewModel.ChannelliumViewModel.Mapping;
//using Radin.Application.Services.Product.Commands.SwediMaxPrice;
//using System.Text.Json;
//using Radin.Application.Services.Product.Commands.PlasticPrice;
//using Endpoint.Site.Models.NestingInterfaceModel;
//using Endpoint.Site.Models.ReportPdf;
//using Sprache;
//using NuGet.Protocol;
//using Radin.Application.Services.Product.Commands.PowerCalculation;
////using Newtonsoft.Json;

//namespace Endpoint.Site.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ProductPriceController : ControllerBase
//    {
//        private readonly IProductPriceFacad _productPriceFacad;

//        private readonly ChannelliumMapper _channelliumMapper;
//        private readonly SwediMapper _swediMapper;
//        private readonly SwediMaxMapper _swediMaxMapper;
//        private readonly PlasticMapper _plasticMapper;
//        private readonly string _pythonApi;

//        private IPriceFeeDataBaseContext _context;
//        private CombinedResult _combinedResult1;
//        private readonly ReportPdfService _reportPdfService;

//        public ProductPriceController(

//           ChannelliumMapper channelliumMapper,
//           SwediMapper SwediMapper,
//           SwediMaxMapper SwediMaxMapper,

//            IProductPriceFacad productPriceFacad,
//            IPriceFeeDataBaseContext context,
//            CombinedResult combinedResult,
//            PlasticMapper plasticMapper,
//            ReportPdfService reportPdfService



//           )
//        {
//            _reportPdfService = reportPdfService;

//            _channelliumMapper = channelliumMapper;
//            _productPriceFacad = productPriceFacad;
//            _context = context;
//            _combinedResult1 = combinedResult;
//            _swediMapper = SwediMapper;
//            _swediMaxMapper = SwediMaxMapper;
//            _plasticMapper = plasticMapper;
//            _pythonApi = Environment.GetEnvironmentVariable("PYTHON_API");

//        }



//        [HttpGet]
//        [Route("ReportPdfTest")]

//        public IActionResult test(ChannelliumViewModel request)
//        {
//            var result = _reportPdfService.Execute(request);



//            return (Ok(result));
//        }




//        [HttpPost]
//        [Route("Price")]
//        public async Task<IActionResult> Price([FromBody] JsonElement jsonData)
//        {
//            var id = jsonData.GetProperty("boardType").GetProperty("id").GetInt32();
//            Console.WriteLine($@"id = {id}");
//            switch (id)
//            {

//                case 6://plastic
//                    var viewModel6 = JsonSerializer.Deserialize<ChannelliumViewModel>(jsonData.GetRawText());

//                    return await Plastic(viewModel6);
                
                
//                case 7://channellium

//                    var viewModel7 = JsonSerializer.Deserialize<ChannelliumViewModel>(jsonData.GetRawText());
//                    Console.WriteLine($@"DATA= {viewModel7.file}");
//                    Console.WriteLine(viewModel7);

//                    //var jsonString = JsonSerializer.Serialize(viewModel1);
//                    //Console.WriteLine($@"String={jsonString}");
//                    //float size=Convert.ToSingle(viewModel1.data.edgesSize.label);
//                    //Console.WriteLine($@"Size={size}");

//                    return await Channellium(viewModel7);


//                case 9://SwediMax
//                    var viewModel9 = JsonSerializer.Deserialize<ChannelliumViewModel>(jsonData.GetRawText());



//                    return await SwediMax(viewModel9);



//                case 10://Swedi
//                    var viewModel10 = JsonSerializer.Deserialize<ChannelliumViewModel>(jsonData.GetRawText());



//                    return await Swedi(viewModel10);

               

//                default:
//                    {
//                        return BadRequest("امکان محاسبه قیمت برای این مشخصات وجود ندارد");
//                    }
//            }
//        }






//        //////////////////////////////////////////////////////////////////////////////////////////////////////////
//        //////////////////////////////////////////////////////////////////////////////////////////////////////////


//        public async Task<IActionResult> Channellium(ChannelliumViewModel model)
//        {
//            Console.WriteLine($"Channellium: {model}");
//            if (model.data == null || model.boardType == null )
//            {
//                return BadRequest("Model properties cannot be null.");
//            }
            
//            var Request = _channelliumMapper.Mapper(model);
//            if (Request == null)
//            {
//                return BadRequest("خطا در داده های ارسال شده");
//            }
//            var secondLayer = "";
//            if (model.data.modelLayerLetters.value.id == 2)
//            {
//                secondLayer = Request.SecondLayerModel;

//            }
//            var State = 1;
//            if (Request.SecondLayerModel == ConstantMaterialName.State2Material && Request.LayerCondition == 2)
//            {
//                State = 2;
//            }
//            float margin = Request.PvcBackLightMargin;
//            var content = _productPriceFacad.ChallPriceService.NestingInputsGet(model.file, secondLayer,margin,State).Data;
//            Console.WriteLine($@"STATE={State}");

//            //var SvgFile = "..\\svgFiles\\iran.svg";
//            //var content = _productPriceFacad.ChallPriceService.NestingInputsGet(SvgFile, secondLayer).Data;


//            HttpResponseMessage response;

//            try
//            {
//                HttpClient client = new();
//                client.Timeout = TimeSpan.FromSeconds(5000);
//                response = await client.PostAsync(_pythonApi, content);
//            }
//            catch (HttpRequestException httpEx)
//            {

//                // Handle the exception or return an appropriate error response
//                return BadRequest($"Request error: {httpEx.Message}");
//            }
//            catch (TaskCanceledException timeoutEx)
//            {
//                // Handle the timeout or return an appropriate error response

//                return BadRequest("خطای طولانی شدن زمان ارتباط");
//            }
//            catch (Exception ex)
//            {
//                // Handle any other exceptions or return an appropriate error response

//                return BadRequest($"An unexpected error occurred: {ex.Message}");
//            }


//            Console.WriteLine("Start");
            

//            if (response.IsSuccessStatusCode)
//            {


               


//                var jsonResponse = await response.Content.ReadAsStringAsync();
//                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonResponse);
//                var Nesting = new RequestNfpInfoDto
//                {

//                    AConsumptionM1 = data["plexiNestingArea"].ToObject<float>(),
//                    AConsumptionM2 = data["secondLayerNestingArea"].ToObject<float>(),
//                    secondLayerRealArea = data["secondLayerRealArea"].ToObject<float>(),
//                    AConsumptionPvc = data["pvcNestingArea"].ToObject<float>(),
//                    ARealPvc = data["pvcRealArea"].ToObject<float>(),
//                    LRealPvc = data["pvcPerimeter"].ToObject<float>(),
//                    BacklightConsumption = data["pvcBacklightArea"].ToObject<float>(),
//                };


            

//                var Result_Aplus = _productPriceFacad.ChallPriceService.Execute(Request, Nesting, ConstantMaterialName.QualityFactor_Aplus);
//                Result_Aplus.Data.QualityDegree = ConstantMaterialName.QualityFactor_Aplus;

//                var Result_A = _productPriceFacad.ChallPriceService.Execute(Request, Nesting, ConstantMaterialName.QualityFactor_A);
//                Result_A.Data.QualityDegree = ConstantMaterialName.QualityFactor_A;

//                var Result_B = _productPriceFacad.ChallPriceService.Execute(Request, Nesting, ConstantMaterialName.QualityFactor_B);
//                Result_B.Data.QualityDegree = ConstantMaterialName.QualityFactor_B;
//                var combinedResult = _combinedResult1.Execute(Result_Aplus.Data, Result_A.Data, Result_B.Data);



//                var supplement = _reportPdfService.Execute(model);
//                supplement.ProjectName = model.ProjectName ?? "------";
//                supplement.type = model.boardType.label;
//                supplement.Images = model.Images ?? new List<string>();
//                supplement.FsmdNumber = Result_Aplus.SupplemantaryData?.Fsmd ?? 0f;
//                supplement.BsmdNumber = Result_Aplus.SupplemantaryData?.Bsmd ?? 0f;
//                supplement.EdgeLength = Nesting.LRealPvc;

//                var supplement1 = JsonSerializer.Deserialize<Output>(JsonSerializer.Serialize(supplement));
//                var supplement2 = JsonSerializer.Deserialize<Output>(JsonSerializer.Serialize(supplement));
//                var supplement3 = JsonSerializer.Deserialize<Output>(JsonSerializer.Serialize(supplement));



//                supplement1.PowerData = Result_Aplus.SupplemantaryData?.PowerList ?? new List<PowerList>();
//                supplement1.QualityFactor=ConstantMaterialName.QualityFactor_Aplus;

//                supplement2.PowerData = Result_A.SupplemantaryData?.PowerList ?? new List<PowerList>();
//                supplement2.QualityFactor=ConstantMaterialName.QualityFactor_A;


//                supplement3.PowerData = Result_B.SupplemantaryData?.PowerList ?? new List<PowerList>();
//                supplement3.QualityFactor=ConstantMaterialName.QualityFactor_B;
//                var SupplementaryCombined=_combinedResult1.Execute(supplement1, supplement2, supplement3);


//                var Result = new ResultDto<Object, Object>()
//                {
//                    Data = combinedResult,
//                    IsSuccess = Result_A.IsSuccess && Result_Aplus.IsSuccess && Result_B.IsSuccess,
                    
//                    Message = $@"A+ = {Result_Aplus.Message}" + "****" +
                    

//                     $@"A = {Result_A.Message}" + "****" +
//                        $@"B = {Result_B.Message}" + "****",
                    
//                    SupplemantaryData= SupplementaryCombined
//                };
//                HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
//                HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "POST, OPTIONS");
//                HttpContext.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");


//                return Ok(Result);
//            }
//            else
//            {
                
//                string resultContent = response.Content.ReadAsStringAsync().Result;
//                var message = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageError>(resultContent);
                
//                return BadRequest(message.error);
//            }

//        }


//        //////////////////////////////////////////////////////////////////////////////////////////////////////////
//        //////////////////////////////////////////////////////////////////////////////////////////////////////////




//        public async Task<IActionResult> Swedi(ChannelliumViewModel model)
//        {
            
//            if (model.data == null || model.boardType == null)
//            {
//                return BadRequest("Model properties cannot be null.");
//            }

//            var Request = _swediMapper.Mapper(model);
//            if (Request == null)
//            {
//                return BadRequest("خطا در داده های ارسال شده");
//            }
//            var secondLayer = "";
//            if (model.data.modelLayerLetters.value.id == 2)
//            {
//                secondLayer = Request.SecondLayerModel;

//            }
//            var State = 1;
//            if (Request.SecondLayerModel == ConstantMaterialName.State2Material & Request.LayerCondition == 2)
//            {
//                State = 2;
//            }
//            float margin = Request.PvcBackLightMargin;
            
//            var content = _productPriceFacad.SwediPriceService.NestingInputsGet(model.file, secondLayer, margin, State).Data;


//            //var SvgFile = "..\\svgFiles\\iran.svg";
//            //var content = _productPriceFacad.ChallPriceService.NestingInputsGet(SvgFile, secondLayer).Data;



//            HttpResponseMessage response;

//            try
//            {
//                HttpClient client = new();
//                client.Timeout = TimeSpan.FromSeconds(5000);
//                response = await client.PostAsync(_pythonApi, content);
//            }
//            catch (HttpRequestException httpEx)
//            {

//                // Handle the exception or return an appropriate error response
//                return BadRequest($"Request error: {httpEx.Message}");
//            }
//            catch (TaskCanceledException timeoutEx)
//            {
//                // Handle the timeout or return an appropriate error response

//                return BadRequest("خطای طولانی شدن زمان ارتباط");
//            }
//            catch (Exception ex)
//            {
//                // Handle any other exceptions or return an appropriate error response

//                return BadRequest($"An unexpected error occurred: {ex.Message}");
//            }




//            if (response.IsSuccessStatusCode)
//            {
//                var jsonResponse = await response.Content.ReadAsStringAsync();
//                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonResponse);
//                var Nesting = new RequestSwediNfpInfoDto
//                {

//                    AConsumptionM1 = data["plexiNestingArea"].ToObject<float>(),
//                    AConsumptionM2 = data["secondLayerNestingArea"].ToObject<float>(),
//                    secondLayerRealArea = data["secondLayerRealArea"].ToObject<float>(),
//                    AConsumptionPvc = data["pvcNestingArea"].ToObject<float>(),
//                    ARealPvc = data["pvcRealArea"].ToObject<float>(),
//                    LRealPvc = data["pvcPerimeter"].ToObject<float>(),
//                    BacklightConsumption = data["pvcBacklightArea"].ToObject<float>(),
//                };

//                var Result_Aplus = _productPriceFacad.SwediPriceService.Execute(Request, Nesting, ConstantMaterialName.QualityFactor_Aplus);
//                Result_Aplus.Data.QualityDegree = ConstantMaterialName.QualityFactor_Aplus;

//                var Result_A = _productPriceFacad.SwediPriceService.Execute(Request, Nesting, ConstantMaterialName.QualityFactor_A);
//                Result_A.Data.QualityDegree = ConstantMaterialName.QualityFactor_A;

//                var Result_B = _productPriceFacad.SwediPriceService.Execute(Request, Nesting, ConstantMaterialName.QualityFactor_B);
//                Result_B.Data.QualityDegree = ConstantMaterialName.QualityFactor_B;
//                var combinedResult = _combinedResult1.Execute(Result_Aplus.Data, Result_A.Data, Result_B.Data);





//                var supplement = _reportPdfService.Execute(model);
//                supplement.ProjectName = model.ProjectName ?? "------";
//                supplement.type = model.boardType.label;
//                supplement.Images = model.Images ?? new List<string>();
//                supplement.FsmdNumber = Result_Aplus.SupplemantaryData?.Fsmd ?? 0f;
//                supplement.BsmdNumber = Result_Aplus.SupplemantaryData?.Bsmd ?? 0f;
//                supplement.EdgeLength = Nesting.LRealPvc;
//                var supplement1 = JsonSerializer.Deserialize<Output>(JsonSerializer.Serialize(supplement));
//                var supplement2 = JsonSerializer.Deserialize<Output>(JsonSerializer.Serialize(supplement));
//                var supplement3 = JsonSerializer.Deserialize<Output>(JsonSerializer.Serialize(supplement));


//                supplement1.PowerData=Result_Aplus.SupplemantaryData?.PowerList;
//                supplement1.QualityFactor=ConstantMaterialName.QualityFactor_Aplus;

//                supplement2.PowerData = Result_A.SupplemantaryData?.PowerList ?? new List<PowerList>();
//                supplement2.QualityFactor=ConstantMaterialName.QualityFactor_A;

//                supplement3.PowerData = Result_B.SupplemantaryData?.PowerList ?? new List<PowerList>();
//                supplement3.QualityFactor=ConstantMaterialName.QualityFactor_B;

//                var SupplementaryCombined = _combinedResult1.Execute(supplement1, supplement2, supplement3);

//                var Result = new ResultDto<Object,Object>()
//                {
//                    Data = combinedResult,
//                    IsSuccess = Result_A.IsSuccess && Result_Aplus.IsSuccess && Result_B.IsSuccess,
//                    Message = $@"A+ = {Result_Aplus.Message}" + "****" +


//                     $@"A = {Result_A.Message}" + "****" +
//                        $@"B = {Result_B.Message}" + "****",
//                    SupplemantaryData= SupplementaryCombined

//                };


//                return Ok(Result);
//            }
//            else
//            {
               
//                string resultContent = response.Content.ReadAsStringAsync().Result;
//                var message = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageError>(resultContent);
//                Console.WriteLine($@"error={message.error}");
                
//                return BadRequest(message.error);
//            }

//        }

//        //////////////////////////////////////////////////////////////////////////////////////////////////////////
//        //////////////////////////////////////////////////////////////////////////////////////////////////////////
//        public async Task<IActionResult> SwediMax(ChannelliumViewModel model)
//        {
//            if (model.data == null || model.boardType == null)
//            {
//                return BadRequest("Model properties cannot be null.");
//            }

//            var Request = _swediMaxMapper.Mapper(model);
//            if (Request == null)
//            {
//                return BadRequest("خطا در داده های ارسال شده");
//            }
//            var secondLayer = "";
//            if (model.data.modelLayerLetters.value.id == 2)
//            {
//                secondLayer = Request.SecondLayerModel;

//            }
//            var State = 1;
//            if (Request.SecondLayerModel == ConstantMaterialName.State2Material & Request.LayerCondition == 2)
//            {
//                State = 2;
//            }
//            float margin = Request.PvcBackLightMargin;
//            var content = _productPriceFacad.SwediMaxPriceService.NestingInputsGet(model.file, secondLayer,margin,State).Data;


//            //var SvgFile = "..\\svgFiles\\iran.svg";
//            //var content = _productPriceFacad.ChallPriceService.NestingInputsGet(SvgFile, secondLayer).Data;



//            HttpResponseMessage response;

//            try
//            {
//                HttpClient client = new();
//                client.Timeout = TimeSpan.FromSeconds(5000);
//                response = await client.PostAsync(_pythonApi, content);
//            }
//            catch (HttpRequestException httpEx)
//            {

//                // Handle the exception or return an appropriate error response
//                return BadRequest($"Request error: {httpEx.Message}");
//            }
//            catch (TaskCanceledException timeoutEx)
//            {
//                // Handle the timeout or return an appropriate error response

//                return BadRequest("خطای طولانی شدن زمان ارتباط");
//            }
//            catch (Exception ex)
//            {
//                // Handle any other exceptions or return an appropriate error response

//                return BadRequest($"An unexpected error occurred: {ex.Message}");
//            }




//            if (response.IsSuccessStatusCode)
//            {
//                var jsonResponse = await response.Content.ReadAsStringAsync();
//                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonResponse);
//                var Nesting = new RequestSwediMaxNfpInfoDto
//                {

//                    AConsumptionM1 = data["plexiNestingArea"].ToObject<float>(),
//                    AConsumptionM2 = data["secondLayerNestingArea"].ToObject<float>(),
//                    secondLayerRealArea = data["secondLayerRealArea"].ToObject<float>(),
//                    AConsumptionPvc = data["pvcNestingArea"].ToObject<float>(),
//                    ARealPvc = data["pvcRealArea"].ToObject<float>(),
//                    LRealPvc = data["pvcPerimeter"].ToObject<float>(),
//                    BacklightConsumption = data["pvcBacklightArea"].ToObject<float>(),
//                };

//                var Result_Aplus = _productPriceFacad.SwediMaxPriceService.Execute(Request, Nesting, ConstantMaterialName.QualityFactor_Aplus);
//                Result_Aplus.Data.QualityDegree = ConstantMaterialName.QualityFactor_Aplus;

//                var Result_A = _productPriceFacad.SwediMaxPriceService.Execute(Request, Nesting, ConstantMaterialName.QualityFactor_A);
//                Result_A.Data.QualityDegree = ConstantMaterialName.QualityFactor_A;

//                var Result_B = _productPriceFacad.SwediMaxPriceService.Execute(Request, Nesting, ConstantMaterialName.QualityFactor_B);
//                Result_B.Data.QualityDegree = ConstantMaterialName.QualityFactor_B;
//                var combinedResult = _combinedResult1.Execute(Result_Aplus.Data, Result_A.Data, Result_B.Data);

//                var supplement = _reportPdfService.Execute(model);
//                supplement.ProjectName = model.ProjectName ?? "------";
//                supplement.type = model.boardType.label;
//                supplement.Images = model.Images ?? new List<string>();
//                supplement.FsmdNumber = Result_Aplus.SupplemantaryData?.Fsmd ?? 0f;
//                supplement.BsmdNumber = Result_Aplus.SupplemantaryData?.Bsmd ?? 0f;
//                supplement.EdgeLength = Nesting.LRealPvc;
//                var supplement1 = JsonSerializer.Deserialize<Output>(JsonSerializer.Serialize(supplement));
//                var supplement2 = JsonSerializer.Deserialize<Output>(JsonSerializer.Serialize(supplement));
//                var supplement3 = JsonSerializer.Deserialize<Output>(JsonSerializer.Serialize(supplement));

//                supplement1.PowerData = Result_Aplus.SupplemantaryData?.PowerList ?? new List<PowerList>();
//                supplement1.QualityFactor=ConstantMaterialName.QualityFactor_Aplus;


//                supplement2.PowerData = Result_A.SupplemantaryData?.PowerList ?? new List<PowerList>();
//                supplement2.QualityFactor=ConstantMaterialName.QualityFactor_A;


//                supplement3.PowerData = Result_B.SupplemantaryData?.PowerList ?? new List<PowerList>();
//                supplement3.QualityFactor=ConstantMaterialName.QualityFactor_B;
//                var SupplementaryCombined = _combinedResult1.Execute(supplement1, supplement2, supplement3);




//                var Result = new ResultDto<Object, Object>()
//                {
//                    Data = combinedResult,
//                    IsSuccess = Result_A.IsSuccess && Result_Aplus.IsSuccess && Result_B.IsSuccess,
//                    Message = $@"A+ = {Result_Aplus.Message}" + "****" +


//                     $@"A = {Result_A.Message}" + "****" +
//                        $@"B = {Result_B.Message}" + "****",
//                    SupplemantaryData= SupplementaryCombined

//                };


//                return Ok(Result);
//            }
//            else
//            {
                
//                string resultContent = response.Content.ReadAsStringAsync().Result;
//                var message = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageError>(resultContent);
                
//                return BadRequest(message.error);
//            }

//        }

//        //////////////////////////////////////////////////////////////////////////////////////////////////////////
//        //////////////////////////////////////////////////////////////////////////////////////////////////////////


//        public async Task<IActionResult> Plastic(ChannelliumViewModel model)
//        {
//            if (model.data == null || model.boardType == null)
//            {
//                return BadRequest("Model properties cannot be null.");
//            }

//            var Request = _plasticMapper.Mapper(model);
//            if (Request == null)
//            {
//                return BadRequest("خطا در داده های ارسال شده");
//            }
//            var secondLayer = "";
//            if (model.data.modelLayerLetters.value.id == 2)
//            {
//                secondLayer = Request.SecondLayerModel;

//            }
//            var State = 1;
//            if (Request.SecondLayerModel == ConstantMaterialName.State2Material & Request.LayerCondition == 2)
//            {
//                State = 2;
//            }
//            float margin = Request.PvcBackLightMargin;
//            var content = _productPriceFacad.PlasticPriceService.NestingInputsGet(model.file, secondLayer,margin,State).Data;


           

//            HttpResponseMessage response;
         
//            try
//            {
//                HttpClient client = new();
//                client.Timeout = TimeSpan.FromSeconds(5000);
//                response = await client.PostAsync(_pythonApi, content);
//            }
//            catch (HttpRequestException httpEx)
//            {
                
//                // Handle the exception or return an appropriate error response
//                return BadRequest($"Request error: {httpEx.Message}");
//            }
//            catch (TaskCanceledException timeoutEx)
//            {
//                // Handle the timeout or return an appropriate error response

//                return BadRequest("خطای طولانی شدن زمان ارتباط");
//            }
//            catch (Exception ex)
//            {
//                // Handle any other exceptions or return an appropriate error response

//                return BadRequest($"An unexpected error occurred: {ex.Message}");
//            }



//            if (response.IsSuccessStatusCode)
//            {
//                var jsonResponse = await response.Content.ReadAsStringAsync();
//                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonResponse);
//                var Nesting = new RequestPlasticNfpInfoDto
//                {

//                    AConsumptionM1 = data["plexiNestingArea"].ToObject<float>(),
//                    AConsumptionM2 = data["secondLayerNestingArea"].ToObject<float>(),
//                    secondLayerRealArea = data["secondLayerRealArea"].ToObject<float>(),
//                    AConsumptionPvc = data["pvcNestingArea"].ToObject<float>(),
//                    ARealPvc = data["pvcRealArea"].ToObject<float>(),
//                    LRealPvc = data["pvcPerimeter"].ToObject<float>(),
//                    BacklightConsumption = data["pvcBacklightArea"].ToObject<float>(),
//                };

//                var Result_Aplus = _productPriceFacad.PlasticPriceService.Execute(Request, Nesting, ConstantMaterialName.QualityFactor_Aplus);
//                Result_Aplus.Data.QualityDegree = ConstantMaterialName.QualityFactor_Aplus;

//                var Result_A = _productPriceFacad.PlasticPriceService.Execute(Request, Nesting, ConstantMaterialName.QualityFactor_A);
//                Result_A.Data.QualityDegree = ConstantMaterialName.QualityFactor_A;

//                var Result_B = _productPriceFacad.PlasticPriceService.Execute(Request, Nesting, ConstantMaterialName.QualityFactor_B);
//                Result_B.Data.QualityDegree = ConstantMaterialName.QualityFactor_B;
//                var combinedResult = _combinedResult1.Execute(Result_Aplus.Data, Result_A.Data, Result_B.Data);


//                var supplement = _reportPdfService.Execute(model);
//                supplement.ProjectName = model.ProjectName ?? "------";
//                supplement.type = model.boardType.label;
//                supplement.Images = model.Images ?? new List<string>();
//                supplement.FsmdNumber = Result_Aplus.SupplemantaryData?.Fsmd ?? 0f;
//                supplement.BsmdNumber = Result_Aplus.SupplemantaryData?.Bsmd ?? 0f;
//                supplement.EdgeLength = Nesting.LRealPvc;
//                var supplement1 = JsonSerializer.Deserialize<Output>(JsonSerializer.Serialize(supplement));
//                var supplement2 = JsonSerializer.Deserialize<Output>(JsonSerializer.Serialize(supplement));
//                var supplement3 = JsonSerializer.Deserialize<Output>(JsonSerializer.Serialize(supplement));

//                supplement1.PowerData = Result_Aplus.SupplemantaryData?.PowerList ?? new List<PowerList>();
//                supplement1.QualityFactor=ConstantMaterialName.QualityFactor_Aplus;

//                supplement2.PowerData = Result_A.SupplemantaryData?.PowerList ?? new List<PowerList>();
//                supplement2.QualityFactor=ConstantMaterialName.QualityFactor_A;


//                supplement3.PowerData = Result_B.SupplemantaryData?.PowerList ?? new List<PowerList>();
//                supplement3.QualityFactor = ConstantMaterialName.QualityFactor_B;
//                Console.WriteLine($@"supplement1 ={supplement1.QualityFactor}");
//                Console.WriteLine($@"supplement2 ={supplement2.QualityFactor}");

//                Console.WriteLine($@"supplement3 ={supplement3.QualityFactor}");

//                var SupplementaryCombined = _combinedResult1.Execute(supplement1, supplement2, supplement3);





//                var Result = new ResultDto<Object,Object>()
//                {
//                    Data = combinedResult,
//                    IsSuccess = Result_A.IsSuccess && Result_Aplus.IsSuccess && Result_B.IsSuccess,
//                    Message = $@"A+ = {Result_Aplus.Message}" + "****" +


//                     $@"A = {Result_A.Message}" + "****" +
//                        $@"B = {Result_B.Message}" + "****",
//                    SupplemantaryData= SupplementaryCombined

//                };


//                return Ok(Result);
//            }
//            else
//            {
                
//                string resultContent = response.Content.ReadAsStringAsync().Result;
//                var message = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageError>(resultContent);
                
//                return BadRequest(message.error);
//            }

//        }
//    }
//}
