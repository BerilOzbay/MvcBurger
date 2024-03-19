using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcBurger.Areas.Identity.Data;
using MvcBurger.Entities;

namespace MvcBurger.Controllers
{
    //[Authorize(Roles ="Musteri")]
    public class SiparisController : Controller
    {
        private readonly MvcBurgerContext _context;
        private readonly UserManager<MvcBurgerUser> _userManager;

        public SiparisController(MvcBurgerContext context, UserManager<MvcBurgerUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Siparis
        public async Task<IActionResult> Index()
        {
            //Tüm siparisler
            ViewBag.Menuler = await _context.Menuler.ToListAsync();
            ViewBag.EkstraMalzemeler = await _context.EkstraMalzemeler.ToListAsync();


            var allUsers = await _userManager.Users.Include(u => u.Siparisler).ToListAsync();
            
            var userId = _userManager.GetUserId(HttpContext.User);
            //Anlık kullanıcı
            var user = allUsers.FirstOrDefault(u => u.Id == userId);
            ViewBag.FullName = user.Ad + " " + user.Soyad;
            //Kullanıcının kendi siparisleri
            ViewBag.KullaniciSiparis = user.Siparisler;
            return View();
            
        }

        // GET: Siparis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Siparisler == null)
            {
                return NotFound();
            }

            var siparis = await _context.Siparisler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (siparis == null)
            {
                return NotFound();
            }

            return View(siparis);
        }

        // GET: Siparis/Create
        public IActionResult Create()
        {
            ViewBag.Menuler1 = _context.Menuler.ToList();
            ViewBag.EkstraMalzemeler1 = _context.EkstraMalzemeler.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Siparis siparis)
        {
            return View();
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Siparisler == null)
            {
                return NotFound();
            }

            var siparis = await _context.Siparisler.FindAsync(id);
            if (siparis == null)
            {
                return NotFound();
            }
            return View(siparis);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MenuId,Buyukluk,EkstraMalzemeId,SiparisSayisi,ToplamFiyat")] Siparis siparis)
        {
            if (id != siparis.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(siparis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SiparisExists(siparis.Id))
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
            return View(siparis);
        }

        // GET: Siparis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Siparisler == null)
            {
                return NotFound();
            }

            var siparis = await _context.Siparisler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (siparis == null)
            {
                return NotFound();
            }

            return View(siparis);
        }

        // POST: Siparis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Siparisler == null)
            {
                return Problem("Entity set 'MvcBurgerContext.Siparisler'  is null.");
            }
            var siparis = await _context.Siparisler.FindAsync(id);
            if (siparis != null)
            {
                _context.Siparisler.Remove(siparis);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SiparisExists(int id)
        {
          return (_context.Siparisler?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
