using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfThronePool.Models
{
    public interface ICharacterRepository : IDisposable
    {
        IEnumerable<ShowCharacterStatusRecord> GetCharacters();
        void StageNewUser(int )
        void Save();
    }
}
