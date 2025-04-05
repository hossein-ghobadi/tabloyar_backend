//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Product.Commands.ChallPrice;
//using Radin.Application.Services.Product.Commands.PowerCalculation;
//using Radin.Common;
//using Radin.Common.Dto;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Radin.Application.Services.Product.Commands.ChallPrice.ChallPriceService;
//using NewtonsoftJson = Newtonsoft.Json;

//namespace Radin.Application.Services.Product.Commands.PlasticPrice
//{
//    public class PlasticPriceService: IPlasticPriceService
//    {
//        private readonly IPriceFeeDataBaseContext _context;
//        private readonly IPowerCalculationService _powerCalculationService;
//        public PlasticPriceService(IPriceFeeDataBaseContext Context, IPowerCalculationService powerCalculationService)
//        {
//            _context = Context;
//            _powerCalculationService = powerCalculationService;
//        }



//        private float TotalCost = 0;
//        private float EdgeCost = 0;
//        private float EdgeWorkerCost = 0;
//        private float FSmdCount = 0;
//        private float FSmdCost = 0;
//        private float BSmdCount = 0;
//        private float BSmdCost = 0;
//        //private float SmdWorkerCost = 0;
//        private float GlueCost = 0;
//        private float PunchCost = 0;
//        private float CrystalCost = 0;
//        private float MLayoutCost = 0;
//        private float PvcLayoutCost = 0;
//        private float SecondMLayoutCost = 0;
//        private ResultPlasticPriceDto falseResult = new ResultPlasticPriceDto
//        {
//            ProductCost = -1,
//            edgeCost = -1,
//            edgeWorkerCost = -1,
//            fSmdCost = -1,
//            bSmdCost = -1,
//            glueCost = -1,
//            punchCost = -1,
//            crystalCost = -1,
//            mLayoutCost = -1,
//            SecondMLayoutCost = -1,

//            pvcLayoutCost = -1,
//            powerCost = -1,
//            lRealPvc = -1,
//            aConsumptionPvc = -1,
//            aConsumptionM1 = -1,
//            aConsumptionM2 = -1,
//            aRealPvc = -1,

//        };

//        public ResultDto<ResultPlasticPriceDto, CalculationResult> Execute(RequestPlasticPriceDto request1, RequestPlasticNfpInfoDto request2, string QualityFactor)
//        {

//            try
//            {

//                var EdgeContent = _context.EdgeProperties.FirstOrDefault(m =>  m.EdgeTitle == request1.Title && m.QualityFactor == QualityFactor && request1.ImplementationModel == m.ImplementationModel);//&& m.EdgeColor == request1.EdgeColor && m.EdgeSize == request1.EdgeSize &&
//                if (EdgeContent != null)
//                {
//                    var EdgeFee = EdgeContent.EdgeFee * request1.EdgeSize;
//                    EdgeCost = request2.LRealPvc * EdgeFee;
//                }

//                else
//                {
//                    return new ResultDto<ResultPlasticPriceDto, CalculationResult>()
//                    {
//                        Data = falseResult,
//                        IsSuccess = false,
//                        Message = "مولفه‌های لبه وارد شود"
//                    };
//                }
//                //------------------------------------------------------------------------------



