using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfThronePool.Models
{
    public class ShowCharacterStatusRecord
    {   
        public int ShowCharacterStatusRecordID { get; set; }
        public string CharacterName { get; set; }
        public bool AliveStatus { get; set; }
        public bool WhiteWalkerStatus { get; set; }
        public int? CharacterDiedEpisodeNo { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
