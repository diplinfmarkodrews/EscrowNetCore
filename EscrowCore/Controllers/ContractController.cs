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
using EscrowCore.Models.VM;

namespace EscrowCore.Controllers
{
    public class ContractController : Controller
    {


        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeployContract(DeployContractVM contractVM)
        {
           
            ContractParam contractParam = new ContractParam(contractVM);
            ContractAccess contractAccess = new ContractAccess(contractVM.Network);

            try
            {
                var transactionHash = await contractAccess.DeployContract(contractParam);
               // contractVM.Message = transactionHash.ToString();
             
               Escrow newContract = new Escrow(transactionHash.ToString(), contractVM);
              
                using (var _context = new ApplicationDbContext())
                {
                    
                    _context.DeployedContracts.Add(newContract);

                    await _context.SaveChangesAsync();
                    //contractVM.ContractList = await _context.DeployedContracts.Include(p => p.Receipt).ToListAsync();
                }
                return new JsonResult(transactionHash);
            }
            catch (Exception e)
            {
                return new JsonResult(e.Message);
            }

            
        }
        public async Task<IActionResult> PollReceipt(string id)
        {
            Receipt res;
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var contract = context.DeployedContracts.Where(p => p.TransactionHash == id).FirstOrDefault();

                    ContractAccess contractAccess = new ContractAccess(contract.Network);
                    var receipt = await contractAccess.PollReceipt(id);
                    res = new Receipt(receipt);


                   
                    context.Entry(contract).Entity.Receipt = res;
                    
                    context.DeployReceipt.Add(res);
                    await context.SaveChangesAsync();

                }
                return PartialView("~/Views/Home/_Receipt.cshtml", res);
            }
            catch (Exception e) {
                return PartialView("~/Views/Shared/Error.cshtml", e);
            }
        }
        public async Task<ActionResult> DeleteAllTxs()
        {
            using (var _context = new ApplicationDbContext())
            {
                foreach (var contract in _context.DeployedContracts.Include(p => p.Receipt))
                {
                    if (contract.Receipt != null)
                    {
                        _context.DeployReceipt.Remove(contract.Receipt);
                    }
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
        public async Task<IActionResult> GetListView()
        {
            IEnumerable<Escrow> res;
            using (var _context = new ApplicationDbContext())
            {
                res = await _context.DeployedContracts.Include(p => p.Receipt).ToListAsync();

            }
            return PartialView("~/Views/Home/_ListContracts.cshtml", res);

        }
        public async Task<ActionResult> Details(int id)
        {
            Escrow res;
            
            using (var _context = new ApplicationDbContext())
            {
                res = await _context.DeployedContracts.Include(p=>p.Receipt).Where(p=>p.ID==id).FirstOrDefaultAsync();

            }
            ContractAccess contractAccess = new ContractAccess(res.Network);
            var rec = await contractAccess.PollReceipt(res.TransactionHash);
            res.Receipt = new Receipt(rec);
            return View(res);
        }
      
      
    }
}