//                float EdgePunchFee = 0;
//                float EdgeWorkerFee = 0;
//                float EdgeHardnessFactor = 0;
//                //if (request1.EdgePunchModel != null && request1.EdgePunchCheckpoint == true)
//                //{
//                //    var EdgePunch = _context.EdgePunchs.FirstOrDefault(m => m.EdgePunchModel == request1.EdgePunchModel && m.EdgePunchTitle == request1.Title && m.QualityFactor == QualityFactor);
//                //    if (EdgePunch != null)
//                //    {
//                //        EdgePunchFee = EdgePunch.EdgePunchFee;
//                //    }
//                //    else
//                //    {
//                //        return new ResultDto<ResultPlasticPriceDto>()
//                //        {
//                //            Data = falseResult,
//                //            IsSuccess = false,
//                //            Message = "فی پانچ لبه وجود ندارد"
//                //        };
//                //    }
//                //}
//                var EdgeWorker = _context.EdgeProperties.FirstOrDefault(m => m.EdgeTitle == request1.Title && m.QualityFactor == QualityFactor);//request.FirstLayerModel
//                if (EdgeWorker != null)
//                {
//                    EdgeWorkerFee = EdgeWorker.EdgeWorkerFee;
//                }
//                else
//                {
//                    return new ResultDto<ResultPlasticPriceDto, CalculationResult>()
//                    {
//                        Data = falseResult,
//                        IsSuccess = false,
//                        Message = "فی دستمزد لبه وجود ندارد"
//                    };
//                }
//                var EdgeHardness = _context.EdgeProperties.FirstOrDefault(m => m.EdgeTitle == request1.Title && m.QualityFactor == QualityFactor);//request.FirstLayerModel
//                if (EdgeHardness != null)
//                {
//                    EdgeHardnessFactor = EdgeHardness.EdgeHardnessFactor;
//                }
//                else
//                {
//                    return new ResultDto<ResultPlasticPriceDto, CalculationResult>()
//                    {
//                        Data = falseResult,
//                        IsSuccess = false,
//                        Message = "فی ضریب سختی وجود ندارد"
//                    };
//                }
//                EdgeWorkerCost = request2.LRealPvc * (EdgeWorkerFee * EdgeHardnessFactor + EdgePunchFee);
//                //------------------------------------------------------------------------------




//                float FSmdFee = 0;
//                float FSmdGoldNumber = 0;
//                float FSmdWorkerFee = 0;
//                if (request1.FSmdModel != null && request1.FSmdCheckpoint == true)
//                {
//                    var FSmd = _context.Smds.FirstOrDefault(m => m.SmdTitle == request1.Title && m.SmdModel == request1.FSmdModel && m.QualityFactor == QualityFactor);//request.FirstLayerModel
//                    if (FSmd != null )
//                    {
//                        FSmdFee = FSmd.SmdFee;
//                        FSmdGoldNumber = FSmd.FSmdGoldNumber;
//                        FSmdWorkerFee = FSmd.SmdWorkerFee;
//                    }
//                    else
//                    {
//                        return new ResultDto<ResultPlasticPriceDto, CalculationResult>()
//                        {
//                            Data = falseResult,
//                            IsSuccess = false,
//                            Message = "اس ام دی جلوی کار وجود ندارد"
//                        };
//                    }
//                }
//                else if (request1.FSmdModel == null && request1.FSmdCheckpoint == true)
//                {
//                    return new ResultDto<ResultPlasticPriceDto, CalculationResult>()
//                    {
//                        Data = falseResult,
//                        IsSuccess = false,
//                        Message = "مدل اس ام دی روی کار انتخاب نشده است"
//                    };
//                }
//                FSmdCount = request2.ARealPvc * FSmdGoldNumber;
//                FSmdCost = FSmdCount * (FSmdFee + FSmdWorkerFee);
//                //------------------------------------------------------------------------------




//                float BSmdFee = 0;
//                float BSmdGoldNumber = 0;
//                float BSmdWorkerFee = 0;
//                if (request1.BSmdModel != null && request1.BSmdCheckpoint == true)
//                {
//                    var BSmd = _context.Smds.FirstOrDefault(m => m.SmdTitle == request1.Title && m.SmdModel == request1.BSmdModel && m.QualityFactor == QualityFactor);//request.FirstLayerModel
//                    if (BSmd != null)
//                    {
//                        BSmdFee = BSmd.SmdFee;
//                        BSmdGoldNumber = BSmd.BSmdGoldNumber;
//                        BSmdWorkerFee = BSmd.SmdWorkerFee;
//                    }
//                    else
//                    {
//                        return new ResultDto<ResultPlasticPriceDto, CalculationResult>()
//                        {
//                            Data = falseResult,
//                            IsSuccess = false,
//                            Message = "اس ام دی بک لایت وجود ندارد"
//                        };
//                    }
//                }
//                else if (request1.BSmdModel == null && request1.BSmdCheckpoint == true)
//                {
//                    return new ResultDto<ResultPlasticPriceDto, CalculationResult>()
//                    {
//                        Data = falseResult,
//                        IsSuccess = false,
//                        Message = "مدل اس ام دی بکلایت انتخاب نشده است"
//                    };
//                }
//                BSmdCount = request2.LRealPvc * BSmdGoldNumber;
//                BSmdCost = BSmdCount * (BSmdFee + BSmdWorkerFee);
//                //------------------------------------------------------------------------------



