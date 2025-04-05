//using Endpoint.Site.Models.NestingInterfaceModel;
//using Endpoint.Site.Models.NestingViewModel.ChannelliumViewModel.Mapping;
//using Endpoint.Site.Models.NestingViewModel.ChannelliumViewModel;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Interfaces.FacadPatterns;
//using Radin.Application.Services.Product.Commands.PlasticPrice;
//using Radin.Application.Services.Product.Commands.SwediMaxPrice;
//using Radin.Application.Services.Product.Commands.SwediPrice;
//using Radin.Application.Services.Product.Commands;
//using Radin.Common.Dto;
//using Radin.Common;
//using static Radin.Application.Services.Product.Commands.ChallPrice.ChallPriceService;
//using System.Text.Json;
//using CsvHelper;
//using Microsoft.EntityFrameworkCore;
//using Radin.Domain.Entities.Factors;
//using System.Xml.Linq;
//using Sprache;
//using Persistence.Contexts;

//using NewtonsoftJson = Newtonsoft.Json;
//using SystemTextJson = System.Text.Json;
//using Radin.Application.Services.Factors.Commands.RecordProduct;
//using Radin.Application.Services.Factors.Commands.Accessory.AccessorySet;
//using Microsoft.AspNetCore.Authorization;
//using Radin.Application.Services.Factors.Commands.ProductPriceDetailSet;
//using static Radin.Application.Services.Factors.Commands.ProductPriceDetailSet.ProductPriceDetailSetService;

//namespace Endpoint.Site.Areas.Proxy.Controllers
//{
//    [Authorize(Roles = "PROXY,PROXYSELLER")]

//    [Route("Proxy/api/[controller]")]
//    [ApiController]
//    public class ProxyPriceController : ControllerBase
//    {


//        private readonly IProductPriceFacad _productPriceFacad;

//        private readonly ChannelliumMapper _channelliumMapper;
//        private readonly SwediMapper _swediMapper;
//        private readonly SwediMaxMapper _swediMaxMapper;
//        private readonly PlasticMapper _plasticMapper;
//        private readonly string _pythonApi;
//        private readonly IDataBaseContext _dataBaseContext;
//        private readonly IPriceFeeDataBaseContext _context;
//        private readonly IRecordProductService _recordProductService;
//        private readonly IProductPriceDetailSetService _productPriceDetailSetService;
//        private readonly CombinedResult _combinedResult1;
//        public ProxyPriceController(

//           ChannelliumMapper channelliumMapper,
//           SwediMapper SwediMapper,
//           SwediMaxMapper SwediMaxMapper,

//            IProductPriceFacad productPriceFacad,
//            IPriceFeeDataBaseContext context,
//            CombinedResult combinedResult,
//            PlasticMapper plasticMapper,
//            IDataBaseContext dataBaseContext,
//            IRecordProductService recordProductService,
//            IProductPriceDetailSetService productPriceDetailSetService

//           )
//        {

//            _channelliumMapper = channelliumMapper;
//            _productPriceFacad = productPriceFacad;
//            _context = context;
//            _combinedResult1 = combinedResult;
//            _swediMapper = SwediMapper;
//            _swediMaxMapper = SwediMaxMapper;
//            _plasticMapper = plasticMapper;
//            _pythonApi = Environment.GetEnvironmentVariable("PYTHON_API");
//            _dataBaseContext = dataBaseContext;
//            _recordProductService = recordProductService;
//            _productPriceDetailSetService = productPriceDetailSetService;
//        }

//        static float ConvertThePrice(float number)
//        {
//            if (number < 1000)
//                return 0f; // If number is less than 1000, return 0

//            return (float)(Math.Floor(number / 1000) * 1000); // Remove last 3 digits by flooring
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
//            if (model.data == null || model.boardType == null)
//            {
//                return BadRequest("Model properties cannot be null.");
//            }

