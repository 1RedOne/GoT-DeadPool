using GameOfThronePool.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

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
}