//                float GLueFee = 0;
//                var GLueInfo = _context.Glues.FirstOrDefault(m => m.GlueTitle == request1.Title && m.QualityFactor == QualityFactor);//request.FirstLayerModel
//                if (GLueInfo != null)
//                {
//                    GLueFee = GLueInfo.GLueFee;
//                }
//                else
//                {
//                    return new ResultDto<ResultPlasticPriceDto, CalculationResult>()
//                    {
//                        Data = falseResult,
//                        IsSuccess = false,
//                        Message = "اطلاعات چسب موجود نیست"
//                    };
//                }
//                GlueCost = request2.LRealPvc * GLueFee;
//                //------------------------------------------------------------------------------




//                float LayerPunchFee = 0;
//                float SecondLayerPunchFee = 0;
//                if (request1.PunchModel != null && request1.LayerPunchCheckpoint == true)
//                {
//                    var LayerPunch = _context.Punchs.FirstOrDefault(m => m.PunchTitle == request1.Title && m.PunchModel == request1.PunchModel); //&& m.QualityFactor == QualityFactor) && request.FirstLayerModel
//                    if (LayerPunch != null)
//                    {
//                        LayerPunchFee = LayerPunch.PunchFee;
//                    }
//                    else
//                    {
//                        return new ResultDto<ResultPlasticPriceDto, CalculationResult>()
//                        {
//                            Data = falseResult,
//                            IsSuccess = false,
//                            Message = "فی پانچ روی کار موجود نیست"
//                        };
//                    }
//                }
//                else if (request1.PunchModel == null && request1.LayerPunchCheckpoint == true)
//                {
//                    return new ResultDto<ResultPlasticPriceDto, CalculationResult>()
//                    {
//                        Data = falseResult,
//                        IsSuccess = false,
//                        Message = "نوع پانچ روی کار انتخاب نشده است"
//                    };
//                }
//                //**********
//                if (request1.SecondPunchModel != null && request1.SecondLayerPunchCheckpoint == true)
//                {
//                    var SecondLayerPunch = _context.Punchs.FirstOrDefault(m => m.PunchTitle == request1.Title && m.PunchModel == request1.SecondPunchModel);// && m.QualityFactor == QualityFactor && request.FirstLayerModel
//                    if (SecondLayerPunch != null)
//                    {
//                        SecondLayerPunchFee = SecondLayerPunch.PunchFee;
//                    }
//                    else
//                    {
//                        return new ResultDto<ResultPlasticPriceDto, CalculationResult>()
//                        {
//                            Data = falseResult,
//                            IsSuccess = false,
//                            Message = "فی پانچ روی کار موجود نیست"
//                        };
//                    }
//                }
//                else if (request1.SecondPunchModel == null && request1.SecondLayerPunchCheckpoint == true)
//                {
//                    return new ResultDto<ResultPlasticPriceDto, CalculationResult>()
//                    {
//                        Data = falseResult,
//                        IsSuccess = false,
//                        Message = "نوع پانچ روی کار انتخاب نشده است"
//                    };
//                }

//                PunchCost = request2.ARealPvc * LayerPunchFee + request2.secondLayerRealArea * SecondLayerPunchFee;
//                //------------------------------------------------------------------------------




