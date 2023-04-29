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
    public class ComprobantesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComprobantesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Comprobantes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Comprobante.Include(c => c.Categoria).Include(c => c.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Comprobantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comprobante == null)
            {
                return NotFound();
            }

            var comprobante = await _context.Comprobante
                .Include(c => c.Categoria)
                .Include(c => c.User)
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
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nombre");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Comprobantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Detalle,Fecha,Costo,Tipo,Activo,Imagen,UserId,CategoriaId")] Comprobante comprobante)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comprobante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nombre", comprobante.CategoriaId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", comprobante.UserId);
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
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nombre", comprobante.CategoriaId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", comprobante.UserId);
            return View(comprobante);
        }

        // POST: Comprobantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Detalle,Fecha,Costo,Tipo,Activo,Imagen,UserId,CategoriaId")] Comprobante comprobante)
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
            ViewData["CategoriaId"] = new SelectList(_context.Categoria, "Id", "Nombre", comprobante.CategoriaId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", comprobante.UserId);
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
                .Include(c => c.Categoria)
                .Include(c => c.User)
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
