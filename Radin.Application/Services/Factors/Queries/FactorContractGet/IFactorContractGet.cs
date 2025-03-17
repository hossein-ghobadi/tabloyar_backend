using Newtonsoft.Json;
using Radin.Application.Interfaces.Contexts;
using Radin.Common;
using Radin.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Factors.Queries.FactorContractGet
{
    public interface IFactorContractGet
    {
        ResultDto<ProductSelectionResult> GetFactorProductSelection(long productId);
    }
    public class FactorContractGet : IFactorContractGet
    {
        private IDataBaseContext _context;
        public FactorContractGet(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto<ProductSelectionResult> GetFactorProductSelection(long productId)
        {
            try
            {
                var Product = _context.ProductFactors.FirstOrDefault(p => p.Id == productId && !p.IsRemoved);
                if (Product == null) { return new ResultDto<ProductSelectionResult> { IsSuccess = false, Message = "محصول مورد نظر وجود ندارد" }; }
                dynamic detail = JsonConvert.DeserializeObject<dynamic>(Product.ProductDetails);
                var Result = new List<List<IdLabelString>>();
                var WorkType = Product.Name;
                var ProductId = Product.Id;
                Result.Add(new List<IdLabelString>
            {
                new(){ id="نوع محصول" , label=WorkType},
                new(){id="شماره محصول", label=ProductId.ToString() }
            });
                bool layernum = detail.data.modelLayerLetters.value.id != 1;

                string edgeColor = detail.data.edgeColor.label;
                string edgePunch = detail.data.isPunch.label;
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 00");

                bool edgePunchCondition = detail.data.isPunch.value;
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 0");

                string edgeSize = detail.data.edgesSize.label;
                string secondEdgeColor = detail.data.secondEdgeColor?.label ?? "";
                Result.Add(new List<IdLabelString>
            {
                new(){ id="رنگ لبه" , label=!string.IsNullOrEmpty(secondEdgeColor)?$"{edgeColor}-{secondEdgeColor}":edgeColor},
                new(){id="اندازه لبه", label=edgeSize },
                new(){id="پانچ لبه", label=edgePunchCondition? edgePunch:"ندارد"},

            });
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 1");


                string firstlayer = "پلکسی";
                string firstlayerColor = "";
                string firstlayerPunch = "";
                bool firstlayerPunchCondition = false;

                string secondlayer = "";
                string secondlayerColor = "";
                string secondlayerPunch = "";
                bool secondlayerPunchCondition = false;
                if (!layernum)
                {
                    firstlayerColor = detail.data.modelLayerLetters.one.colorPelekcy.label;
                    firstlayerPunchCondition = detail.data.modelLayerLetters.one.needPunchPelekcy.value;
                    firstlayerPunch = detail.data.modelLayerLetters.one.needPunchPelekcy.nature.label;
                                Result.Add(new List<IdLabelString>
                        {
                            new(){ id="جنس لایه اول" , label=firstlayer},
                            new(){id="رنگ لایه اول", label=firstlayerColor },
                            new(){id="پانچ لایه اول", label=firstlayerPunchCondition? firstlayerPunch:"ندارد"},

                        });
                }
                else
                {
                    firstlayerColor = detail.data.modelLayerLetters.two.externalColorPelekcy.label;
                    firstlayerPunchCondition = detail.data.modelLayerLetters.two.needPunch.value;
                    firstlayerPunch = detail.data.modelLayerLetters.two.needPunch.nature.label;
                    secondlayer = detail.data.modelLayerLetters.two.layerMaterial.value.label;
                    secondlayerColor = detail.data.modelLayerLetters.two.layerMaterial.color.label;
                    secondlayerPunchCondition = detail.data.modelLayerLetters.two.needPunchInternal.value;
                    secondlayerPunch = detail.data.modelLayerLetters.two.needPunchInternal.nature.label;


                    Result.Add(new List<IdLabelString>
                    {
                        new(){ id="جنس لایه دوم" , label=secondlayer},
                        new(){id="رنگ لایه دوم", label=secondlayerColor },
                        new(){id="پانچ لایه دوم", label=secondlayerPunchCondition? secondlayerPunch:"ندارد"},

                    });
                }
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 2");


                bool pvcCondition = detail.data.needPVC.value;
                bool fsmdCondition = detail.data.needPVC.frontLight.value;
                string fsmdType = detail.data.needPVC.frontLight.nature.label;
                if (fsmdCondition)
                {
                        Result.Add(new List<IdLabelString>
                        {
                            new(){ id="نوع اس ام دی جلوی کار" , label=fsmdType},


                        });
                }

                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> 3");


                bool bsmdCondition = detail.data.needPVC.backLight.value;
                string bsmdType = detail.data.needPVC.backLight.nature.label;
                if (bsmdCondition)
                {
                    Result.Add(new List<IdLabelString>
                    {
                        new(){ id="نوع اس ام دی جلوی کار" , label=bsmdType},


                    });
                }
                bool crystalCondition = detail.data.needCrystal.value;
                string crystalType = detail.data.needCrystal.location.label;
                string crystalColor = detail.data.needCrystal.color.label;
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>4");

                if (crystalCondition)
                {
                    Result.Add(new List<IdLabelString>
                    {
                        new(){ id="نوع کریستال" , label=crystalType},
                        new(){ id="رنگ کریستال" , label=crystalColor},

                    });
                }

                bool PvcbackLightCondition = detail.data.PVCHasBackLight.value;

                bool powerCondition = detail.data.power.value;

                var Conditions = new List<IdLabelIsDefault>
                {
                    new(){id=1,label="پانچ لبه",isDefault=edgePunchCondition},
                    new(){id=1,label="پانچ لایه اول",isDefault=firstlayerPunchCondition},
                    new(){id=1,label="پانچ لایه دوم",isDefault=secondlayerPunchCondition},
                    new(){id=1,label="کریستال ",isDefault=crystalCondition},
                    new(){id=1,label="PVC ",isDefault=pvcCondition},
                    new(){id=1,label="PVC بکلایت",isDefault=PvcbackLightCondition},
                    new(){id=1,label="پاور",isDefault=powerCondition }


                };

                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>5");

                var CompleteResult = new ProductSelectionResult
                {
                    Conditions = Conditions,
                    Specifications = Result
                };
                return new ResultDto<ProductSelectionResult> { Data = CompleteResult, IsSuccess = true, Message = "" };

            }
            catch (Exception ex) 
            {
                return new ResultDto<ProductSelectionResult> { IsSuccess = false, Message = "خطا در استخراج اطلاعات محصول" }; 
            }






        }

    }
    public class ProductSelectionResult
    {
        public List<IdLabelIsDefault> Conditions { get; set; }
        public List<List<IdLabelString>> Specifications { get; set; }
    }
}