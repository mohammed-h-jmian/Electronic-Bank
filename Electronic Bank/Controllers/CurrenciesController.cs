using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Electronic_Bank.Data;
using Electronic_Bank.Models;

namespace Electronic_Bank.Controllers
{
    public class CurrenciesController : Controller
    {
        private readonly ElectronicBankContext _context;

        public CurrenciesController(ElectronicBankContext context)
        {
            _context = context;
        }

        // GET: Currencies
        public async Task<IActionResult> Index()
        {
              return _context.Currencys != null ? 
                          View(await _context.Currencys.ToListAsync()) :
                          Problem("Entity set 'ElectronicBankContext.Currencys'  is null.");
        }

        // GET: Currencies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Currencys == null)
            {
                return NotFound();
            }

            var currency = await _context.Currencys
                .FirstOrDefaultAsync(m => m.CurrencyID == id);
            if (currency == null)
            {
                return NotFound();
            }

            return View(currency);
        }

        // GET: Currencies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Currencies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CurrencyID,CurrencyName,CurrencyCode,CurrencyPrice")] Currency currency)
        {
        //    if (ModelState.IsValid)
        //    {
                _context.Add(currency);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            return View(currency);
        }

        // GET: Currencies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Currencys == null)
            {
                return NotFound();
            }

            var currency = await _context.Currencys.FindAsync(id);
            if (currency == null)
            {
                return NotFound();
            }
            return View(currency);
        }

        // POST: Currencies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CurrencyID,CurrencyName,CurrencyCode,CurrencyPrice")] Currency currency)
        {
            if (id != currency.CurrencyID)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
           // {
                try
                {
                    _context.Update(currency);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CurrencyExists(currency.CurrencyID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            //}
            return View(currency);
        }

        // GET: Currencies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Currencys == null)
            {
                return NotFound();
            }

            var currency = await _context.Currencys
                .FirstOrDefaultAsync(m => m.CurrencyID == id);
            if (currency == null)
            {
                return NotFound();
            }

            return View(currency);
        }

        // POST: Currencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Currencys == null)
            {
                return Problem("Entity set 'ElectronicBankContext.Currencys'  is null.");
            }
            var currency = await _context.Currencys.FindAsync(id);
            if (currency != null)
            {
                _context.Currencys.Remove(currency);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CurrencyExists(int id)
        {
          return (_context.Currencys?.Any(e => e.CurrencyID == id)).GetValueOrDefault();
        }
    }
}
