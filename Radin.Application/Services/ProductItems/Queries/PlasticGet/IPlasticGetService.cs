//using Radin.Application.Interfaces.Contexts;
//using Radin.Application.Services.ProductItems.Queries.ChannelliumGet;
//using Radin.Common;
//using Radin.Common.Dto;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Radin.Application.Services.ProductItems.Queries.ChannelliumGet.ChannelliumGet;
//using static Radin.Application.Services.ProductItems.Queries.PlasticGet.PlasticGetService;

//namespace Radin.Application.Services.ProductItems.Queries.PlasticGet
//{
//    public interface IPlasticGetService
//    {
//        ResultDto<ResultPLasticGetDto> Execute(string request);
//    }
//    public class PlasticGetService : IPlasticGetService
//    {
//        private readonly IPriceFeeDataBaseContext _context;
//        public PlasticGetService(IPriceFeeDataBaseContext Context)
//        {
//            _context = Context;

//        }
//        // add first item as Isdefault if there is no Default in DataBase for IchallGetService
//        public ResultDto<ResultPLasticGetDto> Execute(string request)
//        {
//            var crystals = _context.Crystals;
//            var crystalsList = crystals
//                .GroupBy(p => p.CrystalModel)
//                    .Select(g => g.First())
//                    .AsEnumerable()
//                    .Select((item, index) => new GetDto

//                    {
//                        id = index + 1,
//                        label = item.CrystalColor,
//                        IsDefault = item.IsDefault
//                    }).ToList();
//            bool CrystalDefault = crystalsList.Any(c => c.IsDefault == true);
//            if (!CrystalDefault && crystalsList.Count > 0)
//            {
//                crystalsList[0].IsDefault = true;
//            }
//            //---------------------------------------------------
//            var edgeSizes = _context.MaterialEdgeSizes;
//            var edgeSizesList = edgeSizes.Where(p => p.Title==request)

//                .Select(p => new GetDto
//                {
//                    id = p.Id,
//                    label = p.EdgeSize.ToString(),
//                    IsDefault = p.IsDefault

//                }).ToList();
//            bool edgeSizeDefault = edgeSizesList.Any(c => c.IsDefault == true);
//            if (!edgeSizeDefault && edgeSizesList.Count > 0)
//            {
//                edgeSizesList[0].IsDefault = true;
//            }
//            //---------------------------------------------------
//            var edgePunchs = _context.EdgePunchs;
//            var edgePunchsList = edgePunchs.Where(p => p.EdgePunchTitle==request)
//                .GroupBy(p => p.EdgePunchModel)
//                    .Select(g => g.First())
//                    .AsEnumerable()
//                    .Select((item, index) => new GetDto

//                    {
//                        id = index + 1,
//                        label = item.EdgePunchModel,
//                        IsDefault = item.IsDefault

//                    }).ToList();
//            bool edgePunchsDefault = edgePunchsList.Any(c => c.IsDefault == true);
//            if (!edgePunchsDefault && edgePunchsList.Count > 0)
//            {
//                edgePunchsList[0].IsDefault = true;
//            }
//            //---------------------------------------------------
//            var Punchs = _context.Punchs;
//            var PunchsList = Punchs.Where(p => p.PunchTitle==request)
//                .GroupBy(p => p.PunchModel)
//                    .Select(g => g.First())
//                    .AsEnumerable()
//                    .Select((item, index) => new GetDto

//                    {
//                        id = index + 1,
//                        label = item.PunchModel,
//                        IsDefault = item.IsDefault
//                    }).ToList();
//            bool PunchDefault = PunchsList.Any(c => c.IsDefault == true);
//            if (!edgePunchsDefault && PunchsList.Count > 0)
//            {
//                PunchsList[0].IsDefault = true;
//            }
//            //---------------------------------------------------
//            var firstLayerColors = _context.MaterialColors;
//            var firstLayerColorsList = firstLayerColors.Where(p =>  p.MaterialName == ConstantMaterialName.plexi && p.Title == request)

//                //.GroupBy(p => p.Color) // Group by the Color property for uniqueness
//                //.Select(g => g.First()) // Select the first item from each group
//                .Select(p => new GetDto//p.Title==request &&
//            {
//                id = p.Id,
//                label = p.Color,
//                IsDefault = p.IsDefault
//            }).ToList();
//            bool firstLayerColorsDefault = firstLayerColorsList.Any(c => c.IsDefault == true);
//            if (!firstLayerColorsDefault && firstLayerColorsList.Count > 0)
//            {
//                firstLayerColorsList[0].IsDefault = true;
//            }
//            ////---------------------------------------------------
//            var MaterialList = _context.SecondLayerMaterials.Where(p => p.Title==request).Select(p => new GetDto
//            {
//                id = p.Id,
//                label = p.MaterialName,
//                IsDefault = p.IsDefault
//            }).ToList(); ;
//            bool MaterialListDefault = MaterialList.Any(c => c.IsDefault == true);
//            if (!MaterialListDefault && MaterialList.Count > 0)
//            {
//                MaterialList[0].IsDefault = true;
//            }

