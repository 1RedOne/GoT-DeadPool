using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameOfThronePool.Data;
using GameOfThronePool.Models;
using Microsoft.AspNetCore.Http;

namespace GameOfThronePool.Views
{
    public class SetupCharactersController : Controller
    {
        private readonly DeadPoolDBContext _context;

        public SetupCharactersController(DeadPoolDBContext context)
        {
            _context = context;
        }

        // GET: ShowCharacterStatusRecords
        public async Task<IActionResult> Index()
        {
            return View(await _context.ShowCharacterStatusRecord.ToListAsync());
        }

        // GET: ShowCharacterStatusRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var showCharacterStatusRecord = await _context.ShowCharacterStatusRecord
                .FirstOrDefaultAsync(m => m.ShowCharacterStatusRecordID == id);
            if (showCharacterStatusRecord == null)
            {
                return NotFound();
            }

            return View(showCharacterStatusRecord);
        }

        // GET: ShowCharacterStatusRecords/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShowCharacterStatusRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShowCharacterStatusRecordID,CharacterName,CharacterDiedEpisodeNo")] ShowCharacterStatusRecord showCharacterStatusRecord, IFormCollection form)
        {
            showCharacterStatusRecord.CreatedDate = DateTime.Now;
            showCharacterStatusRecord.AliveStatus = (form["AliveStatus"] == "on") ? true : false;
            showCharacterStatusRecord.WhiteWalkerStatus = (form["WhiteWalkerStatus"] == "on") ? true : false;
            if (ModelState.IsValid)
            {
                _context.Add(showCharacterStatusRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(showCharacterStatusRecord);
        }

        // GET: ShowCharacterStatusRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var showCharacterStatusRecord = await _context.ShowCharacterStatusRecord.FindAsync(id);
            if (showCharacterStatusRecord == null)
            {
                return NotFound();
            }
            return View(showCharacterStatusRecord);
        }

        // POST: ShowCharacterStatusRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShowCharacterStatusRecordID,CharacterName,AliveStatus,WhiteWalkerStatus,CharacterDiedEpisodeNo,CreatedDate")] ShowCharacterStatusRecord showCharacterStatusRecord)
        {
            if (id != showCharacterStatusRecord.ShowCharacterStatusRecordID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(showCharacterStatusRecord);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShowCharacterStatusRecordExists(showCharacterStatusRecord.ShowCharacterStatusRecordID))
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
            return View(showCharacterStatusRecord);
        }

        // GET: ShowCharacterStatusRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var showCharacterStatusRecord = await _context.ShowCharacterStatusRecord
                .FirstOrDefaultAsync(m => m.ShowCharacterStatusRecordID == id);
            if (showCharacterStatusRecord == null)
            {
                return NotFound();
            }

            return View(showCharacterStatusRecord);
        }

        // POST: ShowCharacterStatusRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var showCharacterStatusRecord = await _context.ShowCharacterStatusRecord.FindAsync(id);
            _context.ShowCharacterStatusRecord.Remove(showCharacterStatusRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShowCharacterStatusRecordExists(int id)
        {
            return _context.ShowCharacterStatusRecord.Any(e => e.ShowCharacterStatusRecordID == id);
        }
    }
}
