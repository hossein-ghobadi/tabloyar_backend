using Radin.Application.Interfaces.Contexts;
using Radin.Application.Interfaces.FacadPatterns;
using Radin.Application.Services.Excelloading;
using Radin.Application.Services.Product.Commands.Mapping;
using Radin.Application.Services.Product.Commands.PowerCalculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Application.Services.Excelloading.FacadPattern
{
    public class ExcelLoadingFacad : IExcelLoadingFacad
    {
        //private readonly ExcelHelper _excelHelper;

        public ExcelLoadingFacad(
            //ExcelHelper excelHelper

            )

        {
            //_excelHelper = excelHelper;

        }



        private ColorCostsLoading _colorCostsLoading;
        public ColorCostsLoading ColorCostsLoading
        {
            get
            {
                return _colorCostsLoading = _colorCostsLoading ?? new ColorCostsLoading();
            }
        }
        //________________________________________________________________________________

        private CrystalsLoading _crystalsLoading;
        public CrystalsLoading CrystalsLoading
        {
            get
            {
                return _crystalsLoading = _crystalsLoading ?? new CrystalsLoading();
            }
        }
        //________________________________________________________________________________

        private EdgePropertiesLoading _edgePropertiesLoading;
        public EdgePropertiesLoading EdgePropertiesLoading
        {
            get
            {
                return _edgePropertiesLoading = _edgePropertiesLoading ?? new EdgePropertiesLoading();
            }
        }
        //________________________________________________________________________________


        private EdgePunchsLoading _edgePunchsLoading;
        public EdgePunchsLoading EdgePunchsLoading
        {
            get
            {
                return _edgePunchsLoading = _edgePunchsLoading ?? new EdgePunchsLoading();
            }
        }
        //________________________________________________________________________________

        private GluesLoading _gluesLoading;
        public GluesLoading GluesLoading
        {
            get
            {
                return _gluesLoading = _gluesLoading ?? new GluesLoading();
            }
        }
        //________________________________________________________________________________

        private MarginsLoading _marginsLoading;
        public MarginsLoading MarginsLoading
        {
            get
            {
                return _marginsLoading = _marginsLoading ?? new MarginsLoading();
            }
        }
        //________________________________________________________________________________

        private MaterialEdgeColorsLoading _materialEdgeColorsLoading;
        public MaterialEdgeColorsLoading MaterialEdgeColorsLoading
        {
            get
            {
                return _materialEdgeColorsLoading = _materialEdgeColorsLoading ?? new MaterialEdgeColorsLoading();
            }
        }
        //________________________________________________________________________________

        private MaterialEdgeSizesLoading _materialEdgeSizesLoading;
        public MaterialEdgeSizesLoading MaterialEdgeSizesLoading
        {
            get
            {
                return _materialEdgeSizesLoading = _materialEdgeSizesLoading ?? new MaterialEdgeSizesLoading();
            }
        }
        //________________________________________________________________________________

        private MaterialsLoading _materialsLoading;
        public MaterialsLoading MaterialsLoading
        {
            get
            {
                return _materialsLoading = _materialsLoading ?? new MaterialsLoading();
            }
        }
        //________________________________________________________________________________

        private PowersLoading _powersLoading;
        public PowersLoading PowersLoading
        {
            get
            {
                return _powersLoading = _powersLoading ?? new PowersLoading();
            }
        }
        //________________________________________________________________________________

        private PunchsLoading _punchsLoading;
        public PunchsLoading PunchsLoading
        {
            get
            {
                return _punchsLoading = _punchsLoading ?? new PunchsLoading();
            }
        }
        //________________________________________________________________________________


        private QualityDegreeLoading _qualityDegreeLoading;
        public QualityDegreeLoading QualityDegreeLoading
        {
            get
            {
                return _qualityDegreeLoading = _qualityDegreeLoading ?? new QualityDegreeLoading();
            }
        }
        //________________________________________________________________________________



        private SmdsLoading _smdsLoading;
        public SmdsLoading SmdsLoading
        {
            get
            {
                return _smdsLoading = _smdsLoading ?? new SmdsLoading();
            }
        }
        //________________________________________________________________________________




        private MaterialColorsLoading _materialColorsLoading;
        public MaterialColorsLoading MaterialColorsLoading
        {
            get
            {
                return _materialColorsLoading = _materialColorsLoading ?? new MaterialColorsLoading();
            }
        }
        //________________________________________________________________________________





        private SecondLayerMaterialLoading _secondLayerMaterialLoading;
        public SecondLayerMaterialLoading SecondLayerMaterialLoading
        {
            get
            {
                return _secondLayerMaterialLoading = _secondLayerMaterialLoading ?? new SecondLayerMaterialLoading();
            }
        }
        //________________________________________________________________________________

        private TitlesLoading _titlesLoading;
        public TitlesLoading TitlesLoading
        {
            get
            {
                return _titlesLoading = _titlesLoading ?? new TitlesLoading();
            }
        }
        //________________________________________________________________________________








    }

}