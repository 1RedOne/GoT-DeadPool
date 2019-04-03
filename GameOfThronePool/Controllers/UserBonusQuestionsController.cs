using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameOfThronePool.Data;
using GameOfThronePool.Models;

namespace GameOfThronePool.Views.DeadPoolSelection
{
    public class UserBonusQuestionsController : Controller
    {
        private string[] admins = new string[] { "stephen@foxdeploy.com", "sred13@gmail.com" };
        private readonly DeadPoolDBContext _context;

        public UserBonusQuestionsController(DeadPoolDBContext context)
        {
            _context = context;
        }
        
        // GET: UserBonusQuestions
        public async Task<IActionResult> Index()
        {
            string username = HttpContext.User.Identity.Name;
            //string user = UserManager.GetUserName(User);
            if (admins.Contains(username))
            {
                //this user is an admin
                Console.Write("user admin incoming" + username);
                return View(await _context.UserBonusQuestion.ToListAsync());
            }
            else
            {
                Console.Write("standard user" + username);
                return RedirectToAction("MyQuestions");
            }

        }

        public async Task<IActionResult> MyQuestions()
        {
            string username = HttpContext.User.Identity.Name;
            return View(await _context.UserBonusQuestion.AsQueryable().
                    Where(m => m.UserName.Equals(username)).ToListAsync());
        }

        // GET: UserBonusQuestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBonusQuestion = await _context.UserBonusQuestion
                .FirstOrDefaultAsync(m => m.UserBonusQuestionID == id);
            if (userBonusQuestion == null)
            {
                return NotFound();
            }

            return View(userBonusQuestion);
        }

        // GET: UserBonusQuestions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserBonusQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserBonusQuestionID,UserName,QuestionNumber,QuestionText,QuestionAnswer,Correct")] UserBonusQuestion userBonusQuestion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userBonusQuestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userBonusQuestion);
        }

        // GET: UserBonusQuestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBonusQuestion = await _context.UserBonusQuestion.FindAsync(id);
            if (userBonusQuestion == null)
            {
                return NotFound();
            }
            return View(userBonusQuestion);
        }

        // POST: UserBonusQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserBonusQuestionID,UserName,QuestionNumber,QuestionText,QuestionAnswer,Correct")] UserBonusQuestion userBonusQuestion)
        {
            if (id != userBonusQuestion.UserBonusQuestionID)
            {
                return NotFound();
            }
            string username = HttpContext.User.Identity.Name;
            if (admins.Contains(username) | username.Equals(userBonusQuestion.UserName))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(userBonusQuestion);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserBonusQuestionExists(userBonusQuestion.UserBonusQuestionID))
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
            }
            else
            {
                return Unauthorized();
            }
            return View(userBonusQuestion);
        }

        // GET: UserBonusQuestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBonusQuestion = await _context.UserBonusQuestion
                .FirstOrDefaultAsync(m => m.UserBonusQuestionID == id);
            if (userBonusQuestion == null)
            {
                return NotFound();
            }

            return View(userBonusQuestion);
        }

        // POST: UserBonusQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userBonusQuestion = await _context.UserBonusQuestion.FindAsync(id);
            _context.UserBonusQuestion.Remove(userBonusQuestion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserBonusQuestionExists(int id)
        {
            return _context.UserBonusQuestion.Any(e => e.UserBonusQuestionID == id);
        }
    }
}