//                float CrystalFee = 0;
//                if (request1.CrystalModel != null && request1.CrystalCheckpoint == true)
//                {
//                    var Crystal = _context.Crystals.FirstOrDefault(m => m.CrystalColor == request1.CrystalModel && m.QualityFactor == QualityFactor);// && m.CrystalColor == request1.CrystalColor);
//                    if (Crystal != null)
//                    {
//                        CrystalFee = Crystal.CrystalFee;
//                    }
//                    else
//                    {
//                        return new ResultDto<ResultPlasticPriceDto, CalculationResult>()
//                        {
//                            Data = falseResult,
//                            IsSuccess = false,
//                            Message = "فی کریستال موجود نیست"
//                        };
//                    }
//                }
//                else if (request1.CrystalModel == null && request1.CrystalCheckpoint == true)
//                {
//                    return new ResultDto<ResultPlasticPriceDto, CalculationResult>()
//                    {
//                        Data = falseResult,
//                        IsSuccess = false,
//                        Message = "نوع کریستال انتخاب نشده است"
//                    };
//                }
//                CrystalCost = request2.ARealPvc * CrystalFee;
//                //------------------------------------------------------------------------------




//                float FirstLayerFee = 0;
//                var FirstLayer = _context.Materials.FirstOrDefault(m => m.MaterialName == ConstantMaterialName.plexi && m.QualityFactor == QualityFactor);// request.FirstLayerModel);// && m.MaterialColor == request.FirstLayerColor);//request.FirstLayerModel
//                if (FirstLayer != null)
//                {
//                    FirstLayerFee = FirstLayer.MaterialFee;
//                }
//                else
//                {
//                    return new ResultDto<ResultPlasticPriceDto, CalculationResult>()
//                    {
//                        Data = falseResult,
//                        IsSuccess = false,
//                        Message = "فی متریال لایه اول موجود نیست"
//                    };
//                }
//                MLayoutCost = request2.AConsumptionM1 * FirstLayerFee;
//                //------------------------------------------------------------------------------



//                float SecondLayerFee = 0;
//                if (request1.SecondLayerModel != null && request1.LayerCondition == 2)
//                {
//                    var SecondLayer = _context.Materials.FirstOrDefault(m => m.MaterialName == request1.SecondLayerModel && m.QualityFactor == QualityFactor);// && m.MaterialColor == request.SecondLayerColor);//request.FirstLayerModel
//                    if (SecondLayer != null)
//                    {
//                        SecondLayerFee = SecondLayer.MaterialFee;
//                    }
//                    else
//                    {
//                        return new ResultDto<ResultPlasticPriceDto, CalculationResult>()
//                        {
//                            Data = falseResult,
//                            IsSuccess = false,
//                            Message = "فی متریال لایه دوم موجود نیست"
//                        };
//                    }
//                }
//                else if (request1.SecondLayerModel == null && request1.LayerCondition == 2)
//                {
//                    return new ResultDto<ResultPlasticPriceDto, CalculationResult>()
//                    {
//                        Data = falseResult,
//                        IsSuccess = false,
//                        Message = "نوع متریال لایه دوم انتخاب نشده است"
//                    };
//                }
//                SecondMLayoutCost = request2.AConsumptionM2 * SecondLayerFee;
//                //------------------------------------------------------------------------------


//                float PvcLayerFee = 0;
//                if (request1.PvcCheckPoint == true)
//                {
//                    var PvcLayer = _context.Materials.FirstOrDefault(m => m.MaterialName == ConstantMaterialName.pvc && m.QualityFactor == QualityFactor);// && m.MaterialColor == request.SecondLayerColor);//request.FirstLayerModel
//                    if (PvcLayer != null)
//                    {
//                        PvcLayerFee = PvcLayer.MaterialFee;
//                    }
//                    else
//                    {
//                        return new ResultDto<ResultPlasticPriceDto, CalculationResult>()
//                        {
//                            Data = falseResult,
//                            IsSuccess = false,
//                            Message = "فی پی وی سی موجود نیست"
//                        };
//                    }
//                }
//                PvcLayoutCost = request2.AConsumptionPvc * PvcLayerFee;
//                //------------------------------------------------------------------------------


