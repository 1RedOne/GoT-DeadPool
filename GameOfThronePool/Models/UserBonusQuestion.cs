using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfThronePool.Models
{
    public class BonusQuestion
    {
        public int UserID { get; set; }
        public bool DanyPreggoStatus { get; set; }
        public string WhoKillsTheNightKing { get; set; }
        public string WhoIsOnTheIronThrone { get; set; }
    }
}
