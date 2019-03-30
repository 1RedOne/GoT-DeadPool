using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace GameOfThronePool.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        //[Key]
        //public int UserId { get; set; }
        //[ShowCharacterStatusRecordID] [int] IDENTITY(1,1) NOT NULL,
        //public ICollection<UserCharacterSelection> UserCharacterSelection { get; set; }
    }
}