//            var Request = _channelliumMapper.Mapper(model);
//            if (Request == null)
//            {
//                return BadRequest("خطا در داده های ارسال شده");
//            }
//            var secondLayer = model.data.modelLayerLetters.value.id == 2 ? Request.SecondLayerModel : "";
//            var State = (Request.SecondLayerModel == ConstantMaterialName.State2Material && Request.LayerCondition == 2) ? 2 : 1;

//            float margin = Request.PvcBackLightMargin;
//            var content = _productPriceFacad.ChallPriceService.NestingInputsGet(model.file, secondLayer, margin, State).Data;


//            HttpResponseMessage response;
//            try
//            {
//                HttpClient client = new();
//                client.Timeout = TimeSpan.FromSeconds(5000);
//                response = await client.PostAsync(_pythonApi, content);
//            }
//            catch (HttpRequestException httpEx) { return BadRequest($"Request error: {httpEx.Message}"); }
//            catch (TaskCanceledException timeoutEx) { return BadRequest("خطای طولانی شدن زمان ارتباط"); }
//            catch (Exception ex) { return BadRequest($"An unexpected error occurred: {ex.Message}"); }



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

//                //var FactorDetails = NewtonsoftJson.JsonConvert.SerializeObject(model);
//                //var deserializedViewModel = NewtonsoftJson.JsonConvert.DeserializeObject<ChannelliumViewModel>(jsonString);
               
//                var CalculationResult = _productPriceFacad.ChallPriceService.AllQfCalculation(Request, Nesting);
 
//                var settings = new Newtonsoft.Json.JsonSerializerSettings
//                {
//                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
//                };
//                if(!CalculationResult.IsSuccess)
//                {
//                    return BadRequest("یکی از محاسبات ناموفق بود");
//                }
//                if(model.QualityFactor== "A%2B")
//                {
//                    model.QualityFactor = ConstantMaterialName.QualityFactor_Aplus;
//                }
//                if (model.QualityFactor == "A%2B%2B")
//                {
//                    model.QualityFactor = ConstantMaterialName.QualityFactor_A2plus;

//                }
               



//                var Result = _recordProductService.HandleRecording(new RecordRequest
//                {
//                    factorId = model.FactorId == null || model.FactorId < 0 ? 0L : model.FactorId.Value,
//                    productId = model.ProductId == null || model.ProductId < 0 ? 0L : model.ProductId.Value,
//                    subFactorId = model.SubfactorId == null || model.SubfactorId < 0 ? 0L : model.SubfactorId.Value,
//                    priceIsSuccess = CalculationResult.IsSuccess,
//                    productCost = new QfPrice {
//                        Price_A2plus = ConvertThePrice(CalculationResult.Data.Result_Aplus.ProductCost * 2 * ConstantMaterialName.A2pluseIncrese),
//                        Price_Aplus = ConvertThePrice(CalculationResult.Data.Result_Aplus.ProductCost * 2),
//                        Price_A = ConvertThePrice(CalculationResult.Data.Result_A.ProductCost * 2),
//                        Price_B = ConvertThePrice(CalculationResult.Data.Result_B.ProductCost * 2),
//                    },
//                    QualityFactor=model.QualityFactor ?? ConstantMaterialName.QualityFactor_Aplus,
//                    description = model.description,
//                    productName = model.boardType.label,
//                    ProductDetails = NewtonsoftJson.JsonConvert.SerializeObject(model, settings),
//                    priceMessage = CalculationResult.Message,
//                    NestingResult= NewtonsoftJson.JsonConvert.SerializeObject(Nesting),
//                });
//                Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>PowerList= {CalculationResult.Data.Result_A.powerList}");
//                if (Result.Result.IsSuccess)
//                {
//                    await Task.Delay(50);
//                    var Request_A = _productPriceDetailSetService.PriceRequestMapp(NewtonsoftJson.JsonConvert.SerializeObject(CalculationResult.Data.Result_A));
//                    var PriceDetailResult_A = _productPriceDetailSetService.PriceDetailSet(Request_A.Result, Result.Result.SupplemantaryData, ConstantMaterialName.QualityFactor_A);
//                    var Request_APlus = _productPriceDetailSetService.PriceRequestMapp(NewtonsoftJson.JsonConvert.SerializeObject(CalculationResult.Data.Result_Aplus));
//                    var PriceDetailResult_APlus = _productPriceDetailSetService.PriceDetailSet(Request_APlus.Result, Result.Result.SupplemantaryData, ConstantMaterialName.QualityFactor_Aplus);
//                    var Request_B = _productPriceDetailSetService.PriceRequestMapp(NewtonsoftJson.JsonConvert.SerializeObject(CalculationResult.Data.Result_B));
//                    var PriceDetailResult_B = _productPriceDetailSetService.PriceDetailSet(Request_B.Result, Result.Result.SupplemantaryData, ConstantMaterialName.QualityFactor_B);
//                    if (PriceDetailResult_B.Result.IsSuccess&& PriceDetailResult_APlus.Result.IsSuccess&& PriceDetailResult_A.Result.IsSuccess)
//                    {