//            var secondLayerList = new List<TwoLayerDto>();
//            foreach (var material in MaterialList)
//            {
//                //var secondLayerList=
//                var secondLayerColorsList = _context.MaterialColors.Where(p =>  p.MaterialName == material.label && p.Title == request)

//                    //.GroupBy(p => p.Color) // Group by the Color property for uniqueness
//                    //.Select(g => g.First()) // Select the first item from each group
//                    .Select(p => new GetDto//p.Title==request &&
//                {
//                    id = p.Id,
//                    label = p.Color,
//                    IsDefault = p.IsDefault
//                }).ToList();
//                bool secondLayerColorsListDefault = secondLayerColorsList.Any(c => c.IsDefault == true);
//                if (!secondLayerColorsListDefault && secondLayerColorsList.Count > 0)
//                {
//                    secondLayerColorsList[0].IsDefault = true;
//                }

//                var SecondLayerInfo = new TwoLayerDto
//                {
//                    id = material.id,
//                    label = material.label,
//                    IsDefault = material.IsDefault,
//                    ColorsList = secondLayerColorsList
//                };
//                secondLayerList.Add(SecondLayerInfo);
//            }
//            //---------------------------------------------------
//            var Colors = _context.MaterialEdgeColors;
//            var ColorsList = Colors.Where(p => p.Title==request).Select(p => new GetDto
//            {
//                id = p.Id,
//                label = p.EdgeColor,
//                IsDefault = p.IsDefault
//            }).ToList();
//            bool ColorsListDefault = ColorsList.Any(c => c.IsDefault == true);
//            if (!ColorsListDefault && ColorsList.Count > 0)
//            {
//                ColorsList[0].IsDefault = true;
//            }
//            //---------------------------------------------------
//            var margins = _context.Margins;
//            var marginsList = margins.Where(p => p.MarginTitle==request).Select(p => new GetDto
//            {
//                id = p.Id,
//                label = p.MarginNumber.ToString(),
//                IsDefault = p.IsDefault
//            }).ToList();
//            bool marginsListDefault = marginsList.Any(c => c.IsDefault == true);
//            if (!marginsListDefault && marginsList.Count > 0)
//            {
//                marginsList[0].IsDefault = true;
//            }
//            //---------------------------------------------------
//            //var powers = _context.Powers;
//            //var powersList = powers.Select(p => new GetDto
//            //{
//            //    id = p.Id,
//            //    label = p.PowerType.ToString()
//            //}).ToList();
//            //---------------------------------------------------
//            var ImplementationModel = _context.EdgeProperties;
//            var ImplementationModelList = ImplementationModel.Where(p => p.EdgeTitle == request).
//                GroupBy(p => p.ImplementationModel)
//                    .Select(g => g.First())
//                    .AsEnumerable()
//                    .Select(p => new GetDto
//                    {
//                        id = p.Id,
//                        label = p.ImplementationModel.ToString(),
//                        IsDefault = p.IsDefault
//                    }).ToList();
//            bool ImplementationModelListDefault = ImplementationModelList.Any(c => c.IsDefault == true);
//            if (!ImplementationModelListDefault && ImplementationModelList.Count > 0)
//            {
//                ImplementationModelList[0].IsDefault = true;
//            }
//            //---------------------------------------------------
//            var smds = _context.Smds;
//            var smdsList = smds.Where(p => p.SmdTitle==request)
//                    .GroupBy(p => p.SmdModel)
//                    .Select(g => g.First())
//                    .AsEnumerable()
//                    .Select((item, index) => new GetDto

//                    {
//                        id = index + 1,
//                        label = item.SmdModel,
//                        IsDefault = item.IsDefault

//                    }).ToList();
//            bool smdsListDefault = smdsList.Any(c => c.IsDefault == true);
//            if (!smdsListDefault && smdsList.Count > 0)
//            {
//                smdsList[0].IsDefault = true;
//            }

//            var zeroColor = smds.Where(p => p.SmdTitle==request && p.SmdModel==ConstantMaterialName.singleColor)
//                    .GroupBy(p => p.SmdColor) // Group by SmdColor to prepare for deduplication
//                    .Select(g => g.First()) // Select the first item of each group, effectively deduplicating based on SmdColor
//                    .AsEnumerable()
//                    .Select((item, index) => new GetDto
//                    {
//                        id = index + 1, // Auto-numbering, starting from 1
//                        label = item.SmdColor,
//                        IsDefault = item.IsDefault

//                    }).ToList();
//            bool zeroColorDefault = zeroColor.Any(c => c.IsDefault == true);
//            if (!zeroColorDefault && zeroColor.Count > 0)
//            {
//                zeroColor[0].IsDefault = true;
//            }

