using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCAndreAirLines.Data;
using Models;
using Models.Services;

namespace MVCAndreAirLines.Controllers
{
    public class AeroportoesController : Controller
    {
        private readonly MVCAndreAirLinesContext _context;

        public AeroportoesController(MVCAndreAirLinesContext context)
        {
            _context = context;
        }

        // GET: Aeroportoes
        public async Task<IActionResult> Index()
        {
            return View(await ConsultaAPI.BuscaAeroportoAsync());
        }

        // GET: Aeroportoes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aeroporto = await _context.Aeroporto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aeroporto == null)
            {
                return NotFound();
            }

            return View(aeroporto);
        }

        // GET: Aeroportoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Aeroportoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Sigla,Nome,LoginUser")] Aeroporto aeroporto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aeroporto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aeroporto);
        }

        // GET: Aeroportoes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aeroporto = await _context.Aeroporto.FindAsync(id);
            if (aeroporto == null)
            {
                return NotFound();
            }
            return View(aeroporto);
        }

        // POST: Aeroportoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Sigla,Nome,LoginUser")] Aeroporto aeroporto)
        {
            if (id != aeroporto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aeroporto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AeroportoExists(aeroporto.Id))
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
            return View(aeroporto);
        }

        // GET: Aeroportoes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aeroporto = await _context.Aeroporto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aeroporto == null)
            {
                return NotFound();
            }

            return View(aeroporto);
        }

        // POST: Aeroportoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var aeroporto = await _context.Aeroporto.FindAsync(id);
            _context.Aeroporto.Remove(aeroporto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AeroportoExists(string id)
        {
            return _context.Aeroporto.Any(e => e.Id == id);
        }
    }
}
