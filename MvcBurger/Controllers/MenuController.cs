using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcBurger.Areas.Identity.Data;
using MvcBurger.Entities;
using MvcBurger.Models;

namespace MvcBurger.Controllers
{
   // [Authorize(Roles ="Yonetici")]
    public class MenuController : Controller
    {
        private readonly MvcBurgerContext _context;

        public MenuController(MvcBurgerContext context)
        {
            _context = context;
        }

        // GET: Menu
        public async Task<IActionResult> Index()
        {
              return _context.Menuler != null ? 
                          View(await _context.Menuler.ToListAsync()) :
                          Problem("Entity set 'MvcBurgerContext.Menuler'  is null.");
        }

        // GET: Menu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Menuler == null)
            {
                return NotFound();
            }

            var menu = await _context.Menuler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // GET: Menu/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Menu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuViewModel menuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu menu = new Menu();
                if(menuViewModel.ResimAdi != null)
                {
                    var dosyaAdi = menuViewModel.ResimAdi.FileName;
                    var konum = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Resimler", dosyaAdi);

                    //Ekleme için akış ortamı oluşturalım
                    var akisOrtami = new FileStream(konum, FileMode.Create);

                    //Resmi kaydet
                    menuViewModel.ResimAdi.CopyTo(akisOrtami);

                    //ortamı kapat
                    akisOrtami.Close();

                    menu.ResimAdi = dosyaAdi;

                }

                menu.Ad = menuViewModel.Ad;
                menu.Fiyat = menuViewModel.Fiyat;

                _context.Add(menu);
                await _context.SaveChangesAsync();     
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Menu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Menuler == null)
            {
                return NotFound();
            }

            var menu = await _context.Menuler.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }
            return View(menu);
        }

        // POST: Menu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ad,Fiyat,ResimAdi")] Menu menu)
        {
            if (id != menu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(menu.Id))
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
            return View(menu);
        }

        // GET: Menu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Menuler == null)
            {
                return NotFound();
            }

            var menu = await _context.Menuler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // POST: Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Menuler == null)
            {
                return Problem("Entity set 'MvcBurgerContext.Menuler'  is null.");
            }
            var menu = await _context.Menuler.FindAsync(id);
            if (menu != null)
            {
                _context.Menuler.Remove(menu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
          return (_context.Menuler?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
