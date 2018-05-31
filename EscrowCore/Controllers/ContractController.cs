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


        //}
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
        public async Task<ActionResult> Details(int id)
        {
            Escrow res;
            ContractAccess contractAccess = new ContractAccess();
            using (var _context = new ApplicationDbContext())
            {
                res = await _context.DeployedContracts.FindAsync(id);

            }
            var rec = await contractAccess.PollReceipt(res.TransactionHash);
            res.Receipt = new Receipt(rec);
            return View(res);
        }
        // GET: Contract/Create
        public IActionResult Create()
        {
            return View();
        }

      

      
    }
}
