using System;

namespace GameOfThronePool.Models
{
    public class UserCharacterSelection
        
    {

        public int UserCharacterSelectionID { get; set; }
        public string UserName { get; set; }
        public int CharacterID { get; set; }
        public string CharacterName { get; set; }
        public bool AliveStatus { get; set; }
        public bool BecomesAWhiteWalker { get; set; }
        public DateTime CreatedDate { get; set; }
        
    }

    public class UserBonusQuestion
    {
        public int UserBonusQuestionID { get; set; }
        public string UserName { get; set; }
        public int QuestionNumber { get; set; }
        public string QuestionText { get; set; }
        public string QuestionAnswer { get; set; }
        public bool Correct { get; set; }

    }
}