//                float Pvc_margin = 0;
//                if (request1.PvcBackLightCheckPoint == true)
//                {
//                    Pvc_margin = request1.PvcBackLightMargin;
//                    if (Pvc_margin != null)
//                    {
//                        PvcLayoutCost = PvcLayoutCost + request2.BacklightConsumption * PvcLayerFee;
//                    }
//                }
//                //------------------------------------------------------------------------------
//                float PowerCost = 0;
//                var Powerinfo = new CalculationResult();
//                List<IdLabelDto>? powerList = new List<IdLabelDto>();

//                Powerinfo.Fsmd = FSmdCount;
//                Powerinfo.Bsmd = BSmdCount;
//                if (request1.PowerCheckpoint == true)
//                {
//                    if (request1.PowerCalculationType == ConstantMaterialName.selfchoice | request1.PowerCalculationType == null)
//                    {
//                        //Console.WriteLine("11111111111111111111");

//                        //int Smd = (int)(FSmdCount + BSmdCount);
//                        Powerinfo = _powerCalculationService.CalculateTotalCost(FSmdCount, BSmdCount, QualityFactor);
//                        powerList = Powerinfo.PowerList.Select(p => new IdLabelDto { id = p.PowerType, label = p.Quantity.ToString() }).ToList();

//                        PowerCost = Powerinfo.TotalCost;
//                    }
//                    else
//                    {
//                        PowerCost = 0;  
//                        //Console.WriteLine("2222222222222222222222222");
//                        //if (request1.powerdata != null && ChoosedQualityFactor != null)
//                        //{
//                        //    PowerCost = _powerCalculationService.selfChoosenTotalCost(request1.powerdata, ChoosedQualityFactor);
//                        //}
//                    }
//                }
//                TotalCost = EdgeCost + EdgeWorkerCost + FSmdCost + BSmdCost + GlueCost + PunchCost + CrystalCost + MLayoutCost + PvcLayoutCost + PowerCost+ SecondMLayoutCost;
//                //foreach (var pair in request1.powerdata)
//                //{
//                //    Console.WriteLine($"Key: {pair.Key}, Value: {pair.Value}");
//                //}

//                return new ResultDto<ResultPlasticPriceDto, CalculationResult>
//                {
//                    Data = new ResultPlasticPriceDto
//                    {
//                        ProductCost = TotalCost,
//                        edgeCost = EdgeCost,
//                        edgeWorkerCost = EdgeWorkerCost,
//                        fSmdCost = FSmdCost,
//                        fSmdCount = FSmdCount,
//                        bSmdCount = BSmdCount,
//                        bSmdCost = BSmdCost,
//                        glueCost = GlueCost,
//                        punchCost = PunchCost,
//                        crystalCost = CrystalCost,
//                        mLayoutCost = MLayoutCost,
//                        SecondMLayoutCost = SecondMLayoutCost,
//                        pvcLayoutCost = PvcLayoutCost,
//                        powerCost = PowerCost,
//                        lRealPvc = request2.LRealPvc,
//                        aConsumptionPvc = request2.AConsumptionPvc,
//                        aConsumptionM1 = request2.AConsumptionM1,
//                        aConsumptionM2 = request2.AConsumptionM2,
//                        aRealPvc = request2.ARealPvc,
//                        powerList = powerList.Count != 0 ? NewtonsoftJson.JsonConvert.SerializeObject(powerList) : null


//                    },
//                    IsSuccess = true,
//                    Message = "محاسبه قیمت با موفقیت انجام شد",
//                    SupplemantaryData= Powerinfo

//                };
//            }
        
//            catch (Exception)
//            {
//                return new ResultDto<ResultPlasticPriceDto, CalculationResult>()
//                {
//                    Data = falseResult,
//                    IsSuccess = false,
//                    Message = "محاسبه قیمت با خطا مواجه شد"
//                };
//            }
//        }