//                        return Ok(Result.Result);
//                    }

//                    return BadRequest("خطا در ثبت ریز قیمت فاکتور"); 


//                }


//                return BadRequest(Result.Result.Message); 
//                //return Result.Result.IsSuccess ? Ok(Result.Result) : BadRequest(Result.Result.Message);



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
//            var secondLayer = model.data.modelLayerLetters.value.id == 2 ? Request.SecondLayerModel : "";
//            var State = (Request.SecondLayerModel == ConstantMaterialName.State2Material && Request.LayerCondition == 2) ? 2 : 1;
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
//            catch (HttpRequestException httpEx) { return BadRequest($"Request error: {httpEx.Message}"); }
//            catch (TaskCanceledException timeoutEx) { return BadRequest("خطای طولانی شدن زمان ارتباط"); }
//            catch (Exception ex) { return BadRequest($"An unexpected error occurred: {ex.Message}"); }





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

//                var CalculationResult = _productPriceFacad.SwediPriceService.AllQfCalculation(Request, Nesting);


//                var settings = new Newtonsoft.Json.JsonSerializerSettings
//                {
//                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
//                };
//                if (!CalculationResult.IsSuccess)
//                {
//                    return BadRequest("یکی از محاسبات ناموفق بود");
//                }
//                if (model.QualityFactor == "A%2B")
//                {
//                    model.QualityFactor = ConstantMaterialName.QualityFactor_Aplus;
//                }
//                if (model.QualityFactor == "A%2B%2B")
//                {
//                    model.QualityFactor = ConstantMaterialName.QualityFactor_A2plus;
//                }
//                var Result = _recordProductService.HandleRecording(new RecordRequest
//                {
//                    factorId = model.FactorId == null || model.FactorId < 0 ? 0L : model.FactorId.Value,
//                    productId = model.ProductId == null || model.ProductId < 0 ? 0L : model.ProductId.Value,
//                    subFactorId = model.SubfactorId == null || model.SubfactorId < 0 ? 0L : model.SubfactorId.Value,
//                    priceIsSuccess = CalculationResult.IsSuccess,
//                    productCost = new QfPrice
//                    {
//                        Price_A2plus = ConvertThePrice(CalculationResult.Data.Result_Aplus.ProductCost * 2 * ConstantMaterialName.A2pluseIncrese),
//                        Price_Aplus = ConvertThePrice(CalculationResult.Data.Result_Aplus.ProductCost * 2),
//                        Price_A = ConvertThePrice(CalculationResult.Data.Result_A.ProductCost * 2),
//                        Price_B = ConvertThePrice(CalculationResult.Data.Result_B.ProductCost * 2),
//                    },
//                    QualityFactor = model.QualityFactor ?? ConstantMaterialName.QualityFactor_Aplus,
//                    description = model.description,
//                    productName = model.boardType.label,
//                    ProductDetails = NewtonsoftJson.JsonConvert.SerializeObject(model, settings),
//                    priceMessage = CalculationResult.Message,
//                    NestingResult = NewtonsoftJson.JsonConvert.SerializeObject(Nesting),
//                });
//                if (Result.Result.IsSuccess)
//                {
//                    await Task.Delay(50);
//                    var Request_A = _productPriceDetailSetService.PriceRequestMapp(NewtonsoftJson.JsonConvert.SerializeObject(CalculationResult.Data.Result_A, settings));
//                    var PriceDetailResult_A = _productPriceDetailSetService.PriceDetailSet(Request_A.Result, Result.Result.SupplemantaryData, ConstantMaterialName.QualityFactor_A);
//                    var Request_APlus = _productPriceDetailSetService.PriceRequestMapp(NewtonsoftJson.JsonConvert.SerializeObject(CalculationResult.Data.Result_Aplus, settings));
//                    var PriceDetailResult_APlus = _productPriceDetailSetService.PriceDetailSet(Request_APlus.Result, Result.Result.SupplemantaryData, ConstantMaterialName.QualityFactor_Aplus);
//                    var Request_B = _productPriceDetailSetService.PriceRequestMapp(NewtonsoftJson.JsonConvert.SerializeObject(CalculationResult.Data.Result_B, settings));
//                    var PriceDetailResult_B = _productPriceDetailSetService.PriceDetailSet(Request_B.Result, Result.Result.SupplemantaryData, ConstantMaterialName.QualityFactor_B);
//                    if (PriceDetailResult_B.Result.IsSuccess && PriceDetailResult_APlus.Result.IsSuccess && PriceDetailResult_A.Result.IsSuccess)
//                    {

