//using Newtonsoft.Json.Linq;
//using Radin.Domain.Entities.Products;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Radin.Application.Services.Product.Commands.ChallPrice.ChallPriceService;

//namespace Radin.Application.Services.Product.Commands.Mapping
//{
//    public class ChallMappingDto
//    {

//        public   ChallRequest MapJsonToRequestChallCostDto(JObject jsonData,JObject data)
//        {
//            int Checkpoint = 1;
//            var punchCheckpoint = false;
//            //try
//            //{
//                var punch = "";
//                Checkpoint = jsonData["data"]?["modelLayerLetters"]?["value"]?["id"]?.Value<int>() ?? default(int);
//                if (Checkpoint == 1)
//                {
//                    punch = jsonData["data"]?["modelLayerLetters"]?["one"]?["needPunchPelekcy"]?["nature"]?["label"]?.Value<string>();
//                    punchCheckpoint = jsonData["data"]?["modelLayerLetters"]?["one"]?["needPunchPelekcy"]?["value"]?.Value<bool>() ?? default(bool);
//                }
//                else if (Checkpoint == 2)
//                {
//                    punch = jsonData["data"]?["modelLayerLetters"]?["two"]?["needPunch"]?["nature"]?["label"]?.Value<string>();
//                    punchCheckpoint = jsonData["data"]?["modelLayerLetters"]?["two"]?["needPunch"]?["value"]?.Value<bool>() ?? default(bool);

//                }
//                else
//                {

//                }
//                Console.WriteLine($@"Title={jsonData["boardType"]?["label"]?.Value<string>()}");
//                Console.WriteLine($@"EdgeSize={jsonData["data"]?["edgesSize"]?["label"]?.Value<float>() ?? default(float)}");
//                Console.WriteLine($@"EdgeColor={jsonData["data"]?["edgeColor"]?["label"]?.Value<string>()}");
//                Console.WriteLine($@"PvcCheckPoint={jsonData["data"]?["needPVC"]?["value"]?.Value<bool>() ?? default(bool)}");
//                Console.WriteLine($@"FirstLayerColor={jsonData["data"]?["modelLayerLetters"]?["one"]?["colorPelekcy"]?["label"]?.Value<string>()}");
//                Console.WriteLine($@"PlexiPunchModel={jsonData["data"]?["modelLayerLetters"]?["one"]?["needPunchPelekcy"]?["nature"]?["label"]?.Value<string>()}");
//                //Console.WriteLine($@"CrystalModel={jsonData["data"]?["needCrystal"]?["color"]?["label"]?.Value<string>()}");
//                Console.WriteLine($@"BSmdModel={jsonData["data"]?["needPVC"]?["backLight"]?["nature"]?["label"]?.Value<string>()}");
//                Console.WriteLine($@"FSmdModel={jsonData["data"]?["needPVC"]?["frontLight"]?["nature"]?["label"]?.Value<string>()}");
//                //Console.WriteLine($@"PvcBackLightMargin={jsonData["data"]?["PVCHasBackLight"]?["margin"]?["label"]?.Value<float>() ?? default(float)}");
//                Console.WriteLine($@"PunchModel={punch}");
//                Console.WriteLine($@"PowerCheckpoint={jsonData["data"]?["power"]?["value"]?.Value<bool>() ?? default(bool)}");
//                Console.WriteLine($@"powerdata={jsonData["data"]?["power"]?["data"]?.ToObject<Dictionary<int, int>>()}");
//                Console.WriteLine($@"PowerCalculationType={jsonData["data"]?["power"]?["count"]?["label"]?.Value<string>()}");
//                Console.WriteLine($@"SecondLayerColor={jsonData["data"]?["modelLayerLetters"]?["two"]?["layerMaterial"]?["value"]?["label"]?.Value<string>()}");





//                var request1 = new RequestChallCostDto
//                {

