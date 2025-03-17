//using Newtonsoft.Json.Linq;
//using Radin.Application.Services.Product.Commands.PlasticPrice;
//using Radin.Application.Services.Product.Commands.SimpleSteel;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Radin.Application.Services.Product.Commands.Mapping
//{
//    public class SimpleSteelMappingDto
//    {
//        public SimpleSteelRequest MapJsonToRequestChallCostDto(JObject jsonData, JObject data)
//        {
//            int Checkpoint = 1;
//            var punchCheckpoint = false;
//            try
//            {
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


//                var request1 = new SimpleSteelRequest
//                {

//                    Title = jsonData["boardType"]?["label"]?.Value<string>(),
//                    EdgeSize = jsonData["data"]?["edgesSize"]?["label"]?.Value<float>() ?? default(float),
//                    EdgeColor = jsonData["data"]?["edgeColor"]?["label"]?.Value<string>(),
//                    PvcCheckPoint = jsonData["data"]?["needPVC"]?["value"]?.Value<bool>() ?? default(bool),
//                    FirstLayerColor = jsonData["data"]?["modelLayerLetters"]?["one"]?["colorPelekcy"]?["label"]?.Value<string>(),
//                    PlexiPunchModel = jsonData["data"]?["modelLayerLetters"]?["one"]?["needPunchPelekcy"]?["nature"]?["label"]?.Value<string>(),
//                    CrystalModel = jsonData["data"]?["needCrystal"]?["color"]?["label"]?.Value<string>(),
//                    BSmdModel = jsonData["data"]?["needPVC"]?["backLight"]?["nature"]?["label"]?.Value<string>(),
//                    FSmdModel = jsonData["data"]?["needPVC"]?["frontLight"]?["nature"]?["label"]?.Value<string>(),
//                    PvcBackLightMargin = jsonData["data"]?["PVCHasBackLight"]?["margin"]?["label"]?.Value<float>() ?? default(float),
//                    PunchModel = jsonData["data"]?["modelLayerLetters"]?["one"]?["needPunchPelekcy"]?["nature"]?["label"]?.Value<string>(),
//                    PowerCheckpoint = jsonData["data"]?["power"]?["value"]?.Value<bool>() ?? default(bool),
//                    powerdata = jsonData["data"]?["power"]?["data"]?.ToObject<Dictionary<int, int>>(),
//                    PowerCalculationType = jsonData["data"]?["power"]?["count"]?["label"]?.Value<string>(),
//                    ImplementationModel = jsonData["data"]?["edgeMxecutionModel"]?["label"]?.Value<string>(),
//                    //CrystalCondition= jsonData["data"]?["needCrystal"]?["color"]?.Value<bool>() ?? default(bool)
//                    SecondLayerColor = jsonData["data"]?["modelLayerLetters"]?["two"]?["externalColorPelekcy"]?["label"]?.Value<string>(),
//                    SecondLayerModel = jsonData["data"]?["modelLayerLetters"]?["two"]?["layerMaterial"]?["value"]?["label"]?.Value<string>(),
//                    EdgePunchModel = jsonData["data"]?["isPunch"]?["nature"]?["label"]?.Value<string>(),
//                    LayerCondition = Checkpoint,



//                    LayerPunchCheckpoint = punchCheckpoint,
//                    EdgePunchCheckpoint = jsonData["data"]?["isPunch"]?["value"]?.Value<bool>() ?? default(bool),
//                    FSmdCheckpoint = jsonData["data"]?["needPVC"]?["frontLight"]?["value"]?.Value<bool>() ?? default(bool),
//                    BSmdCheckpoint = jsonData["data"]?["needPVC"]?["backLight"]?["value"]?.Value<bool>() ?? default(bool),
//                    CrystalCheckpoint = jsonData["data"]?["needCrystal"]?["value"]?.Value<bool>() ?? default(bool),



//                };
//                //----------------------------------------------------------------------------------


//                var request2 = new RequestSimpleSteelNfpInfoDto
//                {

//                    AConsumptionM1 = data["plexiNestingArea"].ToObject<float>(),
//                    AConsumptionM2 = 0,

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
//                    // Log exception or handle it according to your application's needs
//                    // choosedQualityFactor retains its default value
//                }
//                //----------------------------------------------------------------------------------



//                return new SimpleSteelRequest
//                {
//                    Request1 = request1,
//                    Request2 = request2,
//                    ChoosedQualityFactor = choosedQualityFactor

//                };
//            }

//            catch
//            {
//                return new SimpleSteelRequest
//                {
//                    Message = " داده های اولیه محاسبه قیمت به درستی دریافت نشد",
//                    Request1 = new RequestSimpleSteelPriceDto(),
//                    Request2 = new RequestSimpleSteelNfpInfoDto(),
//                    ChoosedQualityFactor = ""

//                };
//            }
//        }
//    }

//    public class SimpleSteelRequest
//    {
//        public string Message { get; set; }

//        public RequestSimpleSteelPriceDto Request1 { get; set; }
//        public RequestSimpleSteelNfpInfoDto Request2 { get; set; }
//        public string ChoosedQualityFactor { get; set; }
//    }
//}
//}
