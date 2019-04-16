using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameOfThronePool.Models
{
    public class UserScoreRecord
    {
            public int UserScoreRecordID { get; set; }
            [Display(Name = "Users Name")]
            public string UserFriendlyName { get; set; }
            public string UserName { get; set; }

        [Display(Name = "Points from DeathPool")]
            public int? BaseScore { get; set; }

            [Display(Name = "Points from Bonus Questions")]
            public int? BonusScore { get; set; }

            [Display(Name = "Total Score")]
            public int? TotalScore { get; set; }
            public DateTime CreatedDate { get; set; }
        }

    [NotMapped]
    public class CorrectAnswers
    {        
        public string UserName { get; set; }
        public int MatchingAnswers { get; set; }
    }

    [NotMapped]
    public class wrongWhiteWalkers
    {
        public string UserName { get; set; }
        public int WrongWhiteWalkers { get; set; }
    }

    [NotMapped]
    public class rightWhiteWalkers
    {
        public string UserName { get; set; }
        public int RightWhiteWalkers { get; set; }
    }

    [NotMapped]
    public class BonusQuestions
    {
        public string UserName { get; set; }
        public int QuestionNumber { get; set; }
    }

}
