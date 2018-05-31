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

        
        public async Task<IActionResult> Index()
        {
            ContractVM contractVM = new ContractVM();

            contractVM.DeployContractVM.SellerAddress = "0x7AC658F93a3f21187Ef55afd2E47509dc9E63B15";
            contractVM.DeployContractVM.BuyerAddress = "0x87f39fB5D19bB6e673Fec08ee09a97872241de6C";
            contractVM.DeployContractVM.EscrowHolderAddress = "0x987D6639c025354042BB913A503F4532390652ad";
            contractVM.DeployContractVM.EtherToPay = "0.0001";
            contractVM.DeployContractVM.ExpiryDate = DateTime.Now.AddDays(30);
            contractVM.DeployContractVM.GasLimit = "67412";

            ContractVM contractVMRopsten = new ContractVM();

            contractVMRopsten.DeployContractVM.SellerAddress = "0x005e44B5ce1E91c2ee3b6e13B52F174b664b8124";
            contractVMRopsten.DeployContractVM.BuyerAddress = "0x002E271cd0b4f04Ca47F12D39d248dDb08D4eDdE";
            contractVMRopsten.DeployContractVM.EscrowHolderAddress = "0x004EF9E943EbeecF4d161384666d939b34af9146";
            contractVMRopsten.DeployContractVM.EtherToPay = "0.0001";
            contractVMRopsten.DeployContractVM.ExpiryDate = DateTime.Now.AddDays(30);
            contractVMRopsten.DeployContractVM.GasLimit = "67412";

            contractVMRopsten.ContractList = await ContractController.getContracts();


            return View(contractVMRopsten);
        }
    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContractVM contractVM)
        {

            ContractParam contractParam = new ContractParam(contractVM.DeployContractVM);
            ContractAccess contractAccess = new ContractAccess();
            try
            {
                var transactionHash = await contractAccess.DeployContract(contractParam);
                contractVM.Message = transactionHash.ToString();
                var receipt = await contractAccess.PollReceipt(transactionHash);
                contractVM.DeployContractVM.ContractAddress = receipt.ContractAddress;
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