//            var firstColor = smds.Where(p => p.SmdTitle==request && p.SmdModel==ConstantMaterialName.mixedColor)
//                          .GroupBy(p => p.SmdColor)
//                          .Select(g => g.First())
//                          .AsEnumerable() // Switch to client-side evaluation from here onwards
//                          .Select((item, index) => new GetDto
//                          {
//                              id = index + 1, // Auto-numbering, now supported client-side
//                              label = item.SmdColor,
//                              IsDefault = item.IsDefault
//                          }).ToList();
//            bool firstColorDefault = firstColor.Any(c => c.IsDefault == true);
//            if (!firstColorDefault && firstColor.Count > 0)
//            {
//                firstColor[0].IsDefault = true;
//            }
//            var secondColor = smds.Where(p => p.SmdTitle==request && p.SmdModel==ConstantMaterialName.mixedColor)
//                      .GroupBy(p => p.SmdSecondColor) // This ensures that we group by the second color, effectively deduplicating
//                      .Select(g => g.First()) // Take the first item from each group, effectively deduplicating based on SmdSecondColor
//                      .AsEnumerable()
//                      .Select((item, index) => new GetDto
//                      {
//                          id = index + 1, // Auto-numbering, starting from 1
//                          label = item.SmdSecondColor,
//                          IsDefault = item.IsDefault
//                      }).ToList();
//            bool secondColorDefault = secondColor.Any(c => c.IsDefault == true);
//            if (!secondColorDefault && secondColor.Count > 0)
//            {
//                secondColor[0].IsDefault = true;
//            }
//            //---------------------------------------------------
//            var qualityFactorPowers = new List<PowerDto>();

//            var qualityFactors = _context.QualityDegrees
//                .Select(qd => new GetDto
//                {
//                    id = qd.Id,
//                    label = qd.QualityFactor

//                })
//                .ToList();

//            foreach (var factor in qualityFactors)
//            {
//                var powerValuesTemp = _context.Powers
//                    .Where(pv => pv.QualityFactor == factor.label)
//                    .Select(pv => pv.PowerType.ToString()) // First, get the labels
//                    .ToList(); // Materialize the query to work with the data in-memory

//                // Now, project to Power2 with auto-incremented id
//                var powerValues = powerValuesTemp.Select((label, index) => new GetDto
//                {
//                    id = index + 1, // Auto-numbering applied here
//                    label = label
//                }).ToList();

//                var powerDto = new PowerDto
//                {
//                    id = factor.id,
//                    label = factor.label,
//                    subItem = powerValues
//                };

//                qualityFactorPowers.Add(powerDto);
//            }

//            //---------------------------------------------------

//            var resultData = new ResultPLasticGetDto
//            {
//                crystalsList = crystalsList,
//                edgeSizesList = edgeSizesList,
//                ColorsList = ColorsList,
//                FirstLayerColorsList = firstLayerColorsList,
//                SecondLayerList = secondLayerList,
//                marginsList = marginsList,
//                //powersList = powersList, // Assuming you transform it into GetDto
//                smdsList = smdsList,
//                edgePunchsList = edgePunchsList,
//                PunchsList = PunchsList,
//                Color0 = zeroColor,
//                Color1 = firstColor,
//                Color2 = secondColor,
//                QualityFactorPowers = qualityFactorPowers,
//                ImplementationModel = ImplementationModelList
//            };


//            return new ResultDto<ResultPLasticGetDto>
//            {
//                Data = resultData,
//                IsSuccess = true,
//                Message = "Data retrieved successfully"
//            };


//        }



//        public class VirtualDto
//        {
//            public string label { get; set; }


//        }

//        public class ResultPLasticGetDto
//        {
//            public List<GetDto> crystalsList { get; set; }
//            public List<GetDto> edgeSizesList { get; set; }
//            public List<GetDto> edgePunchsList { get; set; }

//            public List<GetDto> ColorsList { get; set; }
//            public List<GetDto> FirstLayerColorsList { get; set; }
//            public List<TwoLayerDto> SecondLayerList { get; set; }
//            //public List<GetDto> PunchModel { get; set; }
//            public List<GetDto> smdsList { get; set; }
//            //public List<GetDto> SmdColor { get; set; }
//            public List<GetDto> marginsList { get; set; }
//            //public List<GetDto> powersList { get; set; }
//            public List<GetDto> PunchsList { get; set; }
//            public List<GetDto> Color0 { get; set; }
//            public List<GetDto> Color1 { get; set; }
//            public List<GetDto> Color2 { get; set; }
//            public List<PowerDto> QualityFactorPowers { get; set; }

//            public List<GetDto> ImplementationModel { get; set; }






//        }

//        public class GetDto
//        {
//            public long id { get; set; }
//            public string label { get; set; }
//            public bool? IsDefault { get; set; }
//        }
//        public class PowerDto
//        {
//            public long id { get; set; }
//            public string label { get; set; }
//            //public GetDto QualityInfo { get; set; }
//            public List<GetDto> subItem { get; set; }
            
//        }
//        public class TwoLayerDto
//        {
//            public long id { get; set; }
//            public string label { get; set; }
//            public List<GetDto> ColorsList { get; set; }
//            public bool? IsDefault { get; set; }
//        }
//        //public class GetPowerDto
//    }
//}