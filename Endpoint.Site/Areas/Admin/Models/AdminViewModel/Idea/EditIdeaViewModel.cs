namespace Endpoint.Site.Areas.Admin.Models.AdminViewModel.Idea
{
    public class EditIdeaViewModel
    {
        
         public long id { get; set; }
         public string category { get; set; }
         public bool commentable { get; set; }
         public List<string> images { get; set; }
         public string imageDesc { get; set; }
         public string main { get; set; }
         public string mainImage { get; set; }  
         public bool published { get; set; }
         public string meta { get; set; }
         public bool showComment { get; set; }
         public int sort { get; set; }
         public string title { get; set; }
         public string url { get; set; }
         public bool IsIndex { get; set; }
    }
}
