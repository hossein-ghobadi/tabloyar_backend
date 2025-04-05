//using Newtonsoft.Json;
//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.Excelloading;
//using Radin.Common;
//using Radin.Common.Dto;
//using Radin.Domain.Entities.Factors;
//using Radin.Domain.Entities.Products;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Reflection.Emit;
//using System.Text;
//using System.Threading.Tasks;
//using NewtonsoftJson = Newtonsoft.Json;

//namespace Radin.Application.Services.FactorComplementation.Queries
//{
//    public interface IFactorComplementationFieldsGetService
//    {
//        Task<ResultDto<FactorComplementaionResult>> GetFields(long productId,int complementaryType);
//        Task<ResultDto<List<IdLabelIsDefault>>> GetComplementaryTypes();
//        Task<ResultDto<FactorItemsDetails>> GetFactorDetails(long productId);



//    }

//    public class FactorComplementationFieldsGetService : IFactorComplementationFieldsGetService
//    {
//        private readonly IDataBaseContext _context;
//        private readonly IPriceFeeDataBaseContext _context2;

//        //private static readonly HttpClient client = new HttpClient();

//        public FactorComplementationFieldsGetService(IDataBaseContext context, IPriceFeeDataBaseContext context2)
//        {
//            _context = context;
//            _context2 = context2;
//        }

//        public async Task<ResultDto<List<IdLabelIsDefault>>> GetComplementaryTypes()
//        {
//            var Result = _context.FactorComplementaryTypes.Select(p=>new IdLabelIsDefault
//            {
//                id=p.ComplementaryId,
//                label=p.Description,
//                isDefault=false
//            }).ToList();
//            Result[0].isDefault=true;
//            return new ResultDto<List<IdLabelIsDefault>>() { Data= Result ,IsSuccess=true,Message="دریافت موفق"};
//        }



//        public async Task<ResultDto<FactorItemsDetails>> GetFactorDetails(long productId)
//        {
//            var product = _context.ProductFactors
//                .FirstOrDefault(p => !p.IsUndefinedProduct && !p.IsAccessory && !p.IsService && !p.IsRemoved && p.Id == productId);

//            if (product == null)
//            {
//                return new ResultDto<FactorItemsDetails> { IsSuccess = false, Message = "چنین فاکتوری وجود ندارد" };
//            }

//            var factor = _context.MainFactors.FirstOrDefault(p => p.Id == product.FactorID);
//            var subfactor = _context.SubFactors.FirstOrDefault(p => p.Id == product.SubFactorID);

//            if (factor == null || subfactor == null)
//            {
//                return new ResultDto<FactorItemsDetails> { IsSuccess = false, Message = "چنین فاکتوری وجود ندارد" };
//            }

//            dynamic detail = JsonConvert.DeserializeObject<dynamic>(product.ProductDetails);
//            dynamic detail2 = JsonConvert.DeserializeObject<dynamic>(product.NestingResult);
//            bool layernum = detail.data.modelLayerLetters.value.id != 1;
//            bool pvcCondition = detail.data.needPVC.value;
//            bool powerCondition= detail.data.power.value;
//            bool backLightCondition =   detail.data.PVCHasBackLight.value;
//            bool crystalCondition =  detail.data.needCrystal.value;
//            string crystalType = detail.data.needCrystal.location.label;
//            bool fsmdCondition = detail.data.needPVC.frontLight.value; 
//            bool fsmdColorHistory= (detail.data.needPVC.frontLight.nature.label == ConstantMaterialName.singleColor | detail.data.needPVC.frontLight.nature.label == ConstantMaterialName.mixedColor);
//            bool bsmdCondition = detail.data.needPVC.backLight.value; 
//            bool bsmdColorHistory= (detail.data.needPVC.backLight.nature.label == ConstantMaterialName.singleColor | detail.data.needPVC.backLight.nature.label == ConstantMaterialName.mixedColor);
//            var firstPart = new List<IdLabelString>
//            {
//                new() { id = "نام کار", label = factor.WorkName },
//                new() { id = "جنس تابلو", label = product.Name },
//                new() { id = "درجه کیفی", label = subfactor.QualityFactor }
//            };

//            var firstPartCheckbox = new List<IdLabelIsDefault>
//            {
//                new() { label = "PVC", isDefault = pvcCondition },
//                new() { label = "PVC بک لایت", isDefault = backLightCondition },
//                new() { label = "دولایه", isDefault = layernum },
//                new() { label = "پاور", isDefault = powerCondition },
//                new() { label = "کریستال", isDefault = powerCondition },
//            };



//            var secondPart = new List<SecondPart>();


//            //////////////////////////////////////////////////////////
//            var edgeHistory = _context.FactorProductComplementaries
//                        .Where(p => p.ProductId == product.Id && p.ComplementaryId == 1)
//                        .Select(p => new  { id = p.FirstArg, label = p.SecondArg, description = p.Description  })
//                        .ToList();

