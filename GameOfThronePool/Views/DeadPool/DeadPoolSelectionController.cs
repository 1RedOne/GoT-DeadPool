using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameOfThronePool.Data;
using GameOfThronePool.Models;

namespace GameOfThronePool.Views.DeadPool
{
    public class DeadPoolSelectionController : Controller
    {
        private readonly DeadPoolDBContext _context;

        public DeadPoolSelectionController(DeadPoolDBContext context)
        {
            _context = context;
        }

        // GET: DeadPoolSelection
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserCharacterSelection.ToListAsync());
        }

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

        // GET: DeadPoolSelection/Create
        public IActionResult Create()
        {
            return View();
        }

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,UserCharacterSelectionID,CharacterID,CharacterName,AliveStatus,BecomesAWhiteWalker,CreatedDate")] UserCharacterSelection userCharacterSelection)
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