//                        return Ok(Result.Result);
//                    }

//                    return BadRequest("خطا در ثبت ریز قیمت فاکتور");


//                }


//                return BadRequest(Result.Result.Message);


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
//            var secondLayer = model.data.modelLayerLetters.value.id == 2 ? Request.SecondLayerModel : "";
//            var State = (Request.SecondLayerModel == ConstantMaterialName.State2Material && Request.LayerCondition == 2) ? 2 : 1;
//            float margin = Request.PvcBackLightMargin;
//            var content = _productPriceFacad.SwediMaxPriceService.NestingInputsGet(model.file, secondLayer, margin, State).Data;


//            //var SvgFile = "..\\svgFiles\\iran.svg";
//            //var content = _productPriceFacad.ChallPriceService.NestingInputsGet(SvgFile, secondLayer).Data;


//            HttpResponseMessage response;
//            try
//            {
//                HttpClient client = new();
//                client.Timeout = TimeSpan.FromSeconds(5000);
//                response = await client.PostAsync(_pythonApi, content);
//            }
//            catch (HttpRequestException httpEx) { return BadRequest($"Request error: {httpEx.Message}"); }
//            catch (TaskCanceledException timeoutEx) { return BadRequest("خطای طولانی شدن زمان ارتباط"); }
//            catch (Exception ex) { return BadRequest($"An unexpected error occurred: {ex.Message}"); }






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

//                var CalculationResult = _productPriceFacad.SwediMaxPriceService.AllQfCalculation(Request, Nesting);

//                var settings = new Newtonsoft.Json.JsonSerializerSettings
//                {
//                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
//                };
//                if (!CalculationResult.IsSuccess)
//                {
//                    return BadRequest("یکی از محاسبات ناموفق بود");
//                }
//                if (model.QualityFactor == "A%2B")
//                {
//                    model.QualityFactor = ConstantMaterialName.QualityFactor_Aplus;
                    
//                }
//                if (model.QualityFactor == "A%2B%2B")
//                {
//                    model.QualityFactor = ConstantMaterialName.QualityFactor_A2plus;
//                }
               


