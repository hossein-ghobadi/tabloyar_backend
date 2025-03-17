namespace Endpoint.Site.Areas.Admin.Models.AdminViewModel.Content
{
    public class EditContentViewModel
    {
        public long id { get; set; }
        public string category { get; set; }
        public bool commentable { get; set; }
        public string image { get; set; }
        public string imageDesc { get; set; }
        public string? imageTitle { get; set; } = "";
        public string main { get; set; }
        public bool published { get; set; }
        public string meta { get; set; }
        public bool showComment { get; set; }
        public int sort { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string canonical { get; set; }
        public bool IsIndex { get; set; }
    }
}
