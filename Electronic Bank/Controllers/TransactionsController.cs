using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Electronic_Bank.Data;
using Electronic_Bank.Models;
using Electronic_Bank.ViewModel;

namespace Electronic_Bank.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly ElectronicBankContext _context;

        public TransactionsController(ElectronicBankContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var electronicBankContext = _context.Transactions.Include(t => t.Wallets);
            return View(await electronicBankContext.ToListAsync());
        }

        public ActionResult SendFailed()
        {
            return View();
        }



        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Wallets)
                .FirstOrDefaultAsync(m => m.TransactionID == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            ViewData["WalletID"] = new SelectList(_context.Wallets, "WalletID", "WalletID");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionID,WalletID,TransactionAmount,TransactionDescription,TransactionDate,TransactionType")] Transaction transaction)
        {
            //if (ModelState.IsValid)
            //{
            var nowDate = DateTime.Now;
            transaction.TransactionDate = nowDate;
            _context.Add(transaction);

            //}
            ViewData["WalletID"] = new SelectList(_context.Wallets, "WalletID", "WalletID", transaction.WalletID);
            var walletq = await _context.Wallets
    .FirstOrDefaultAsync(m => m.WalletID == transaction.WalletID);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            return View(transaction);
        }

        // GET: Transactions/Create
        public async Task<IActionResult> Withdraw(int? walletId, int? clientId)
        {
            ViewData["WalletID"] = walletId;
            ViewData["ClientID"] = clientId;
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdraw([Bind("TransactionID,WalletID,TransactionAmount,TransactionDescription,TransactionDate,TransactionType")] Transaction transaction)
        {
            //if (ModelState.IsValid)
            //{
            var wallet = await _context.Wallets
                        .FirstOrDefaultAsync(m => m.WalletID == transaction.WalletID);
            var curr = await _context.Currencys
                       .FirstOrDefaultAsync(m => m.CurrencyID == wallet.CurrencyID);
            if (transaction.TransactionAmount <= wallet.WalletAmount && transaction.TransactionAmount > 0)
            {
                var nowDate = DateTime.Now;
                transaction.TransactionDate = nowDate;
                transaction.TransactionType = "withdraw";
                transaction.TransactionDescription = transaction.TransactionAmount + " " + curr.CurrencyCode + " has been withdrawn by the customer";
                _context.Add(transaction);

                //}
                var walletq = await _context.Wallets
        .FirstOrDefaultAsync(m => m.WalletID == transaction.WalletID);



                walletq.WalletAmount = walletq.WalletAmount - transaction.TransactionAmount;


               
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(SendFailed));
            }

            return View(transaction);
        }
        /**************************************************************/
        // GET: Transactions/Create
        public async Task<IActionResult> Deposit(int? walletId, int? clientId)
        {
            ViewData["WalletID"] = walletId;
            ViewData["ClientID"] = clientId;
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit([Bind("TransactionID,WalletID,TransactionAmount,TransactionDescription,TransactionDate,TransactionType")] Transaction transaction)
        {
            var wallet = await _context.Wallets
                     .FirstOrDefaultAsync(m => m.WalletID == transaction.WalletID);
            var curr = await _context.Currencys
                       .FirstOrDefaultAsync(m => m.CurrencyID == wallet.CurrencyID);

            if (transaction.TransactionAmount > 0)
            {
                //if (ModelState.IsValid)
                //{
                var nowDate = DateTime.Now;
                transaction.TransactionDate = nowDate;
                transaction.TransactionType = "deposit";
                transaction.TransactionDescription = transaction.TransactionAmount + " " + curr.CurrencyCode + " deposited by the customer";
                _context.Add(transaction);

                //}

                var walletq = await _context.Wallets
        .FirstOrDefaultAsync(m => m.WalletID == transaction.WalletID);



                walletq.WalletAmount = walletq.WalletAmount + transaction.TransactionAmount;


                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(SendFailed));
            }

            return View(transaction);
        }
        /****************************************************************/


        // GET: Transactions/Create
        public async Task<IActionResult> Transfer(int? walletId, int? clientId)
        {

            ViewData["WalletID"] = walletId;
            ViewData["ClientID"] = clientId;

            //Wallet w = await _context.Wallets
            //   .FirstOrDefaultAsync(m => m.WalletID == id);
            //var m = _context.Wallets;
            //m.Remove(w);

            ViewBag.Wallets = new SelectList(_context.Wallets, "WalletID", "WalletID");
            return View();
        }



        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer([Bind("senderID,receiverID,Amount")] TransactionTransfer transactionTransfer)
        {
            //if (ModelState.IsValid)
            //{
            ViewData["Wallets"] = new SelectList(_context.Wallets, "WalletID", "WalletID");



            //}

            var sendID = await _context.Wallets
            .FirstOrDefaultAsync(m => m.WalletID == transactionTransfer.senderID);

            if (transactionTransfer.Amount <= sendID.WalletAmount && transactionTransfer.Amount > 0)
            {
                Transaction t = new Transaction();
                var nowDate = DateTime.Now;

                var senderPrice = await _context.Currencys
                            .FirstOrDefaultAsync(m => m.CurrencyID == sendID.CurrencyID);




                var receiveID = await _context.Wallets
                .FirstOrDefaultAsync(m => m.WalletID == transactionTransfer.receiverID);
                var receivePrice = await _context.Currencys
                .FirstOrDefaultAsync(m => m.CurrencyID == receiveID.CurrencyID);

                sendID.WalletAmount = sendID.WalletAmount - transactionTransfer.Amount;

                double receivedAmount = transactionTransfer.Amount * (senderPrice.CurrencyPrice / receivePrice.CurrencyPrice);

                receiveID.WalletAmount = receiveID.WalletAmount + receivedAmount;

                t.TransactionDate = nowDate;
                t.TransactionType = "transfer";
                t.TransactionDescription = "The amount of " + transactionTransfer.Amount + " " + senderPrice.CurrencyCode +
                                         " was sent from the wallet with id " + transactionTransfer.senderID + " to the wallet with id " + transactionTransfer.receiverID + " and an amount has been added " + receiveID.WalletAmount + " " + receivedAmount;
                t.TransactionAmount = transactionTransfer.Amount;
                t.WalletID = transactionTransfer.senderID;
                _context.Add(t);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(SendFailed));

            }

            return View(transactionTransfer);
        }
        /*****************************************************************/
        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["WalletID"] = new SelectList(_context.Wallets, "WalletID", "WalletID", transaction.WalletID);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransactionID,WalletID,TransactionAmount,TransactionDescription,TransactionDate,TransactionType")] Transaction transaction)
        {
            if (id != transaction.TransactionID)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
            try
            {
                _context.Update(transaction);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(transaction.TransactionID))
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
            ViewData["WalletID"] = new SelectList(_context.Wallets, "WalletID", "WalletID", transaction.WalletID);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Wallets)
                .FirstOrDefaultAsync(m => m.TransactionID == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Transactions == null)
            {
                return Problem("Entity set 'ElectronicBankContext.Transactions'  is null.");
            }
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
            return (_context.Transactions?.Any(e => e.TransactionID == id)).GetValueOrDefault();
        }

    }
}
