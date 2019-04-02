using System;
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

namespace GameOfThronePool.Views.DeadPool
{
    public class DeadPoolSelectionController : Controller
    {
        private readonly DeadPoolDBContext _context;

        public DeadPoolSelectionController(DeadPoolDBContext context)
        {
            _context = context;           
        }
        //internal methods
        public IEnumerable<ShowCharacterStatusRecord> GetAllCharacters()
        {
            return _context.ShowCharacterStatusRecord.ToList();
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
        [HttpGet]
        [Authorize]
        // GET: DeadPoolSelection
        public async Task<IActionResult> Index(List<UserCharacterSelection> userRecords)
        {
            string username = HttpContext.User.Identity.Name;
            userRecords = await _context.UserCharacterSelection.AsQueryable().
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
            return View(userRecords);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostUpdate(List<UserCharacterSelection> changedRecords, IFormCollection form)
        {
            int itemCount = form["item.UserCharacterSelectionID"].ToString().Split(",").Count();
            if (ModelState.IsValid)
            {
                for (int i = 0; i < itemCount; i++)
                {
                    int thisID = Convert.ToInt32(form["item.UserCharacterSelectionID"].ToString().Split(",")[i]);
                    UserCharacterSelection userCharacterSelection = _context.UserCharacterSelection.Where(m => thisID.Equals(m.UserCharacterSelectionID)).FirstOrDefault();
                    //to do, populate and update fields which changed

                    //to do, add cooler checkboxes back
                
                    //use this syntax to update!
                    //_context.Update(userCharacterSelection);
                    //await _context.SaveChangesAsync();
                }
            
                foreach (UserCharacterSelection record in changedRecords)
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
        public async Task<IActionResult> Edit(int id, [Bind("UserName,UserCharacterSelectionID,CharacterID,CharacterName,AliveStatus,BecomesAWhiteWalker,CreatedDate")] UserCharacterSelection userCharacterSelection)
        {
            if (id != userCharacterSelection.UserCharacterSelectionID)
            {
                return NotFound();
            }

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

        private bool UserCharacterSelectionExists(int id)
        {
            return _context.UserCharacterSelection.Any(e => e.UserCharacterSelectionID == id);
        }
    }
}
