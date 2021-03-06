﻿using System;
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

        
        public async Task<IActionResult> Index(int? id)
        {
            if (!id.HasValue) { id = 0; }
            ContractVM contractVM = new ContractVM();
            switch (id)
            {
                case 0:
                    contractVM.DeployContractVM.SellerAddress = "0x7AC658F93a3f21187Ef55afd2E47509dc9E63B15";
                    contractVM.DeployContractVM.BuyerAddress = "0x87f39fB5D19bB6e673Fec08ee09a97872241de6C";
                    contractVM.DeployContractVM.EscrowHolderAddress = "0x987D6639c025354042BB913A503F4532390652ad";
                    contractVM.DeployContractVM.EtherToPay = "0.0001";
                    contractVM.DeployContractVM.ExpiryDate = DateTime.Now.AddDays(30);
                    contractVM.DeployContractVM.GasLimit = "67412";
                    contractVM.DeployContractVM.Network = Network.Ganache;
                    break;
                case 1:
                    contractVM.DeployContractVM.SellerAddress = "0x005e44B5ce1E91c2ee3b6e13B52F174b664b8124";
                    contractVM.DeployContractVM.BuyerAddress = "0x002E271cd0b4f04Ca47F12D39d248dDb08D4eDdE";
                    contractVM.DeployContractVM.EscrowHolderAddress = "0x004EF9E943EbeecF4d161384666d939b34af9146";
                    contractVM.DeployContractVM.EtherToPay = "0.0001";
                    contractVM.DeployContractVM.ExpiryDate = DateTime.Now.AddDays(30);
                    contractVM.DeployContractVM.GasLimit = "67412";
                    contractVM.DeployContractVM.Network = Network.Ropsten;
                    break;
            }
            

            contractVM.ContractList = await ContractController.getContracts();


            return View(contractVM);
        }

        public IActionResult ChangeNetwork(int id)
        {

            DeployContractVM contractVM = new DeployContractVM();
            
            switch (id)
            {
                case 0:
                    contractVM.SellerAddress = "0x7AC658F93a3f21187Ef55afd2E47509dc9E63B15";
                    contractVM.BuyerAddress = "0x87f39fB5D19bB6e673Fec08ee09a97872241de6C";
                    contractVM.EscrowHolderAddress = "0x987D6639c025354042BB913A503F4532390652ad";
                    contractVM.EtherToPay = "0.0001";
                    contractVM.ExpiryDate = DateTime.Now.AddDays(30);
                    contractVM.GasLimit = "67412";
                    contractVM.Network = Network.Ganache;
                    break;
                case 1:
                    contractVM.SellerAddress = "0x005e44B5ce1E91c2ee3b6e13B52F174b664b8124";
                    contractVM.BuyerAddress = "0x002E271cd0b4f04Ca47F12D39d248dDb08D4eDdE";
                    contractVM.EscrowHolderAddress = "0x004EF9E943EbeecF4d161384666d939b34af9146";
                    contractVM.EtherToPay = "0.0001";
                    contractVM.ExpiryDate = DateTime.Now.AddDays(30);
                    contractVM.GasLimit = "67412";
                    contractVM.Network = Network.Ropsten;
                    break;
            }


            
            

            return PartialView("~/Views/Home/_DeployContract.cshtml", contractVM);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Index(ContractVM contractVM)
        //{

        //    ContractParam contractParam = new ContractParam(contractVM.DeployContractVM);
        //    ContractAccess contractAccess = new ContractAccess();
        //    try
        //    {
        //        var transactionHash = await contractAccess.DeployContract(contractParam);
        //        contractVM.Message = transactionHash.ToString();
        //        var receipt = await contractAccess.PollReceipt(transactionHash);
        //        contractVM.DeployContractVM.ContractAddress = receipt.ContractAddress;
        //        Escrow newContract = new Escrow(transactionHash.ToString(), contractVM.DeployContractVM);
        //        newContract.Receipt = new Receipt(receipt);
        //        using (var _context = new ApplicationDbContext())
        //        {
        //            _context.DeployReceipt.Add(newContract.Receipt);
        //            _context.DeployedContracts.Add(newContract);

        //            await _context.SaveChangesAsync();
        //            contractVM.ContractList = await _context.DeployedContracts.Include(p => p.Receipt).ToListAsync();
        //        }
        //        contractVM.Receipt = new Receipt(receipt);


        //    }
        //    catch (Exception e)
        //    {
        //        contractVM.Message = e.Message;
        //        contractVM.Flag = StatusFlag.Error;
        //    }

        //    return View(contractVM);
        //}


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