//            var EdgeItem = new SecondPart
//            {
//                Output = new List<IdLabelString>
//                    {
//                        new() { id = "سایز لبه", label = detail.data.edgesSize.label },
//                        new() { id = "متراژ لبه", label = Convert.ToString(detail2.LRealPvc) }
//                    },
//                History = new FactorDetailHistory
//                {
//                    Headers = new List<string> { "رنگ لبه", "توضیحات" },
//                    Rows = edgeHistory.Select(color => new Dictionary<string, string>
//                    {
//                        { "رنگ لبه",  color.label != null ? $"{color.id}-{color.label}" : color.id },
//                        { "توضیحات", color.description }
//                        }).ToList()
//                    }
//                };

//            if (detail.data.isPunch.value == true) { EdgeItem.Output.Add(new IdLabelString { id = "مدل پانچ لبه", label = detail.data.isPunch.nature.label }); }//
//            secondPart.Add(EdgeItem);
//            //////////////////////////////////////////////////////////
//            ///
            



//            //////////////////////////////////////////////////////////
//            var secondLayerLabel = "";
//            var firstLayerPunchModel = "";
//            var secondLayerPunchModel = "";
//            var firstLayerPunchCondition = false;
//            var secondLayerPunchCondition = false;
//            if (layernum) { 
//                secondLayerLabel = detail.data.modelLayerLetters.two.layerMaterial.value.label;
//                secondLayerPunchCondition = detail.data.modelLayerLetters.two.layerMaterial.needPunchInternal.value;
//                firstLayerPunchCondition = detail.data.modelLayerLetters.two.layerMaterial.needPunch.value;
//                if (firstLayerPunchCondition) { firstLayerPunchModel = detail.data.modelLayerLetters.two.layerMaterial.needPunch.nature.label; }
//                if (secondLayerPunchCondition) { secondLayerPunchModel = detail.data.modelLayerLetters.two.layerMaterial.needPunchInternal.nature.label; }

//            }





//            //////////////////////////////////////////////////////////

//            var firstLayerHistory = _context.FactorProductComplementaries
//                        .Where(p => p.ProductId == product.Id && p.ComplementaryId == 2)
//                        .Select(p => new IdLabelString { id = p.FirstArg, label = p.Description })
//                        .ToList();
            
//            var FirstLayerItem = new SecondPart
//            {
//                Output = new List<IdLabelString>
//                    {
//                        new() { id = "جنس لایه بیرونی", label = "پلکسی" }
//                    },
//                History = new FactorDetailHistory
//                {
//                    Headers = new List<string> { "رنگ", "توضیحات" },
//                    Rows = firstLayerHistory.Select(color => new Dictionary<string, string>
//                        {
//                            { "رنگ", color.id },
//                            { "توضیحات", color.label }
//                        }).ToList()
//                }
//            };

//            if (firstLayerPunchCondition) { FirstLayerItem.Output.Add(new IdLabelString { id = "مدل پانچ لایه بیرونی", label = firstLayerPunchModel }); }//
//            secondPart.Add(FirstLayerItem);
//            //////////////////////////////////////////////////////////


//            //////////////////////////////////////////////////////////
//            if (layernum)
//            {
//                var secondLayerHistory = _context.FactorProductComplementaries
//                            .Where(p => p.ProductId == product.Id && p.ComplementaryId == 3)
//                            .Select(p => new IdLabelString { id = p.FirstArg, label = p.Description })
//                            .ToList();

//                var SecondtLayerItem = new SecondPart
//                {
//                    Output = new List<IdLabelString>
//                    {
//                        new() { id = "جنس لایه بیرونی", label = secondLayerLabel }
//                    },
//                    History = new FactorDetailHistory
//                    {
//                        Headers = new List<string> { "رنگ", "توضیحات" },
//                        Rows = secondLayerHistory.Select(color => new Dictionary<string, string>
//                        {
//                            { "رنگ", color.id },
//                            { "توضیحات", color.label }
//                        }).ToList()
//                    }
//                };

//                if (secondLayerPunchCondition) { SecondtLayerItem.Output.Add(new IdLabelString { id = "مدل پانچ لایه داخلی", label = secondLayerPunchModel }); }//
//                secondPart.Add(SecondtLayerItem);
//            }
//            //////////////////////////////////////////////////////////


//            //////////////////////////////////////////////////////////
//            if (fsmdCondition)
//            {
//                string fsmdModel = detail.data.needPVC.frontLight.nature.label;
//                var fsmdNumber=_context.ProductPriceDetails.Where(p=>p.ProductId==productId).FirstOrDefault();
//                var fsmdHistory = _context.FactorProductComplementaries
//                            .Where(p => p.ProductId == product.Id && p.ComplementaryId == 5)
//                            .Select(p => new  { id = p.FirstArg, label = p.SecondArg ,description=p.Description})
//                            .ToList();
                

