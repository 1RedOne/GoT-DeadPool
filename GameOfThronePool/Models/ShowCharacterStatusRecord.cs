using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfThronePool.Models
{
    public class ShowCharacterStatusRecord
    {   
        public int ShowCharacterStatusRecordID { get; set; }
        [Display(Name = "Character Name")]
        public string CharacterName { get; set; }

        [Display(Name = "Alive or Dead?")]
        public bool AliveStatus { get; set; }

        [Display(Name = "Are they a White Walker?")]
        public bool WhiteWalkerStatus { get; set; }

        [Display(Name = "When did they die?")]
        public int? CharacterDiedEpisodeNo { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
