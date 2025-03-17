
namespace Endpoint.Site.Models.NestingViewModel.ChannelliumViewModel
{
    public class modelLayerLetters
    {
        public one? one { get; set; }    
        public two? two { get; set; }
        public Base? value { get; set; }
    }






    public class one
    {  
        public Base? colorPelekcy { get; set; }
        public needPunchContent? needPunchPelekcy { get; set; }
    }
    public class two
    {
        public Base? externalColorPelekcy { get; set; }
        public Content2? layerMaterial { get; set; }

        public needPunchContent? needPunch { get; set; }
        public needPunchContent? needPunchInternal { get; set; }

    }






    public class Base
    {
        public bool IsDefault { get; set; }

        public int id { get; set; }

        public string label { get; set; }
    }
  
    public class number
    {
        public int id { get; set; }
    }






    public class Content
    {
        public Base? nature { get; set; }
    }
    public class Content2
    {
        public Base value { get; set; }
        public Base? color { get; set; }
    }

    public class needPunchContent : Content
    {
        public bool value { get; set; }
    }
}