//                var fsmdItem = new SecondPart
//                {
//                    Output = new List<IdLabelString>
//                    {
//                        new() { id = "مدل SMD جلوی کار", label = fsmdModel },
//                        new() { id = "تعداد SMD جلوی کار", label = fsmdNumber!=null?((int)fsmdNumber.FSmdCount).ToString():"????" }
//                    },
                    
                    
//                };
//                if (fsmdColorHistory)
//                {
//                    fsmdItem.History = new FactorDetailHistory
//                    {
//                        Headers = new List<string> { "رنگ SMD جلوی کار", "توضیحات" },
//                        Rows = fsmdHistory.Select(color => new Dictionary<string, string>
//                        {
//                            { "رنگ SMD جلوی کار",color.label != null ? $"{color.id}-{color.label}" : color.id },
//                            { "توضیحات", color.description }
//                        }).ToList()
//                    };
//                }
               
//                secondPart.Add(fsmdItem);
//            }
//            //////////////////////////////////////////////////////////




//            //////////////////////////////////////////////////////////
//            if (bsmdCondition)
//            {
//                string bsmdModel = detail.data.needPVC.backLight.nature.label;
//                var bsmdNumber = _context.ProductPriceDetails.Where(p => p.ProductId == productId).FirstOrDefault();
//                var bsmdHistory = _context.FactorProductComplementaries
//                            .Where(p => p.ProductId == product.Id && p.ComplementaryId == 6)
//                            .Select(p => new { id = p.FirstArg, label = p.SecondArg, description = p.Description })
//                            .ToList();


//                var bsmdItem = new SecondPart
//                {
//                    Output = new List<IdLabelString>
//                    {
//                        new() { id = "مدل SMD بک لایت", label = bsmdModel },
//                        new() { id = "تعداد SMD بک لایت", label = bsmdNumber!=null?((int)bsmdNumber.BSmdCount).ToString():"????" }
//                    },


//                };
//                if (fsmdColorHistory)
//                {
//                    bsmdItem.History = new FactorDetailHistory
//                    {
//                        Headers = new List<string> { "رنگ SMD بک لایت", "توضیحات" },
//                        Rows = bsmdHistory.Select(color => new Dictionary<string, string>
//                        {
//                            { "رنگ SMD بک لایت",color.label != null ? $"{color.id}-{color.label}" : color.id },
//                            { "توضیحات", color.description }
//                        }).ToList()
//                    };
//                }

//                secondPart.Add(bsmdItem);
//            }
//            //////////////////////////////////////////////////////////



//            //////////////////////////////////////////////////////////
//            if (crystalCondition)
//            {
//                var crystalHistory = _context.FactorProductComplementaries
//                            .Where(p => p.ProductId == product.Id && p.ComplementaryId == 4)
//                            .Select(p => new IdLabelString { id = p.FirstArg, label = p.Description })
//                            .ToList();

//                var crystalItem = new SecondPart
//                {
//                    Output = new List<IdLabelString>
//                    {
//                        new() { id = " نوع کریستال", label = crystalType }
//                    },
//                    History = new FactorDetailHistory
//                    {
//                        Headers = new List<string> { "رنگ کریستال", "توضیحات" },
//                        Rows = crystalHistory.Select(color => new Dictionary<string, string>
//                        {
//                            { "رنگ کریستال", color.id },
//                            { "توضیحات", color.label }
//                        }).ToList()
//                    }
//                };

//                secondPart.Add(crystalItem);
//            }
//            //////////////////////////////////////////////////////////
//            ///

//            Console.WriteLine($">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>                .>.>>>>>");


//            //////////////////////////////////////////////////////////
//            if (powerCondition && (fsmdCondition || bsmdCondition))
//            {
//                var subfactoritem = _context.SubFactors.FirstOrDefault(p => p.Id == product.SubFactorID);
//                if (subfactoritem == null)
//                {
//                    return new ResultDto<FactorItemsDetails>
//                    {

//                        IsSuccess = false,
//                        Message = "دریافت اطلاعات پاور با مشکل مواجه شده است"
//                    };

//                }
//                var powerHistory = _context.ProductPriceDetails
//                            .FirstOrDefault(p => p.ProductId == product.Id && p.QualityFactor == subfactoritem.QualityFactor);
//                if(powerHistory.powerList != null) 
//                {
//                    var powers = JsonConvert.DeserializeObject<List<IdLabelDto>>(powerHistory.powerList);

