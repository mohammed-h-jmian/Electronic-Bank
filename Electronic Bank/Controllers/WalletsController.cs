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
    public class WalletsController : Controller
    {
        private readonly ElectronicBankContext _context;

        public WalletsController(ElectronicBankContext context)
        {
            _context = context;
        }

        // GET: Wallets
        public async Task<IActionResult> Index()
        {
            var electronicBankContext = _context.Wallets.Include(w => w.Clients).Include(w => w.Currencys);
            return View(await electronicBankContext.ToListAsync());
        }

        // GET: Wallets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Wallets == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallets
                .Include(w => w.Clients)
                .Include(w => w.Currencys)
                .FirstOrDefaultAsync(m => m.WalletID == id);
            if (wallet == null)
            {
                return NotFound();
            }

            return View(wallet);
        }

        // GET: Wallets/Create
        public async Task<IActionResult> Create(int? id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            ViewData["ClientID"] = id;
            ViewData["CurrencyID"] = new SelectList(_context.Currencys, "CurrencyID", "CurrencyName");
            return View();
        }

        // POST: Wallets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WalletID,WalletAmount,ClientID,CurrencyID")] Wallet wallet)
        {
            //if (ModelState.IsValid)
            //{
            _context.Add(wallet);

            //}
            ViewData["ClientID"] = new SelectList(_context.Clients, "ClientID", "ClientID", wallet.ClientID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencys, "CurrencyID", "CurrencyID", wallet.CurrencyID);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            return View(wallet);
        }

        // GET: Wallets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Wallets == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallets.FindAsync(id);
            if (wallet == null)
            {
                return NotFound();
            }
            ViewData["ClientID"] = new SelectList(_context.Clients, "ClientID", "ClientName", wallet.ClientID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencys, "CurrencyID", "CurrencyName", wallet.CurrencyID);
            return View(wallet);
        }

        // POST: Wallets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WalletID,WalletAmount,ClientID,CurrencyID")] Wallet wallet)
        {
            if (id != wallet.WalletID)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
            try
            {
                _context.Update(wallet);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WalletExists(wallet.WalletID))
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
            ViewData["ClientID"] = new SelectList(_context.Clients, "ClientID", "ClientID", wallet.ClientID);
            ViewData["CurrencyID"] = new SelectList(_context.Currencys, "CurrencyID", "CurrencyID", wallet.CurrencyID);
            return View(wallet);
        }

        // GET: Wallets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Wallets == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallets
                .Include(w => w.Clients)
                .Include(w => w.Currencys)
                .FirstOrDefaultAsync(m => m.WalletID == id);
            if (wallet == null)
            {
                return NotFound();
            }

            return View(wallet);
        }

        // POST: Wallets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Wallets == null)
            {
                return Problem("Entity set 'ElectronicBankContext.Wallets'  is null.");
            }
            var wallet = await _context.Wallets.FindAsync(id);
            if (wallet != null)
            {
                _context.Wallets.Remove(wallet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WalletExists(int id)
        {
            return (_context.Wallets?.Any(e => e.WalletID == id)).GetValueOrDefault();
        }


    }
}
