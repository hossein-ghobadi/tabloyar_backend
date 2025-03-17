namespace Endpoint.Site.Models.ViewModels.CharacterTypeCalculationModels
{
    public class Question
    {
        public int Qid { get; set; }
        public string Qtext { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();
    }
    public class Answer
    {
        public int Aid { get; set; }
        public string Atext { get; set; }
        public int DScore { get; set; }
        public int IScore { get; set; }
        public int SScore { get; set; }
        public int CScore { get; set; }
    }
}
