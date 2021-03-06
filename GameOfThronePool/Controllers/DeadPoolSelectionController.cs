﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameOfThronePool.Data;
using GameOfThronePool.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace GameOfThronePool.Views.DeadPool
{
    public class DeadPoolSelectionController : Controller
    {
        private readonly DeadPoolDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private string[] admins = new string[] { "stephen@foxdeploy.com", "sred13@gmail.com" };
        public DeadPoolSelectionController(DeadPoolDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        //internal methods
        public IEnumerable<ShowCharacterStatusRecord> GetAllCharacters()
        {
            return _context.ShowCharacterStatusRecord.ToList();
        }
        public async Task<JsonResult> SetStatusForCharAsync(int id, string Property, bool Value)
        {
            string username = HttpContext.User.Identity.Name;

            if (!(admins.Contains(username))){
                return Json(new { status = "blocked, registrations are closed!" });
            }

            var userCharacterSelection = await _context.UserCharacterSelection.FindAsync(id);
            
            if (userCharacterSelection == null)
            {
                return Json(new
                {
                    status = "recordNotFound"                    
                });
            }
            if (userCharacterSelection.UserName != username) {
                return Json(new { status = "Cannot edit someone elses record",
                    youAre = username,
                    recordBelongsTo = userCharacterSelection.UserName,
                    InputProperty = Property,
                    InputValue = Value,
                    InputID = id
                    }
                );
            }else
            {
                //if toggling the alive state
                if (Property ==  "AliveStatus")
                {
                    userCharacterSelection.AliveStatus = Value;                    
                }

                //if toggling the white walker state
                if (Property == "BecomesAWhiteWalker")
                {
                    userCharacterSelection.BecomesAWhiteWalker = Value;
                }

                try
                {
                    _context.Update(userCharacterSelection);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    return Json(NotFound());
                }
                return Json(userCharacterSelection);
            }
            /*if (HttpContext.User)

            return Json(userCharacterSelection);*/
            
        }


        public void StageNewUser(string UserName)
        {
            List<ShowCharacterStatusRecord> allCharacters = GetAllCharacters().ToList();
            foreach (ShowCharacterStatusRecord character in allCharacters)
            {
                UserCharacterSelection newUserCharacterSelection = new UserCharacterSelection();
                newUserCharacterSelection.AliveStatus = true;
                newUserCharacterSelection.BecomesAWhiteWalker = false;
                newUserCharacterSelection.CharacterName = character.CharacterName;
                newUserCharacterSelection.UserName = UserName;
                newUserCharacterSelection.CreatedDate = DateTime.Now;

                _context.UserCharacterSelection.Add(newUserCharacterSelection);
            }
            _context.SaveChanges();
            return;
        }

        public void StageNewUserQuestions(string UserName)
        {
            UserBonusQuestion newUserBonusQuestion = new UserBonusQuestion
            {
                UserName = UserName,
                QuestionNumber = 1,
                QuestionText = "Is Daenerys (Khaleesi) pregnant?",
                QuestionAnswer = null
            };

            _context.UserBonusQuestion.Add(newUserBonusQuestion);

            newUserBonusQuestion = new UserBonusQuestion
            {
                UserName = UserName,
                QuestionNumber = 2,
                QuestionText = "Who kills the Night King?",
                QuestionAnswer = null
            };

            _context.UserBonusQuestion.Add(newUserBonusQuestion);

            newUserBonusQuestion = new UserBonusQuestion
            {
                UserName = UserName,
                QuestionNumber = 3,
                QuestionText = "Who will hold the Iron Throne at the end?",
                QuestionAnswer = null
            };

            _context.UserBonusQuestion.Add(newUserBonusQuestion);

            _context.SaveChanges();
            return;
        }
        [HttpPost]
        [Authorize]
        public ActionResult Restage()
        {
            var users = _userManager.Users.ToList();
            int Restaged = 0;
            foreach (var user in users)
            {
                List<UserCharacterSelection> userRecords =  _context.UserCharacterSelection.AsQueryable().
                Where(m => m.UserName.Equals(user.UserName)).ToList();

                List<UserBonusQuestion> userQuestions = _context.UserBonusQuestion.AsQueryable().
                    Where(m => m.UserName.Equals(user.UserName)).ToList();
                if (!userRecords.Any())
                {
                    //found no records
                    StageNewUser(user.UserName);
                    Restaged = 1;
                }

                if (!userQuestions.Any())
                {
                    //found no records
                    StageNewUserQuestions(user.UserName);
                    Restaged = 1;
                }

            }
            if (Restaged == 1)
            {
                TempData["Message"] = "Created missing Records for users...";
            }            
            return RedirectToAction("Update", "ScoreBoard");
        }

        [HttpGet]
        [Authorize]
        // GET: DeadPoolSelection
        public async Task<IActionResult> Index(List<UserCharacterSelection> userRecords)
        {
            string username = HttpContext.User.Identity.Name;
            userRecords = await _context.UserCharacterSelection.AsQueryable().
                Where(m => m.UserName.Equals(username)).ToListAsync();

            List<UserBonusQuestion> userQuestions = await _context.UserBonusQuestion.AsQueryable().
                Where(m => m.UserName.Equals(username)).ToListAsync();
            if (userRecords.Any()){
                //found records
                Console.WriteLine("found records");
            }
            else
            {
                //found no records
                StageNewUser(username);
                Console.WriteLine("setting up user records");
                userRecords = await _context.UserCharacterSelection.AsQueryable().
                                Where(m => m.UserName.Equals(username)).ToListAsync();
            }

            if (userQuestions.Any())
            {
                //found records
                Console.WriteLine("found questions records");
            }
            else
            {
                //found no records
                StageNewUserQuestions(username);
                Console.WriteLine("setting up user records");
                userQuestions = await _context.UserBonusQuestion.AsQueryable().
                    Where(m => m.UserName.Equals(username)).ToListAsync();
            }

            return View(userRecords);
        }

        [HttpGet]
        [Authorize]
        // GET: DeadPoolSelection
        public async Task<IActionResult> ViewOnly(int UserScoreRecordID)
        {
            var UserScoreRecord = _context.UserScoreRecord.AsQueryable().
                Where(m => m.UserScoreRecordID.Equals(UserScoreRecordID)).FirstOrDefault();
            string username = UserScoreRecord.UserName;
            string friendlyName = UserScoreRecord.UserFriendlyName;
            List<UserCharacterSelection> userRecords = await _context.UserCharacterSelection.AsQueryable().
                Where(m => m.UserName.Equals(username)).ToListAsync();
            ViewBag.FriendlyName = friendlyName;
            return View(userRecords);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostUpdate(ICollection<UserCharacterSelection> UserCharacterSelections, IFormCollection form)
        {
            int itemCount = form["item.UserCharacterSelectionID"].ToString().Split(",").Count();
            if (ModelState.IsValid)
            {
                for (int i = 0; i < itemCount; i++)
                {
                    int thisID = Convert.ToInt32(form["item.UserCharacterSelectionID"].ToString().Split(",")[i]);
                    UserCharacterSelection userCharacterSelection = _context.UserCharacterSelection.Where(m => thisID.Equals(m.UserCharacterSelectionID)).FirstOrDefault();
                    //to do, populate and update fields which changed
                    userCharacterSelection.CreatedDate = DateTime.Now;
                    userCharacterSelection.AliveStatus = (form["item.AliveStatus"].ToString().Split(",")[i] == "true") ? true : false;
                    userCharacterSelection.BecomesAWhiteWalker = (form["item.BecomesAWhiteWalker"].ToString().Split(",")[i] == "true") ? true : false;
                    //to do, add cooler checkboxes back

                    //use this syntax to update!
                    //_context.Update(userCharacterSelection);
                    //await _context.SaveChangesAsync();
                }

                foreach (UserCharacterSelection record in UserCharacterSelections)
                {
                    System.Diagnostics.Debug.WriteLine("updating status of character " + record.CharacterName);
                }
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        // GET: DeadPoolSelection/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCharacterSelection = await _context.UserCharacterSelection
                .FirstOrDefaultAsync(m => m.UserCharacterSelectionID == id);
            if (userCharacterSelection == null)
            {
                return NotFound();
            }

            return View(userCharacterSelection);
        }
        [Authorize]
        // GET: DeadPoolSelection/Create
        public IActionResult Create()
        {
            return View();
        }
        /*
        [Authorize]
        // POST: DeadPoolSelection/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,UserCharacterSelectionID,CharacterID,CharacterName,AliveStatus,BecomesAWhiteWalker,CreatedDate")] UserCharacterSelection userCharacterSelection)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userCharacterSelection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userCharacterSelection);
        }
        [Authorize]
        // GET: DeadPoolSelection/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCharacterSelection = await _context.UserCharacterSelection.FindAsync(id);
            if (userCharacterSelection == null)
            {
                return NotFound();
            }
            return View(userCharacterSelection);
        }

        // POST: DeadPoolSelection/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserName,UserCharacterSelectionID,CharacterID,CharacterName,CreatedDate")] UserCharacterSelection userCharacterSelection, IFormCollection form)
        {
            if (id != userCharacterSelection.UserCharacterSelectionID)
            {
                return NotFound();
            }
            userCharacterSelection.AliveStatus = (form["AliveStatus"] == "on") ? true : false;
            userCharacterSelection.BecomesAWhiteWalker = (form["BecomesAWhiteWalker"] == "on") ? true : false;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userCharacterSelection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserCharacterSelectionExists(userCharacterSelection.UserCharacterSelectionID))
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
            return View(userCharacterSelection);
        }
        /*
        // GET: DeadPoolSelection/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCharacterSelection = await _context.UserCharacterSelection
                .FirstOrDefaultAsync(m => m.UserCharacterSelectionID == id);
            if (userCharacterSelection == null)
            {
                return NotFound();
            }

            return View(userCharacterSelection);
        }

        // POST: DeadPoolSelection/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userCharacterSelection = await _context.UserCharacterSelection.FindAsync(id);
            _context.UserCharacterSelection.Remove(userCharacterSelection);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */
        private bool UserCharacterSelectionExists(int id)
        {
            return _context.UserCharacterSelection.Any(e => e.UserCharacterSelectionID == id);
        }
    }
}
