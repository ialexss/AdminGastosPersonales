﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GastosPersonales.Data;
using GastosPersonales.Models;
using Microsoft.AspNetCore.Authorization;

namespace GastosPersonales.Controllers
{
    public class ComprobantesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComprobantesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Comprobantes
        [Authorize]
        public async Task<IActionResult> Index()
        {
              return _context.Comprobante != null ? 
                          View(await _context.Comprobante.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Comprobante'  is null.");
        }

        // GET: Comprobantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comprobante == null)
            {
                return NotFound();
            }

            var comprobante = await _context.Comprobante
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comprobante == null)
            {
                return NotFound();
            }

            return View(comprobante);
        }

        // GET: Comprobantes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Comprobantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Detalle,Fecha,Costo,Tipo,Activo,Imagen,UserId")] Comprobante comprobante)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comprobante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(comprobante);
        }

        // GET: Comprobantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Comprobante == null)
            {
                return NotFound();
            }

            var comprobante = await _context.Comprobante.FindAsync(id);
            if (comprobante == null)
            {
                return NotFound();
            }
            return View(comprobante);
        }

        // POST: Comprobantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Detalle,Fecha,Costo,Tipo,Activo,Imagen,UserId")] Comprobante comprobante)
        {
            if (id != comprobante.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comprobante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComprobanteExists(comprobante.Id))
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
            return View(comprobante);
        }

        // GET: Comprobantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comprobante == null)
            {
                return NotFound();
            }

            var comprobante = await _context.Comprobante
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comprobante == null)
            {
                return NotFound();
            }

            return View(comprobante);
        }

        // POST: Comprobantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Comprobante == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Comprobante'  is null.");
            }
            var comprobante = await _context.Comprobante.FindAsync(id);
            if (comprobante != null)
            {
                _context.Comprobante.Remove(comprobante);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComprobanteExists(int id)
        {
          return (_context.Comprobante?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
