using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GastosPersonales.Data;
using GastosPersonales.Models;

namespace GastosPersonales.Controllers
{
    public class SaldoMensualesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SaldoMensualesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SaldoMensuales
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SaldoMensual.Include(s => s.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SaldoMensuales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SaldoMensual == null)
            {
                return NotFound();
            }

            var saldoMensual = await _context.SaldoMensual
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (saldoMensual == null)
            {
                return NotFound();
            }

            return View(saldoMensual);
        }

        // GET: SaldoMensuales/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: SaldoMensuales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Total,Mes,Año,UserId")] SaldoMensual saldoMensual)
        {
            if (ModelState.IsValid)
            {
                _context.Add(saldoMensual);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", saldoMensual.UserId);
            return View(saldoMensual);
        }

        // GET: SaldoMensuales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SaldoMensual == null)
            {
                return NotFound();
            }

            var saldoMensual = await _context.SaldoMensual.FindAsync(id);
            if (saldoMensual == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", saldoMensual.UserId);
            return View(saldoMensual);
        }

        // POST: SaldoMensuales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Total,Mes,Año,UserId")] SaldoMensual saldoMensual)
        {
            if (id != saldoMensual.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(saldoMensual);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SaldoMensualExists(saldoMensual.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", saldoMensual.UserId);
            return View(saldoMensual);
        }

        // GET: SaldoMensuales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SaldoMensual == null)
            {
                return NotFound();
            }

            var saldoMensual = await _context.SaldoMensual
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (saldoMensual == null)
            {
                return NotFound();
            }

            return View(saldoMensual);
        }

        // POST: SaldoMensuales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SaldoMensual == null)
            {
                return Problem("Entity set 'ApplicationDbContext.SaldoMensual'  is null.");
            }
            var saldoMensual = await _context.SaldoMensual.FindAsync(id);
            if (saldoMensual != null)
            {
                _context.SaldoMensual.Remove(saldoMensual);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SaldoMensualExists(int id)
        {
          return (_context.SaldoMensual?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
