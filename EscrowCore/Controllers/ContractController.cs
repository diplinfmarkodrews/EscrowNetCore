using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EscrowCore.Data;
using EscrowCore.Models;
using EscrowCore.Utils;

namespace EscrowCore.Controllers
{
    public class ContractController : Controller
    {
        private readonly ApplicationDbContext _context;

        //public ContractController(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        // GET: Contract
        public async Task<IActionResult> Index()
        {
            return View(await _context.DeployedContracts.ToListAsync());
        }
        [HttpGet]
        public async Task<ActionResult> GetReceipt(string id)
        {
            var receipt = await ContractAccess.PollReceipt(id);

            return Json(receipt);


        }
        public async Task<ActionResult> DeleteAllTxs()
        {
            using (var _context = new ApplicationDbContext())
            {
                foreach (var contract in _context.DeployedContracts.Include(p => p.Receipt))
                {

                    _context.DeployReceipt.Remove(contract.Receipt);
                    _context.DeployedContracts.Remove(contract);

                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Home");
        }
        public static async Task<IEnumerable<Models.Escrow>> getContracts()
        {
            IEnumerable<Escrow> res;
            using (var _context = new ApplicationDbContext())
            {
                res = await _context.DeployedContracts.Include(p => p.Receipt).ToListAsync();

            }
            return res;

        }
        // GET: Contract/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var escrow = await _context.DeployedContracts
                .SingleOrDefaultAsync(m => m.ID == id);
            if (escrow == null)
            {
                return NotFound();
            }

            return View(escrow);
        }

        // GET: Contract/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contract/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ContractAddress,SellerAddress,BuyerAddress,EscrowHolderAddress,TransactionHash")] Escrow escrow)
        {
            if (ModelState.IsValid)
            {
                _context.Add(escrow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(escrow);
        }

        // GET: Contract/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var escrow = await _context.DeployedContracts.SingleOrDefaultAsync(m => m.ID == id);
            if (escrow == null)
            {
                return NotFound();
            }
            return View(escrow);
        }

        // POST: Contract/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ContractAddress,SellerAddress,BuyerAddress,EscrowHolderAddress,TransactionHash")] Escrow escrow)
        {
            if (id != escrow.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(escrow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EscrowExists(escrow.ID))
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
            return View(escrow);
        }

        // GET: Contract/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var escrow = await _context.DeployedContracts
                .SingleOrDefaultAsync(m => m.ID == id);
            if (escrow == null)
            {
                return NotFound();
            }

            return View(escrow);
        }

        // POST: Contract/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var escrow = await _context.DeployedContracts.SingleOrDefaultAsync(m => m.ID == id);
            _context.DeployedContracts.Remove(escrow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EscrowExists(int id)
        {
            return _context.DeployedContracts.Any(e => e.ID == id);
        }
    }
}
