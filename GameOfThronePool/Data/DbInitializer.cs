using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameOfThronePool.Models;

namespace GameOfThronePool.Data
{
    public static class DbInitializer
    {
        public static void Initialize(DeadPoolDBContext context)
        {
            context.Database.EnsureCreated();

            ///* Look for any Show Records.
            if (context.ShowCharacterStatusRecord.Any())
            {
                return;   // DB has been seeded
            }

            var records = new ShowCharacterStatusRecord[]
            {
            new ShowCharacterStatusRecord{CharacterName="Jon Snow",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Sansa Stark",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Arya Stark",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Bran Stark",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Cersei Lannister",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Jaime Lannister",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Tyrion Lannister",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Daenerys Targaryen",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Yara Greyjoy",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Theon Greyjoy",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Euron Greyjoy",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Melisandre",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Jorah Mormont",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Yara Greyjoy",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="The Hound",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="The Mountain",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Samwell Tarley",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Gilly",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Baby Sam",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Lord Varys",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Brienne of Tarth",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Yara Greyjoy",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Ser Davos Seaworth",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Bronn",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Podrick Payne",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Tormund Giantsbane",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Grey Worm",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Gendry Baratheon",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now },
            new ShowCharacterStatusRecord{CharacterName="Beric Dondarrion",AliveStatus=true,WhiteWalkerStatus=false,CharacterDiedEpisodeNo=null,CreatedDate=DateTime.Now }
            



            };
            foreach (ShowCharacterStatusRecord r in records)
            {
                context.ShowCharacterStatusRecord.Add(r);
            }
            context.SaveChanges();
            /*
            var courses = new Course[]
            {
            new Course{CourseID=1050,Title="Chemistry",Credits=3},
            new Course{CourseID=4022,Title="Microeconomics",Credits=3},
            new Course{CourseID=4041,Title="Macroeconomics",Credits=3},
            new Course{CourseID=1045,Title="Calculus",Credits=4},
            new Course{CourseID=3141,Title="Trigonometry",Credits=4},
            new Course{CourseID=2021,Title="Composition",Credits=3},
            new Course{CourseID=2042,Title="Literature",Credits=4}
            };
            foreach (Course c in courses)
            {
                context.Courses.Add(c);
            }
            context.SaveChanges();

            var enrollments = new Enrollment[]
            {
            new Enrollment{StudentID=1,CourseID=1050,Grade=Grade.A},
            new Enrollment{StudentID=1,CourseID=4022,Grade=Grade.C},
            new Enrollment{StudentID=1,CourseID=4041,Grade=Grade.B},
            new Enrollment{StudentID=2,CourseID=1045,Grade=Grade.B},
            new Enrollment{StudentID=2,CourseID=3141,Grade=Grade.F},
            new Enrollment{StudentID=2,CourseID=2021,Grade=Grade.F},
            new Enrollment{StudentID=3,CourseID=1050},
            new Enrollment{StudentID=4,CourseID=1050},
            new Enrollment{StudentID=4,CourseID=4022,Grade=Grade.F},
            new Enrollment{StudentID=5,CourseID=4041,Grade=Grade.C},
            new Enrollment{StudentID=6,CourseID=1045},
            new Enrollment{StudentID=7,CourseID=3141,Grade=Grade.A},
            };
            foreach (Enrollment e in enrollments)
            {
                context.Enrollments.Add(e);
            }
            context.SaveChanges();
            */
        }
    }
}