//        public ResultDto<AllQfPlasticResultDto> AllQfCalculation(RequestPlasticPriceDto request1, RequestPlasticNfpInfoDto request2)
//        {
//            var CalculationAplus = Execute(request1, request2, ConstantMaterialName.QualityFactor_Aplus);
//            var CalculationA = Execute(request1, request2, ConstantMaterialName.QualityFactor_A);
//            var CalculationB = Execute(request1, request2, ConstantMaterialName.QualityFactor_B);
//            if (!(CalculationAplus.IsSuccess && CalculationA.IsSuccess && CalculationB.IsSuccess))
//            {
//                return new ResultDto<AllQfPlasticResultDto>
//                {

//                    Data = new AllQfPlasticResultDto(),
//                    IsSuccess = false,
//                    Message = "یکی از محاسبات ناموفق بود"


//                };

//            }
//            else
//            {
//                return new ResultDto<AllQfPlasticResultDto>
//                {

//                    Data = new AllQfPlasticResultDto
//                    {

//                        Result_Aplus = CalculationAplus.Data,
//                        Result_A = CalculationA.Data,
//                        Result_B = CalculationB.Data

//                    },

//                    IsSuccess = true,
//                    Message = "محاسبه موفق"


//                };
//            }


//        }
//        public ResultDto<MultipartFormDataContent> NestingInputsGet(string address, string secondLayer, float margin, int secondLayerState)
//        {
//            try
//            {
//                var sheetLength = _context.Materials
//                .Where(p => p.MaterialName == ConstantMaterialName.plexi)
//                .Select(p => p.MaterialSizeX)
//                .FirstOrDefault();
//                var sheetWidth = _context.Materials
//                    .Where(p => p.MaterialName == ConstantMaterialName.plexi)
//                    .Select(p => p.MaterialSizeY)
//                    .FirstOrDefault();
//                var pvcLength = _context.Materials
//                    .Where(p => p.MaterialName == ConstantMaterialName.pvc)
//                    .Select(p => p.MaterialSizeX)
//                    .FirstOrDefault();
//                var pvcWidth = _context.Materials
//                    .Where(p => p.MaterialName == ConstantMaterialName.pvc)
//                    .Select(p => p.MaterialSizeY)
//                    .FirstOrDefault();
//                float secondSheetLength = 0;
//                float secondSheetWidth = 0;
//                if (secondLayer != "")
//                {
//                    secondSheetLength = _context.Materials
//                               .Where(p => p.MaterialName == secondLayer)
//                               .Select(p => p.MaterialSizeX)
//                               .FirstOrDefault();
//                    secondSheetWidth = _context.Materials
//                       .Where(p => p.MaterialName == secondLayer)
//                       .Select(p => p.MaterialSizeY)
//                       .FirstOrDefault();
//                }
                

//                var content = new MultipartFormDataContent
//                {
//                    { new StringContent($@"{address}"), "fileAddress" },
//                    { new StringContent(sheetLength.ToString()), "sheetLength" },
//                    { new StringContent(sheetWidth.ToString()), "sheetWidth" },
//                    { new StringContent(pvcLength.ToString()), "pvcLength" },
//                    { new StringContent(pvcWidth.ToString()), "pvcWidth" },
//                    { new StringContent(secondSheetLength.ToString()), "secondSheetLength" },
//                    { new StringContent(secondSheetWidth.ToString()), "secondSheetWidth" },
//                    { new StringContent(secondLayerState.ToString()), "secondLayerState" },
//                    { new StringContent(margin.ToString()), "margin" },

//                };
//                return new ResultDto<MultipartFormDataContent>()
//                {
//                    Data = content,
//                    IsSuccess = true,
//                    Message = "اطلاعات ورودی نستینگ با موفقیت دریافت شد"
//                };
            
//            }
//           catch (Exception) {

//                return new ResultDto<MultipartFormDataContent>()
//                {
//                    Data = new MultipartFormDataContent { } ,
//                    IsSuccess = false,
//                    Message = "اطلاعات ورودی برای انجام عملیات نستینگ بدرستی دریافت نشد"
//                };

//            }
//        }

//    }
//}