//                var Result = _recordProductService.HandleRecording(new RecordRequest
//                {
//                    factorId = model.FactorId == null || model.FactorId < 0 ? 0L : model.FactorId.Value,
//                    productId = model.ProductId == null || model.ProductId < 0 ? 0L : model.ProductId.Value,
//                    subFactorId = model.SubfactorId == null || model.SubfactorId < 0 ? 0L : model.SubfactorId.Value,
//                    priceIsSuccess = CalculationResult.IsSuccess,
//                    productCost = new QfPrice
//                    {
//                        Price_A2plus = ConvertThePrice(CalculationResult.Data.Result_Aplus.ProductCost * 2 * ConstantMaterialName.A2pluseIncrese),
//                        Price_Aplus = ConvertThePrice(CalculationResult.Data.Result_Aplus.ProductCost * 2),
//                        Price_A = ConvertThePrice(CalculationResult.Data.Result_A.ProductCost * 2),
//                        Price_B = ConvertThePrice(CalculationResult.Data.Result_B.ProductCost * 2),
//                    },
//                    QualityFactor = model.QualityFactor ?? ConstantMaterialName.QualityFactor_Aplus,
//                    description = model.description,
//                    productName = model.boardType.label,
//                    ProductDetails = NewtonsoftJson.JsonConvert.SerializeObject(model, settings),
//                    priceMessage = CalculationResult.Message,
//                    NestingResult = NewtonsoftJson.JsonConvert.SerializeObject(Nesting),
//                });

//                if (Result.Result.IsSuccess)
//                {
//                    await Task.Delay(50);
//                    var Request_A = _productPriceDetailSetService.PriceRequestMapp(NewtonsoftJson.JsonConvert.SerializeObject(CalculationResult.Data.Result_A, settings));
//                    var PriceDetailResult_A = _productPriceDetailSetService.PriceDetailSet(Request_A.Result, Result.Result.SupplemantaryData, ConstantMaterialName.QualityFactor_A);
//                    var Request_APlus = _productPriceDetailSetService.PriceRequestMapp(NewtonsoftJson.JsonConvert.SerializeObject(CalculationResult.Data.Result_Aplus, settings));
//                    var PriceDetailResult_APlus = _productPriceDetailSetService.PriceDetailSet(Request_APlus.Result, Result.Result.SupplemantaryData, ConstantMaterialName.QualityFactor_Aplus);
//                    var Request_B = _productPriceDetailSetService.PriceRequestMapp(NewtonsoftJson.JsonConvert.SerializeObject(CalculationResult.Data.Result_B, settings));
//                    var PriceDetailResult_B = _productPriceDetailSetService.PriceDetailSet(Request_B.Result, Result.Result.SupplemantaryData, ConstantMaterialName.QualityFactor_B);
//                    if (PriceDetailResult_B.Result.IsSuccess && PriceDetailResult_APlus.Result.IsSuccess && PriceDetailResult_A.Result.IsSuccess)
//                    {

//                        return Ok(Result.Result);
//                    }

//                    return BadRequest("خطا در ثبت ریز قیمت فاکتور");


//                }


//                return BadRequest(Result.Result.Message);


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
//            var secondLayer = model.data.modelLayerLetters.value.id == 2 ? Request.SecondLayerModel : "";
//            var State = (Request.SecondLayerModel == ConstantMaterialName.State2Material && Request.LayerCondition == 2) ? 2 : 1;
//            float margin = Request.PvcBackLightMargin;
//            var content = _productPriceFacad.PlasticPriceService.NestingInputsGet(model.file, secondLayer, margin, State).Data;




//            HttpResponseMessage response;
//            try
//            {
//                HttpClient client = new();
//                client.Timeout = TimeSpan.FromSeconds(5000);
//                response = await client.PostAsync(_pythonApi, content);
//            }
//            catch (HttpRequestException httpEx) { return BadRequest($"Request error: {httpEx.Message}"); }
//            catch (TaskCanceledException timeoutEx) { return BadRequest("خطای طولانی شدن زمان ارتباط"); }
//            catch (Exception ex) { return BadRequest($"An unexpected error occurred: {ex.Message}"); }




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
//                var CalculationResult = _productPriceFacad.PlasticPriceService.AllQfCalculation(Request, Nesting);


