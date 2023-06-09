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
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using SendGrid.Helpers.Mail;

namespace GastosPersonales.Controllers
{
    public class SaldoMensualesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SaldoMensualesController(ApplicationDbContext context)
        {
            _context = context;

        }

        // GET: SaldoMensuales/SearchByYear
        public async Task<IActionResult> SearchByYear(int? year)
        {
            //Valida que no se pueda buscar en futuro
            if (!year.HasValue || year < 1900 || year > DateTime.Today.Year)
            {
                return NotFound();
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var applicationDbContext = _context.SaldoMensual.Include(s => s.User)
                .Where(c => c.UserId == userId && c.Año == year);

            return View("Index", await applicationDbContext.ToListAsync());
        }

        // GET: SaldoMensuales
        [Authorize]
        public async Task<IActionResult> Index()
        {
            //Trae el usuario que inicio sesion
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var applicationDbContext = _context.SaldoMensual.Include(s => s.User).Where(c => c.UserId == userId);
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
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //Saldo Actual
            decimal sumaIngresos = _context.Comprobante.Where(c => c.Tipo == "Ingreso" && c.UserId == userId).Sum(c => c.Costo);
            decimal sumaGastos = _context.Comprobante.Where(c => c.Tipo == "Egreso" && c.UserId == userId).Sum(c => c.Costo);
            decimal saldoactual = sumaIngresos - sumaGastos;

            // Establecer valores predeterminados para el mes y el año
            int mes = DateTime.Now.Month;
            int año = DateTime.Now.Year;

            ViewData["TotalActual"] = saldoactual.ToString(); // Mandamos el total actual a la vista
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["Mes"] = mes;
            ViewData["Año"] = año;

            return View();
        }
  

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Total,Mes,Año,UserId")] SaldoMensual saldoMensual)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            saldoMensual.UserId = userId;

            // Validar si ya existe un saldo mensual para el mismo mes y año
            bool mesExistente = _context.SaldoMensual.Any(s => s.UserId == userId && s.Mes == saldoMensual.Mes && s.Año == saldoMensual.Año);
            if (mesExistente)
            {
                ModelState.AddModelError(string.Empty, "Ya existe un cierre mensual para este mes y año.");
            }

            // Validar si estamos en dia 30 del mes

            int dia = DateTime.Now.Day;
            if (dia != 30)
            {
                ModelState.AddModelError(string.Empty, "Solo puedes registrar un cierre de mes el dia 30 de cada mes.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(saldoMensual);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            //Saldo Actual
            decimal sumaIngresos = _context.Comprobante.Where(c => c.Tipo == "Ingreso" && c.UserId == userId).Sum(c => c.Costo);
            decimal sumaGastos = _context.Comprobante.Where(c => c.Tipo == "Egreso" && c.UserId == userId).Sum(c => c.Costo);
            decimal saldoactual = sumaIngresos - sumaGastos;

            // Establecer valores predeterminados para el mes y el año
            int mes = DateTime.Now.Month;
            int año = DateTime.Now.Year;

            ViewData["TotalActual"] = saldoactual.ToString(); // Mandamos el total actual a la vista
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", saldoMensual.UserId);
            ViewData["Mes"] = mes;
            ViewData["Año"] = año;

            //si no es valido
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
                // Validar que el mes no se repita
                var existingSaldoMensual = await _context.SaldoMensual
                    .Where(sm => sm.Mes == saldoMensual.Mes && sm.Año == saldoMensual.Año && sm.Id != saldoMensual.Id)
                    .FirstOrDefaultAsync();

                if (existingSaldoMensual != null)
                {
                    ModelState.AddModelError("Mes", "Ya existe un saldo mensual registrado para este mes y año");
                    ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", saldoMensual.UserId);
                    return View(saldoMensual);
                }

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