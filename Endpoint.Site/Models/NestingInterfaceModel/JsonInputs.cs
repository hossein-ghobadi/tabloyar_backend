using Newtonsoft.Json.Linq;

namespace Endpoint.Site.Models.NestingInterfaceModel
{
    public class JsonInputs
    {
        public JObject inputs { get; set; } 
        public JObject NestingResult { get; set; }
    }
}