//                    Title = jsonData["boardType"]?["label"]?.Value<string>(),
//                    EdgeSize = jsonData["data"]?["edgesSize"]?["label"]?.Value<float>() ?? default(float),
//                    EdgeColor = jsonData["data"]?["edgeColor"]?["label"]?.Value<string>(),
//                    PvcCheckPoint = jsonData["data"]?["needPVC"]?["value"]?.Value<bool>() ?? default(bool),
//                    FirstLayerColor = jsonData["data"]?["modelLayerLetters"]?["one"]?["colorPelekcy"]?["label"]?.Value<string>(),
//                    PlexiPunchModel = jsonData["data"]?["modelLayerLetters"]?["one"]?["needPunchPelekcy"]?["nature"]?["label"]?.Value<string>(),
//                    CrystalModel ="t",// jsonData["data"]?["needCrystal"]?["color"]?["label"]?.Value<string>(),
//                    BSmdModel = jsonData["data"]?["needPVC"]?["backLight"]?["nature"]?["label"]?.Value<string>(),
//                    FSmdModel = jsonData["data"]?["needPVC"]?["frontLight"]?["nature"]?["label"]?.Value<string>(),
//                    PvcBackLightMargin = jsonData["data"]?["PVCHasBackLight"]?["margin"]?["label"]?.Value<float>() ?? default(float),
//                    PunchModel = punch,
//                    PowerCheckpoint = jsonData["data"]?["power"]?["value"]?.Value<bool>() ?? default(bool),
//                    powerdata = jsonData["data"]?["power"]?["data"]?.ToObject<Dictionary<int, int>>(),
//                    PowerCalculationType = jsonData["data"]?["power"]?["count"]?["label"]?.Value<string>(),
//                    SecondLayerColor = jsonData["data"]?["modelLayerLetters"]?["two"]?["externalColorPelekcy"]?["label"]?.Value<string>(),
//                    SecondLayerModel = jsonData["data"]?["modelLayerLetters"]?["two"]?["layerMaterial"]?["value"]?["label"]?.Value<string>(),
//                    EdgePunchModel = jsonData["data"]?["isPunch"]?["nature"]?["label"]?.Value<string>(),
//                    LayerCondition = Checkpoint,
//                    //CrystalCondition= jsonData["data"]?["needCrystal"]?["color"]?.Value<bool>() ?? default(bool)

//                    LayerPunchCheckpoint = punchCheckpoint,
//                    EdgePunchCheckpoint = jsonData["data"]?["isPunch"]?["value"]?.Value<bool>() ?? default(bool),
//                    FSmdCheckpoint = jsonData["data"]?["needPVC"]?["frontLight"]?["value"]?.Value<bool>() ?? default(bool),
//                    BSmdCheckpoint = jsonData["data"]?["needPVC"]?["backLight"]?["value"]?.Value<bool>() ?? default(bool),
//                    CrystalCheckpoint = jsonData["data"]?["needCrystal"]?["value"]?.Value<bool>() ?? default(bool),
//                    PvcBackLightCheckPoint= jsonData["data"]?["PVCHasBackLight"]?["value"]?.Value<bool>() ?? default(bool),
//                };
//                //----------------------------------------------------------------------------------


//                var request2 = new RequestNfpInfoDto
//                {

//                    AConsumptionM1 = data["plexiNestingArea"].ToObject<float>(),
//                    AConsumptionM2 = data["secondLayerNestingArea"].ToObject<float>(),
//                    AConsumptionPvc = data["pvcNestingArea"].ToObject<float>(),
//                    ARealPvc = data["pvcRealArea"].ToObject<float>(),
//                    LRealPvc = data["pvcPerimeter"].ToObject<float>()
//                };
//                //----------------------------------------------------------------------------------





//                string choosedQualityFactor = "";// Default value
//                try
//                {
//                    var possibleLabel = jsonData["data"]?["power"]?["qualityDegree"]?["label"];
//                    if (possibleLabel != null)
//                    {
//                        choosedQualityFactor = possibleLabel.Value<string>();
//                    }
//                }
//                catch (Exception)
//                {
//                    //return new ChallRequest
//                    //{
//                    //    Message = " خطا در دریافت درجه کیفی پاور",
//                    //    Request1 = new RequestChallCostDto(),
//                    //    Request2 = new RequestNfpInfoDto(),
//                    //    ChoosedQualityFactor = ""

//                    //};
//                }

//                //----------------------------------------------------------------------------------



//                return new ChallRequest
//                {
//                    Message = " داده های اولیه محاسبه قیمت با موفقیت دریافت شد",
//                    Request1 = request1,
//                    Request2 = request2,
//                    ChoosedQualityFactor = choosedQualityFactor

//                };
//            //}

//            //catch
//            //{
//            //    return new ChallRequest
//            //    {
//            //        Message = " داده های اولیه محاسبه قیمت به درستی دریافت نشد",
//            //        Request1 = new RequestChallCostDto(),
//            //        Request2 = new RequestNfpInfoDto(),
//            //        ChoosedQualityFactor = ""

//            //    };
//            //}
//        }
//    }

//    public class ChallRequest
//    {
//        public string Message {  get; set; }
//        public RequestChallCostDto Request1 { get; set; }
//        public RequestNfpInfoDto Request2 { get; set; }
//        public string ChoosedQualityFactor { get; set; }
//    }
//}