//                var settings = new Newtonsoft.Json.JsonSerializerSettings// تنظیمات حذف مقادیر نال از درون آبجکت
//                {
//                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
//                };
//                if (!CalculationResult.IsSuccess)
//                {
//                    return BadRequest("یکی از محاسبات ناموفق بود");
//                }
//                if (model.QualityFactor == "A%2B")
//                {
//                    model.QualityFactor = ConstantMaterialName.QualityFactor_Aplus;
//                }
//                if (model.QualityFactor == "A%2B%2B")
//                {
//                    model.QualityFactor = ConstantMaterialName.QualityFactor_A2plus;
//                }
//                var Result = _recordProductService.HandleRecording(new RecordRequest
//                {
//                    factorId = model.FactorId == null || model.FactorId < 0 ? 0L : model.FactorId.Value,
//                    productId = model.ProductId == null || model.ProductId < 0 ? 0L : model.ProductId.Value,
//                    subFactorId = model.SubfactorId == null || model.SubfactorId < 0 ? 0L : model.SubfactorId.Value,
//                    priceIsSuccess = CalculationResult.IsSuccess,
//                    productCost = new QfPrice
//                    {

//                        Price_A2plus = ConvertThePrice(CalculationResult.Data.Result_Aplus.ProductCost * 2 * ConstantMaterialName.A2pluseIncrese),
//                        Price_Aplus = ConvertThePrice(CalculationResult.Data.Result_Aplus.ProductCost * 2),
//                        Price_A = ConvertThePrice(CalculationResult.Data.Result_A.ProductCost * 2),
//                        Price_B = ConvertThePrice(CalculationResult.Data.Result_B.ProductCost * 2),
//                    },
//                    QualityFactor = model.QualityFactor ?? ConstantMaterialName.QualityFactor_Aplus,
//                    description = model.description,
//                    productName = model.boardType.label,
//                    ProductDetails = NewtonsoftJson.JsonConvert.SerializeObject(model, settings),
//                    priceMessage = CalculationResult.Message,
//                    NestingResult = NewtonsoftJson.JsonConvert.SerializeObject(Nesting),
//                });
//                if (Result.Result.IsSuccess)
//                {
//                    await Task.Delay(50);
//                    var Request_A = _productPriceDetailSetService.PriceRequestMapp(NewtonsoftJson.JsonConvert.SerializeObject(CalculationResult.Data.Result_A, settings));
//                    var PriceDetailResult_A = _productPriceDetailSetService.PriceDetailSet(Request_A.Result, Result.Result.SupplemantaryData, ConstantMaterialName.QualityFactor_A);
//                    var Request_APlus = _productPriceDetailSetService.PriceRequestMapp(NewtonsoftJson.JsonConvert.SerializeObject(CalculationResult.Data.Result_Aplus, settings));
//                    var PriceDetailResult_APlus = _productPriceDetailSetService.PriceDetailSet(Request_APlus.Result, Result.Result.SupplemantaryData, ConstantMaterialName.QualityFactor_Aplus);
//                    var Request_B = _productPriceDetailSetService.PriceRequestMapp(NewtonsoftJson.JsonConvert.SerializeObject(CalculationResult.Data.Result_B, settings));
//                    var PriceDetailResult_B = _productPriceDetailSetService.PriceDetailSet(Request_B.Result, Result.Result.SupplemantaryData, ConstantMaterialName.QualityFactor_B);
//                    if (PriceDetailResult_B.Result.IsSuccess && PriceDetailResult_APlus.Result.IsSuccess && PriceDetailResult_A.Result.IsSuccess)
//                    {

//                        return Ok(Result.Result);
//                    }

//                    return BadRequest("خطا در ثبت ریز قیمت فاکتور");


//                }


//                return BadRequest(Result.Result.Message);


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
