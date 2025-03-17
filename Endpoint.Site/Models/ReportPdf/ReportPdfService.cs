using Endpoint.Site.Models.NestingViewModel.ChannelliumViewModel;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Radin.Application.Services.Product.Commands.PowerCalculation;
using Radin.Common;
using Radin.Domain.Entities.Products;
using Sprache;
using static Endpoint.Site.Models.ReportPdf.Output;

namespace Endpoint.Site.Models.ReportPdf
{
    public class ReportPdfService
    {
        public Output Execute(ChannelliumViewModel request)
        {



            var edgeCheckpoint = false;
            var edgePunchModel = "بدون پانچ";
            var secondLayerCheckpoint = false;
            var secondLayerMaterial = "------";
            var secondLayerColor = "------";
            var firstLayerColor = request.data.modelLayerLetters.one.colorPelekcy.label;
            var firstLayerPunchModel= "بدون پانچ";
            var secondLayerPunchModel= "------";
            var fsmdColor= "------";
            var bsmdColor= "------";
            var fsmdName= "------";
            var bsmdName= "------";
            var edgeColor = request.data.edgeColor.label;
            var CrystalCheckpoint = false;
            var CrystalLocation = "-----";
            var CrystalColor = "-----";
            if (request.data.needPVC.value)
            {
                if (request.data.needPVC.frontLight.value)

                {
                    fsmdName = request.data.needPVC.frontLight.nature.label;
                    if (fsmdName == ConstantMaterialName.singleColor)
                    {
                        fsmdColor = request.data.needPVC.frontLight.color1.label;
                    }
                    else if(fsmdName == ConstantMaterialName.mixedColor)
                    {
                        fsmdColor= $"{request.data.needPVC.frontLight.color1.label}-{request.data.needPVC.frontLight.color2.label}";
                    }

                }
                if (request.data.needPVC.backLight.value)
                {
                    bsmdName = request.data.needPVC.backLight.nature.label;
                    if (bsmdName == ConstantMaterialName.singleColor)
                    {
                        bsmdColor = request.data.needPVC.backLight.color1.label;
                    }
                    else if (bsmdName == ConstantMaterialName.mixedColor)
                    {
                        bsmdColor = $"{request.data.needPVC.backLight.color1.label}-{request.data.needPVC.backLight.color2.label}";
                    }
                }


            }

            if ((request.data.secondEdgeColor==null))
            {
                
            }
            else
            {
                edgeColor = $"{edgeColor}-{request.data.secondEdgeColor.label}";
            }

            if (request.data.needCrystal.value)
            {
                CrystalCheckpoint = true;
                CrystalColor= request.data.needCrystal.color.label;
                CrystalLocation = request.data.needCrystal.location.label;


            }




            if (request.data.isPunch.value)
            {
                edgeCheckpoint=true;
                edgePunchModel = request.data.isPunch.nature.label;
            }






            if (request.data.modelLayerLetters.value.id==2)
            {
                secondLayerCheckpoint = true;
                secondLayerMaterial = request.data.modelLayerLetters.two.layerMaterial.value.label;
                secondLayerColor =  request.data.modelLayerLetters.two.layerMaterial.color.label;
                firstLayerColor = request.data.modelLayerLetters.two.externalColorPelekcy.label;
                if (request.data.modelLayerLetters.two.needPunch.value)
                {
                    firstLayerPunchModel = request.data.modelLayerLetters.two.needPunch.nature.label;


                }
                if(request.data.modelLayerLetters.two.needPunchInternal.value) 
                {
                    secondLayerPunchModel = request.data.modelLayerLetters.two.needPunchInternal.nature.label;


                }
            }
            else
            {
                if (request.data.modelLayerLetters.one.needPunchPelekcy.value)
                {
                    firstLayerPunchModel = request.data.modelLayerLetters.one.needPunchPelekcy.nature.label;


                }
            }






            Output Result = new Output
            {
                Description=request.description,
               
                EdgeCheckpoint=edgeCheckpoint,
                EdgeColor= edgeColor ,
                EdgePunchModel=edgePunchModel,
                EdgeSize=request.data.edgesSize.label,
                SecondLayerCheckpoint=secondLayerCheckpoint,
                SecondLayerMaterial=secondLayerMaterial,
                SecondLayerColor=secondLayerColor,
                FirstLayerColor= firstLayerColor,
                FirstLayerPunchModel=firstLayerPunchModel,
                SecondLayerPunchModel=secondLayerPunchModel,
                PvcBackLightCheckpoint=request.data.PVCHasBackLight.value,
                PowerCheckpoint=request.data.power.value,
                FsmdColor=fsmdColor,
                BsmdColor=bsmdColor,
                FsmdName=fsmdName,
                BsmdName=bsmdName,
                CrystalCheckpoint=CrystalCheckpoint,
                CrystalColor=CrystalColor,
                CrystalLocation=CrystalLocation,
                //FsmdNumber=request.FsmdNumber,
                //BsmdNumber=request.BsmdNumber,
                //PowerData=request.PowersList
            };
           
            return Result;


        }
    }
    public class Output
    {
        public string ProjectName { get; set; }
        public string type { get; set; }    
        public string Description { get; set; }
        public List<string> Images { get; set; } // New field for images
        public bool EdgeCheckpoint { get; set; }
        public string EdgeColor { get; set; }
        public string EdgePunchModel { get; set; }
        public string EdgeSize { get; set; }
        public float EdgeLength { get; set; }
        public bool SecondLayerCheckpoint { get; set; }
        public string SecondLayerMaterial { get; set; }
        public string SecondLayerColor { get; set; }
        public string FirstLayerColor { get; set; }
        public string FirstLayerPunchModel { get; set; }
        public string SecondLayerPunchModel { get; set; }
        public bool PvcBackLightCheckpoint { get; set; }
        public bool CrystalCheckpoint { get; set; }
        public bool PowerCheckpoint { get; set; }

        public float FsmdNumber { get; set; }
        public string FsmdName { get; set; }
        public string FsmdColor { get; set; }
        public float BsmdNumber { get; set; }
        public string BsmdName { get; set; }
        public string BsmdColor { get; set; }
        public List<PowerList> PowerData { get; set; }
        public string QualityFactor {get; set;}
        public string CrystalColor { get; set; }
        public string CrystalLocation { get; set; }
    }



    //    public class Input : ChannelliumViewModel
    //{
    //    public string ProjectName { get; set; }
    //    public int FsmdNumber { get; set; }
    //    public int BsmdNumber { get; set; }
    //    public float PvcLength { get; set;}
    //    public  List<Power> PowersList { get; set; } 

    //}
    // public class Power
    // {
    //     public int PowerType { get; set; }
    //     public int Quantity { get; set; }
    // }
}
