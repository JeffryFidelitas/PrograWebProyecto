using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    public class LavadosController : Controller
    {
        private readonly ProyectoContext _context;

        public LavadosController(ProyectoContext context)
        {
            _context = context;
        }

        // GET: Lavados
        //Solo Administrador
        public async Task<IActionResult> Index()
        {
            return View(await _context.Lavado.ToListAsync());
        }

        // GET: Lavados/Details/5
        //Solo Administrador
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lavado = await _context.Lavado
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lavado == null)
            {
                return NotFound();
            }

            return View(lavado);
        }

        // GET: Lavados/Create
        //Solo Administrador
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lavados/Create
        //Solo Administrador
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Descripcion,Precio,Duracion,Estado")] Lavado lavado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lavado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lavado);
        }

        // GET: Lavados/Edit/5
        //Solo Administrador
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lavado = await _context.Lavado.FindAsync(id);
            if (lavado == null)
            {
                return NotFound();
            }
            return View(lavado);
        }

        // POST: Lavados/Edit/5
        //Solo Administrador
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Precio,Duracion,Estado")] Lavado lavado)
        {
            if (id != lavado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lavado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LavadoExists(lavado.Id))
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
            return View(lavado);
        }

        // GET: Lavados/Delete/5
        //Solo administrador
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lavado = await _context.Lavado
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lavado == null)
            {
                return NotFound();
            }

            return View(lavado);
        }

        // POST: Lavados/Delete/5
        //Solo Administrador
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lavado = await _context.Lavado.FindAsync(id);
            if (lavado != null)
            {
                _context.Lavado.Remove(lavado);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LavadoExists(int id)
        {
            return _context.Lavado.Any(e => e.Id == id);
        }
    }
}
