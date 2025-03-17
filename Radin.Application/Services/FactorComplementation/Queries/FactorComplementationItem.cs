using Radin.Application.Interfaces.Contexts;
using Radin.Common;
using Radin.Common.Dto;
using Radin.Domain.Entities.Factors;
using Radin.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.FactorComplementation.Queries
{
    public class FactorComplementationItem
    {

        private readonly IDataBaseContext _context;
        private readonly IPriceFeeDataBaseContext _context2;


        public FactorComplementationItem(IDataBaseContext context, IPriceFeeDataBaseContext context2)
        {
            _context = context;
            _context2 = context2;
        }


        public FactorComplementaionResult GetEdges (ProductFactor product,int ComplementaryType)
        {
            var productId=product.Id;
            dynamic Detail = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(product.ProductDetails);
            int TitleId = Detail.boardType.id;
            string TitleName = Detail.boardType.label;
            var Result =new FactorComplementaionResult();

            if (TitleId == 9)
            {
                var ColorsList = new List<ComplexColorDto>();
                var ColorsList1 = _context2.MaterialEdgeColors.Where(p => p.Title == TitleName)
                                .GroupBy(p => p.EdgeColor)
                                    .Select(g => g.First())
                                    .AsEnumerable()
                                .Select(qd => new IdLabelIsDefault
                                {
                                    id = qd.Id,
                                    label = qd.EdgeColor,
                                    isDefault = qd.IsDefault ?? false,
                                })
                                .ToList();
                bool ColorsList1Default = ColorsList1.Any(c => c.isDefault == true);
                if (!ColorsList1Default && ColorsList1.Count > 0)
                {
                    ColorsList1[0].isDefault = true;
                }
                Console.WriteLine(ColorsList1);
                foreach (var color in ColorsList1)
                {
                    Console.WriteLine(color.label.ToString());
                    var secondColorListTemp = _context2.MaterialEdgeColors
                        .Where(pv => pv.EdgeColor == color.label && pv.Title == TitleName && pv.SecondEdgeColor != pv.EdgeColor)
                        .Select(pv => new IdLabelIsDefault
                        {
                            id = pv.Id,
                            label = pv.SecondEdgeColor.ToString(),
                            isDefault = pv.IsDefault ?? false
                        }) // First, get the labels
                        .ToList(); // Materialize the query to work with the data in-memory

                    // Now, project to Power2 with auto-incremented id
                    var secondColorList = secondColorListTemp.Select((label, index) => new IdLabelIsDefault
                    {
                        id = index + 1, // Auto-numbering applied here
                        label = label.label,
                        isDefault = label.isDefault,
                    }).ToList();
                    bool secondColorListDefault = secondColorList.Any(c => c.isDefault == true);
                    if (!secondColorListDefault && secondColorList.Count > 0)
                    {
                        secondColorList[0].isDefault = true;
                    }
                    var colorDto = new ComplexColorDto
                    {
                        id = color.id,
                        label = color.label,
                        IsDefault = color.isDefault,
                        subItem = secondColorList
                    };

                    ColorsList.Add(colorDto);

                }
                var History = _context.FactorProductComplementaries.Where(p => p.ProductId == productId && p.ComplementaryId == ComplementaryType).ToList().Cast<object>().ToList();


                Result.id = 1;
                Result.label = _context.FactorComplementaryTypes.FirstOrDefault(p => p.ComplementaryId == ComplementaryType).Description;
                Result.itemList = ColorsList.Cast<object>().ToList();
                Result.History = History;

                return Result;

            }
            else
            {
                var ColorsList = _context2.MaterialEdgeColors.Where(p => p.Title == TitleName).Select(p => new IdLabelIsDefault
                {
                    id = p.Id,
                    label = p.EdgeColor,
                    isDefault = p.IsDefault ?? false
                }).ToList();
                bool ColorsListDefault = ColorsList.Any(c => c.isDefault == true);
                if (!ColorsListDefault && ColorsList.Count > 0)
                {
                    ColorsList[0].isDefault = true;
                }
                var History = _context.FactorProductComplementaries.Where(p => p.ProductId == productId && p.ComplementaryId == ComplementaryType).ToList().Cast<object>().ToList();


                Result.id = 1;
                Result.label = _context.FactorComplementaryTypes.FirstOrDefault(p => p.ComplementaryId == ComplementaryType).Description;
                Result.itemList = ColorsList.Cast<object>().ToList();
                Result.History = History;
                return Result;



            }





        }














        public FactorComplementaionResult GetLayer1(ProductFactor product, int ComplementaryType)
        {
            var productId = product.Id;
            dynamic Detail = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(product.ProductDetails);
            int TitleId = Detail.boardType.id;
            string TitleName = Detail.boardType.label;
            var Result = new FactorComplementaionResult();

            var firstLayerColors = _context2.MaterialColors;
            
            var firstLayerColorsList = firstLayerColors.Where(p => p.MaterialName == ConstantMaterialName.plexi && p.Title == TitleName)
                //.GroupBy(p => p.Color) // Group by the Color property for uniqueness
                //.Select(g => g.First()) // Select the first item from each group
                .Select(p => new IdLabelIsDefault// p.Title==request &&
                {
                    id = p.Id,
                    label = p.Color,
                    isDefault = p.IsDefault ?? false
                }).ToList();
            bool firstLayerColorsDefault = firstLayerColorsList.Any(c => c.isDefault == true);
            if (!firstLayerColorsDefault && firstLayerColorsList.Count > 0)
            {
                firstLayerColorsList[0].isDefault = true;
            }

            var History = _context.FactorProductComplementaries.Where(p => p.ProductId == productId && p.ComplementaryId == ComplementaryType).ToList().Cast<object>().ToList();
         

            Result.id = 1;
            Result.label = _context.FactorComplementaryTypes.FirstOrDefault(p => p.ComplementaryId == ComplementaryType).Description;
            Result.itemList = firstLayerColorsList.Cast<object>().ToList();
            Result.History = History;
            return Result;

        }



        public FactorComplementaionResult GetLayer2(ProductFactor product, int ComplementaryType)
        {
            var productId = product.Id;
            dynamic Detail = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(product.ProductDetails);
            int TitleId = Detail.boardType.id;
            string TitleName = Detail.boardType.label;
            var Result = new FactorComplementaionResult();

            
            string materialName = Detail.data.modelLayerLetters.two.layerMaterial.value.label;


           
                var secondLayerColorsList = _context2.MaterialColors.Where(p => p.MaterialName == materialName && p.Title == TitleName)

                    //.GroupBy(p => p.Color) // Group by the Color property for uniqueness
                    //.Select(g => g.First()) // Select the first item from each group
                    .Select(p => new IdLabelIsDefault//p.Title==request &&
                    {
                        id = p.Id,
                        label = p.Color,
                        isDefault = p.IsDefault?? false
                    }).ToList();

            bool secondLayerColorsListDefault = secondLayerColorsList.Any(c => c.isDefault == true);
            if (!secondLayerColorsListDefault && secondLayerColorsList.Count > 0)
                {
                    secondLayerColorsList[0].isDefault = true;
                }

              

            var History = _context.FactorProductComplementaries.Where(p => p.ProductId == productId && p.ComplementaryId == ComplementaryType).ToList().Cast<object>().ToList();


            Result.id = 1;
            Result.label = _context.FactorComplementaryTypes.FirstOrDefault(p => p.ComplementaryId == ComplementaryType).Description;
            Result.itemList = secondLayerColorsList.Cast<object>().ToList();
            Result.History = History;
            return Result;

        }












        public FactorComplementaionResult GetCrystal(ProductFactor product, int ComplementaryType)
        {
            var productId = product.Id;
            dynamic Detail = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(product.ProductDetails);
           
            var Result = new FactorComplementaionResult();
            var crystals = _context2.Crystals;
            var crystalsList = crystals
                .GroupBy(p => p.CrystalModel)
                    .Select(g => g.First())
                    .AsEnumerable()
                    .Select((item, index) => new IdLabelIsDefault

                    {
                        id = index + 1,
                        label = item.CrystalColor,
                        isDefault = item.IsDefault?? false
                    }).ToList();


            bool CrystalDefault = crystalsList.Any(c => c.isDefault == true);
            if (!CrystalDefault && crystalsList.Count > 0)
            {
                crystalsList[0].isDefault = true;
            }

            var History = _context.FactorProductComplementaries.Where(p => p.ProductId == productId && p.ComplementaryId == ComplementaryType).ToList().Cast<object>().ToList();


            Result.id = 1;
            Result.label = _context.FactorComplementaryTypes.FirstOrDefault(p => p.ComplementaryId == ComplementaryType).Description;
            Result.itemList = crystalsList.Cast<object>().ToList();
            Result.History = History;
            return Result;

        }







        public FactorComplementaionResult GetFSmd(ProductFactor product, int ComplementaryType)
        {
            var productId = product.Id;
            dynamic Detail = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(product.ProductDetails);
            string ColorSmd = Detail.data.needPVC.frontLight.nature.label;
            bool singleColorSmd = false;
            int TitleId = Detail.boardType.id;
            string TitleName = Detail.boardType.label;
            var Result = new FactorComplementaionResult();
            if (ColorSmd == ConstantMaterialName.singleColor) { singleColorSmd = true; }

            var ColorsList = new List<ComplexColorDto>();
            var ColorsList1 = _context2.Smds.Where(p => p.SmdTitle == TitleName && p.SmdModel==ConstantMaterialName.singleColor)
                            .GroupBy(p => p.SmdColor)
                                .Select(g => g.First())
                                .AsEnumerable()
                            .Select(qd => new IdLabelIsDefault
                            {
                                id = qd.Id,
                                label = qd.SmdColor,
                                isDefault = qd.IsDefault ?? false,
                            })
                            .ToList();
            bool ColorsList1Default = ColorsList1.Any(c => c.isDefault == true);
            if (!ColorsList1Default && ColorsList1.Count > 0)
            {
                ColorsList1[0].isDefault = true;
            }


            if (singleColorSmd)
            {

                var History = _context.FactorProductComplementaries.Where(p => p.ProductId == productId && p.ComplementaryId == ComplementaryType).ToList().Cast<object>().ToList();


                Result.id = 1;
                Result.label = _context.FactorComplementaryTypes.FirstOrDefault(p => p.ComplementaryId == ComplementaryType).Description;
                Result.itemList = ColorsList1.Cast<object>().ToList();
                Result.History = History;
                return Result;
            }
            else
            {
                foreach (var color in ColorsList1)
                {
                    Console.WriteLine(color.label.ToString());
                    var secondColorListTemp = ColorsList1
                        .Where(pv => pv.label != color.label)
                        .Select(pv => new IdLabelIsDefault
                        {
                            id = pv.id,
                            label = pv.label.ToString(),
                            isDefault = pv.isDefault
                        }) // First, get the labels
                        .ToList(); // Materialize the query to work with the data in-memory

                    // Now, project to Power2 with auto-incremented id
                    var secondColorList = secondColorListTemp.Select((label, index) => new IdLabelIsDefault
                    {
                        id = index + 1, // Auto-numbering applied here
                        label = label.label,
                        isDefault = label.isDefault,
                    }).ToList();
                    bool secondColorListDefault = secondColorList.Any(c => c.isDefault == true);
                    if (!secondColorListDefault && secondColorList.Count > 0)
                    {
                        secondColorList[0].isDefault = true;
                    }
                    var colorDto = new ComplexColorDto
                    {
                        id = color.id,
                        label = color.label,
                        IsDefault = color.isDefault,
                        subItem = secondColorList
                    };

                    ColorsList.Add(colorDto);


                }
                var History = _context.FactorProductComplementaries.Where(p => p.ProductId == productId && p.ComplementaryId == ComplementaryType).ToList().Cast<object>().ToList();


                Result.id = 1;
                Result.label = _context.FactorComplementaryTypes.FirstOrDefault(p => p.ComplementaryId == ComplementaryType).Description;
                Result.itemList = ColorsList.Cast<object>().ToList();
                Result.History = History;
                return Result;

            }

        }


















        public FactorComplementaionResult GetBSmd(ProductFactor product, int ComplementaryType)
        {
            var productId = product.Id;
            dynamic Detail = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(product.ProductDetails);
            string ColorSmd = Detail.data.needPVC.backLight.nature.label;
            bool singleColorSmd = false;
            int TitleId = Detail.boardType.id;
            string TitleName = Detail.boardType.label;
            var Result = new FactorComplementaionResult();
            if (ColorSmd == ConstantMaterialName.singleColor) { singleColorSmd = true; }

            var ColorsList = new List<ComplexColorDto>();
            var ColorsList1 = _context2.Smds.Where(p => p.SmdTitle == TitleName && p.SmdModel == ConstantMaterialName.singleColor)
                            .GroupBy(p => p.SmdColor)
                                .Select(g => g.First())
                                .AsEnumerable()
                            .Select(qd => new IdLabelIsDefault
                            {
                                id = qd.Id,
                                label = qd.SmdColor,
                                isDefault = qd.IsDefault ?? false,
                            })
                            .ToList();
            bool ColorsList1Default = ColorsList1.Any(c => c.isDefault == true);
            if (!ColorsList1Default && ColorsList1.Count > 0)
            {
                ColorsList1[0].isDefault = true;
            }


            if (singleColorSmd)
            {

                var History = _context.FactorProductComplementaries.Where(p => p.ProductId == productId && p.ComplementaryId == ComplementaryType).ToList().Cast<object>().ToList();


                Result.id = 1;
                Result.label = _context.FactorComplementaryTypes.FirstOrDefault(p => p.ComplementaryId == ComplementaryType).Description;
                Result.itemList = ColorsList1.Cast<object>().ToList();
                Result.History = History;
                return Result;
            }
            else
            {
                foreach (var color in ColorsList1)
                {
                    Console.WriteLine(color.label.ToString());
                    var secondColorListTemp = ColorsList1
                        .Where(pv => pv.label != color.label)
                        .Select(pv => new IdLabelIsDefault
                        {
                            id = pv.id,
                            label = pv.label.ToString(),
                            isDefault = pv.isDefault
                        }) // First, get the labels
                        .ToList(); // Materialize the query to work with the data in-memory

                    // Now, project to Power2 with auto-incremented id
                    var secondColorList = secondColorListTemp.Select((label, index) => new IdLabelIsDefault
                    {
                        id = index + 1, // Auto-numbering applied here
                        label = label.label,
                        isDefault = label.isDefault,
                    }).ToList();
                    bool secondColorListDefault = secondColorList.Any(c => c.isDefault == true);
                    if (!secondColorListDefault && secondColorList.Count > 0)
                    {
                        secondColorList[0].isDefault = true;
                    }
                    var colorDto = new ComplexColorDto
                    {
                        id = color.id,
                        label = color.label,
                        IsDefault = color.isDefault,
                        subItem = secondColorList
                    };

                    ColorsList.Add(colorDto);


                }
                var History = _context.FactorProductComplementaries.Where(p => p.ProductId == productId && p.ComplementaryId == ComplementaryType).ToList().Cast<object>().ToList();


                Result.id = 1;
                Result.label = _context.FactorComplementaryTypes.FirstOrDefault(p => p.ComplementaryId == ComplementaryType).Description;
                Result.itemList = ColorsList.Cast<object>().ToList();
                Result.History = History;
                return Result;

            }

        }

    }
}
