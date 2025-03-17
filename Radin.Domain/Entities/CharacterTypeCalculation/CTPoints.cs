using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radin.Domain.Entities.CharacterTypeCalculation
{
    public class CTPoints
    {
        public int Id {  get; set; }
        public int QuestionId { get; set; }
        public int QuestionText { get; set; }
        public int AnswerId { get; set; }
        public int AnswerText { get; set; }
        public int Dpoint {  get; set; }

    }
}
