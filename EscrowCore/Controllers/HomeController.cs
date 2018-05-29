using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EscrowCore.Models;
using EscrowCore.Models.VM;
using Microsoft.EntityFrameworkCore;
using EscrowCore.Utils;
using EscrowCore.Data;

namespace EscrowCore.Controllers
{
    public class HomeController : Controller
    {

        private async Task<IEnumerable<Models.Escrow>> getContracts()
        {
            IEnumerable<Escrow> res;
            using (var _context = new ApplicationDbContext())
            {
                res = await _context.DeployedContracts.Include(p => p.Receipt).ToListAsync();

            }
            return res;

        }
    public async Task<IActionResult> Index()
    {
        ContractVM contractVM = new ContractVM();

        contractVM.DeployContractVM.SellerAddress = "0x7AC658F93a3f21187Ef55afd2E47509dc9E63B15";
        contractVM.DeployContractVM.BuyerAddress = "0x87f39fB5D19bB6e673Fec08ee09a97872241de6C";
        contractVM.DeployContractVM.EscrowHolderAddress = "0x987D6639c025354042BB913A503F4532390652ad";
        contractVM.DeployContractVM.EtherToPay = "0.0001";
        contractVM.DeployContractVM.ExpiryDate = DateTime.Now.AddDays(30);
        contractVM.DeployContractVM.GasLimit = "67412";

        contractVM.ContractList = await getContracts();


        return View(contractVM);
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
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContractVM contractVM)
    {

        ContractParam contractParam = new ContractParam(contractVM.DeployContractVM);
        try
        {
            var transactionHash = await ContractAccess.DeployContract(contractParam);
            contractVM.Message = transactionHash.ToString();
            var receipt = await ContractAccess.PollReceipt(transactionHash);
            Escrow newContract = new Escrow(transactionHash.ToString(), contractVM.DeployContractVM);
            newContract.Receipt = new Receipt(receipt);
            using (var _context = new ApplicationDbContext())
            {
                _context.DeployReceipt.Add(newContract.Receipt);
                _context.DeployedContracts.Add(newContract);

                await _context.SaveChangesAsync();
                contractVM.ContractList = await _context.DeployedContracts.Include(p => p.Receipt).ToListAsync();
            }
            contractVM.Receipt = new Receipt(receipt);


        }
        catch (Exception e)
        {
            contractVM.Message = e.Message;
            contractVM.Flag = StatusFlag.Error;
        }

        return View(contractVM);
    }
    [HttpGet]
    public async Task<ActionResult> GetReceipt(string id)
    {
        var receipt = await ContractAccess.PollReceipt(id);

        return Json(receipt);


    }

    public async Task<ActionResult> Details(int id)
    {
        Escrow res;
        using (var _context = new ApplicationDbContext())
        {
            res = await _context.DeployedContracts.FindAsync(id);

        }
        var rec = await ContractAccess.PollReceipt(res.TransactionHash);
        res.Receipt = new Receipt(rec);
        return View(res);
    }
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public IActionResult About()
    {
        ViewData["Message"] = "Your application description page.";

        return View();
    }

    public IActionResult Contact()
    {
        ViewData["Message"] = "Your contact page.";

        return View();
    }
}
}

