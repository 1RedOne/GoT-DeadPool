using GameOfThronePool.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfThronePool.Models
{
    public class CharacterRecordRepository : ICharacterRepository
    {

        private readonly DeadPoolDBContext _context;
        
        /*public CharacterRecordRepository(DeadPoolDBContext context)
        {
            _context = context;
        }
        */

        public CharacterRecordRepository() { }
        
        public IEnumerable<ShowCharacterStatusRecord> GetCharacters()
        {
            return _context.ShowCharacterStatusRecord.ToList();
        }

        public void StageNewUser(string UserName)
        {
            List<ShowCharacterStatusRecord> allCharacters = GetCharacters().ToList();
            foreach (ShowCharacterStatusRecord character in allCharacters)
            {
                UserCharacterSelection newUserCharacterSelection = new UserCharacterSelection();
                newUserCharacterSelection.AliveStatus = true;
                newUserCharacterSelection.BecomesAWhiteWalker = false;
                newUserCharacterSelection.CharacterName = character.CharacterName;
                newUserCharacterSelection.UserName = UserName;

                _context.UserCharacterSelection.Add(newUserCharacterSelection);
            }
            _context.SaveChanges();
            return;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