//                    var powerItem = new SecondPart
//                    {
//                        Output = new List<IdLabelString>
//                    {
//                        new() { id = " پاور", label = "" }
//                    },
//                        History = new FactorDetailHistory
//                        {
//                            Headers = new List<string> { "نوع پاور", "تعداد" },
//                            Rows = powers.Select(color => new Dictionary<string, string>
//                        {
//                            { "نوع پاور", color.id.ToString() },
//                            { "تعداد", color.label }
//                        }).ToList()
//                        }
//                    };

//                    secondPart.Add(powerItem);

//                }
                
//            }
//            //////////////////////////////////////////////////////////


//            //var saveName = $"جزییات محصول_{factor.WorkName}_{product.Name}_{product.Id}";

//            return new ResultDto<FactorItemsDetails>
//            {
//                Data = new FactorItemsDetails
//                {
//                    SaveName = "",
//                    firstPart = firstPart,
//                    firstPartCheckbox = firstPartCheckbox,
//                    secondPart = secondPart
//                },
//                IsSuccess = true,
//                Message = "دریافت موفق"
//            };
//            //var PunchModel= new IdLabelString
//            //{
//            //    id = "مدل پانچ",
//            //    label = Detail.LRealPvc,

//            //};
//        }







//        public async Task<ResultDto<FactorComplementaionResult>> GetFields(long productId, int complementaryType)
//        {


//            var product = _context.ProductFactors.FirstOrDefault(p => p.Id == productId && !p.IsRemoved);
//            if (product == null) { return new ResultDto<FactorComplementaionResult> { IsSuccess = false, Message = "چنین محصولی وجود ندارد" }; }
//            var factor = _context.MainFactors.FirstOrDefault(p => !p.IsRemoved && p.Id == product.FactorID);
//            var subfactor = _context.SubFactors.FirstOrDefault(p => !p.IsRemoved && p.Id == product.SubFactorID);//&& p.status);
//            if (subfactor == null || factor == null)
//            {
//                return new ResultDto<FactorComplementaionResult> { IsSuccess = false, Message = "چنین فاکتوری وجود ندارد" };
//            }
//            dynamic Detail = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(product.ProductDetails);

//            FactorComplementationItem Item = new FactorComplementationItem(_context, _context2);
//            if (complementaryType == 1)
//            {
//                var Result = Item.GetEdges(product, complementaryType);
//                return new ResultDto<FactorComplementaionResult> { Data = Result, IsSuccess = true, Message = "دریافت موفق" };
//            }


           


//            if ( complementaryType == 2)
//            {
//                var Result = Item.GetLayer1(product, 2);
//                return new ResultDto<FactorComplementaionResult> { Data = Result, IsSuccess = true, Message = "دریافت موفق" };
//            }

//            if (Detail.data.modelLayerLetters.value.id == 2 && (complementaryType == 2 || complementaryType == 3))
//            {
//                if (complementaryType == 3) {
                    
//                    var Result = Item.GetLayer2(product, 3);
//                    return new ResultDto<FactorComplementaionResult> { Data = Result, IsSuccess = true, Message = "دریافت موفق" };

//                }
//                else
//                {
//                    var Result = Item.GetLayer1(product, 2);
//                    return new ResultDto<FactorComplementaionResult> { Data = Result, IsSuccess = true, Message = "دریافت موفق" };
//                }
//            }
//            bool CrystalCondition= Detail.data.needCrystal.value;

//            if (complementaryType == 4 && CrystalCondition)
//            {
//                var Result = Item.GetCrystal(product, 4);
//                return new ResultDto<FactorComplementaionResult> { Data = Result, IsSuccess = true, Message = "دریافت موفق" };
//            }
//            string frontLight = Detail.data.needPVC.frontLight.nature.label;
//            string backLight = Detail.data.needPVC.backLight.nature.label;


//            if (complementaryType == 5 && Detail.data.needPVC.frontLight.value == true)
//            {

//                var Result = Item.GetFSmd(product, 5);
//                return new ResultDto<FactorComplementaionResult> { Data = Result, IsSuccess = true, Message = "دریافت موفق" };
//            }
//            if (complementaryType == 6 && Detail.data.needPVC.backLight.value == true)
//            {

//                var Result = Item.GetBSmd(product, 6);
//                return new ResultDto<FactorComplementaionResult> { Data = Result, IsSuccess = true, Message = "دریافت موفق" };
//            }




//            return new ResultDto<FactorComplementaionResult> { IsSuccess = false, Message = "چنین مشخصه‌ای برای فاکتور شما وجود ندارد" };




//        }











//    }
//        public class ComplexColorDto
//    {
//        public long id { get; set; }
//        public string label { get; set; }
//        //public GetDto QualityInfo { get; set; }
//        public List<IdLabelIsDefault> subItem { get; set; }
//        public bool? IsDefault { get; set; }
//    }

//    public class FactorComplementaionResult
//    {
//        public int id { get; set; }
//        public string label { get; set; }
//        public List<object> itemList { get; set; }
//        public List<object> History { get; set; }

//    }


//}
