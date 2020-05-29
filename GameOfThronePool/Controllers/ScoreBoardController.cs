using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameOfThronePool.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace GameOfThronePool.Models
{
    public class ScoreBoardController : Controller
    {
        private readonly DeadPoolDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private string[] admins = new string[] { "stephen@foxdeploy.com", "sred13@gmail.com" };
        string[] UsersNotToDisplay = new string[] { "sred13@foxdeploy.com", "stephenowenii@gmail.com" };
        public ScoreBoardController(DeadPoolDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        // GET: ScoreBoard
        public async Task<IActionResult> Index()
        {
            ViewBag.IsPrivileged = false;
            string username = HttpContext.User.Identity.Name;
            //string user = UserManager.GetUserName(User);
            if (admins.Contains(username))
            {
                ViewBag.IsPrivileged = true;
            }

            if (!_context.UserScoreRecord.Any())
            {
                StageScoreBoard(); 
            }

            string cookieValueFromContext = HttpContext.Request.Cookies["UserSeenModal"];
            //read cookie from Request object  
            string cookieValueFromReq = Request.Cookies["UserSeenModal"];

            if (cookieValueFromReq != null)
            {
                ViewBag.ShowModal = false;
            }
            else
            {
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(12);
                Response.Cookies.Append("UserSeenModal", "true", option);
                ViewBag.ShowModal = true;
            }
            
            return View(await _context.UserScoreRecord.Where(m=>!UsersNotToDisplay.Contains(m.UserName))
                .ToListAsync());
            
        }

        [Authorize]
        public async Task<IActionResult> Update()
        {
            string username = HttpContext.User.Identity.Name;
            //string user = UserManager.GetUserName(User);
            if (!admins.Contains(username))
            {
                return Unauthorized();
            }            
            return View(await _context.UserScoreRecord.ToListAsync());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateScores()
        {
            string username = HttpContext.User.Identity.Name;
            //string user = UserManager.GetUserName(User);
            if (!admins.Contains(username))
            {
                return Unauthorized();
            }
            //todo update and reward for proper whitewalker guesses
            List<CorrectAnswers> Correct = _context.CorrectAnswers.FromSqlRaw(@"select UserName,count(distinct ShowCharacterStatusRecordID) as 'MatchingAnswers' 
                    from UserCharacterSelection as USelection
                    join ShowCharacterStatusRecord as SHOW on SHOW.CharacterName = USelection.CharacterName and SHow.AliveStatus = USelection.AliveStatus
                    group by UserName ").ToList();

            List<wrongWhiteWalkers> wrongWW= _context.wrongWhiteWalkers.FromSqlRaw(@"select UserName,count(distinct ShowCharacterStatusRecordID) as 'WrongWhiteWalkers' 
                    from UserCharacterSelection as USelection
                    join ShowCharacterStatusRecord as SHOW on SHOW.CharacterName =USelection.CharacterName and SHow.WhiteWalkerStatus != USelection.BecomesAWhiteWalker
                    where USelection.BecomesAWhiteWalker = 1
                    group by UserName").ToList();
            List<rightWhiteWalkers> rightWW = _context.rightWhiteWalkers.FromSqlRaw(@"select UserName, count(distinct ShowCharacterStatusRecordID) as 'RightWhiteWalkers'
                    from UserCharacterSelection as USelection
                    join ShowCharacterStatusRecord as SHOW on SHOW.CharacterName = USelection.CharacterName and SHow.WhiteWalkerStatus = USelection.BecomesAWhiteWalker
                    where USelection.BecomesAWhiteWalker = 1
                    group by UserName").ToList();

            List<BonusQuestions> bonus = _context.BonusQuestions.FromSqlRaw(@"select UserName, QuestionNumber 
                    from UserBonusQuestion 
                    where Correct = 1
                    group by UserName, QuestionNumber ").ToList();

            foreach (UserScoreRecord user in _context.UserScoreRecord.ToList())
            {
                user.BonusScore = 0;
                user.BaseScore = 0;
                user.TotalScore = 0;

                var thisUserBase = Correct.Where(m => m.UserName.Equals(user.UserName)).FirstOrDefault();
                var thisUserWrongWhiteWalker = wrongWW.Where(m => m.UserName.Equals(user.UserName)).FirstOrDefault();
                var thisUserRightWhiteWalker = rightWW.Where(m => m.UserName.Equals(user.UserName)).FirstOrDefault();
                List<BonusQuestions> thisUserBonus = bonus.Where(m => m.UserName.Equals(user.UserName)).ToList();
                //string msg = "updating: " + user.UserFriendlyName;
                int score = thisUserBase.MatchingAnswers;


                user.BaseScore = (thisUserWrongWhiteWalker == null) ? score : score - thisUserWrongWhiteWalker.WrongWhiteWalkers;
                user.BaseScore = (thisUserRightWhiteWalker == null) ? score : score + thisUserRightWhiteWalker.RightWhiteWalkers;

                if (thisUserBonus != null)
                {
                    foreach (BonusQuestions b in thisUserBonus)
                    {
                        if (b.QuestionNumber == 1) { user.BonusScore += 1; }
                        if (b.QuestionNumber == 2) { user.BonusScore += 2; }
                        if (b.QuestionNumber == 3) { user.BonusScore += 4; }                        
                    }
                }

                user.TotalScore = user.BonusScore + score;
                user.CreatedDate = DateTime.Now;
                _context.SaveChanges();
            }

            TempData["Message"] = "Updated scores...";
            return RedirectToAction("Update");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Restage()
        {
            string username = HttpContext.User.Identity.Name;
            //string user = UserManager.GetUserName(User);
            if (!admins.Contains(username))
            {
                return Unauthorized();
            }
            StageScoreBoard();
            TempData["Message"] = "Restaged Scores...";
            return RedirectToAction("Update");
        }

        [Authorize]
        public void StageScoreBoard()
        {
            var users = _userManager.Users.ToList();
            
            foreach (var user in users)
            {
                //check for a score record, if exists, display, if not, create
                var userScoreRecord = _context.UserScoreRecord
                .FirstOrDefault(m => m.UserFriendlyName == user.UserFriendlyName);
                if (userScoreRecord == null)
                {
                    userScoreRecord = new UserScoreRecord
                    {
                        UserFriendlyName = user.UserFriendlyName,
                        CreatedDate = DateTime.Now
                    };

                    _context.UserScoreRecord.Add(userScoreRecord);
                    _context.SaveChanges();
                }

                //check for null username and populate
                if (userScoreRecord.UserName == null)
                {
                    userScoreRecord.UserName = user.UserName;
                    _context.SaveChanges();
                }
            }
        }

        [Authorize]
        // GET: ScoreBoard/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            string username = HttpContext.User.Identity.Name;
            //string user = UserManager.GetUserName(User);
            if (!admins.Contains(username))
            {
                return Unauthorized();
            }            

            if (id == null)
            {
                return NotFound();
            }

            var userScoreRecord = await _context.UserScoreRecord
                .FirstOrDefaultAsync(m => m.UserScoreRecordID == id);
            if (userScoreRecord == null)
            {
                return NotFound();
            }

            return View(userScoreRecord);
        }

        //[Authorize]
        //// GET: ScoreBoard/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: ScoreBoard/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("UserScoreRecordID,UserFriendlyName,BaseScore,BonusScore,TotalScore,CreatedDate")] UserScoreRecord userScoreRecord)
        //{
        //    string username = HttpContext.User.Identity.Name;
        //    //string user = UserManager.GetUserName(User);
        //    if (!admins.Contains(username))
        //    {
        //        return Unauthorized();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(userScoreRecord);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(userScoreRecord);
        //}

        // GET: ScoreBoard/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            string username = HttpContext.User.Identity.Name;
            if (!admins.Contains(username))
            {
                return Unauthorized();
            }
            if (id == null)
            {
                return NotFound();
            }

            var userScoreRecord = await _context.UserScoreRecord.FindAsync(id);
            if (userScoreRecord == null)
            {
                return NotFound();
            }
            return View(userScoreRecord);
        }

        // POST: ScoreBoard/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserScoreRecordID,UserFriendlyName,BaseScore,BonusScore,TotalScore,CreatedDate")] UserScoreRecord userScoreRecord)
        {
            string username = HttpContext.User.Identity.Name;
            //string user = UserManager.GetUserName(User);
            if (!admins.Contains(username))
            {
                return Unauthorized();
            }

            if (id != userScoreRecord.UserScoreRecordID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userScoreRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserScoreRecordExists(userScoreRecord.UserScoreRecordID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userScoreRecord);
        }

        // GET: ScoreBoard/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userScoreRecord = await _context.UserScoreRecord
                .FirstOrDefaultAsync(m => m.UserScoreRecordID == id);
            if (userScoreRecord == null)
            {
                return NotFound();
            }

            return View(userScoreRecord);
        }
        [Authorize]
        // POST: ScoreBoard/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userScoreRecord = await _context.UserScoreRecord.FindAsync(id);
            _context.UserScoreRecord.Remove(userScoreRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserScoreRecordExists(int id)
        {
            return _context.UserScoreRecord.Any(e => e.UserScoreRecordID == id);
        }
        
    }
}
