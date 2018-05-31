﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EscrowCore.Models
{
    public enum EscrowStatus
    {
        Created,
        Funded,
        Refunded,
        Expired,
        Released
    }
    public class Escrow
    {
        public int ID { get; set; }
        public string ContractAddress { get; set; }
        public string SellerAddress { get; set; }
        public string BuyerAddress { get; set; }
        public string EscrowHolderAddress { get; set; }
        public string TransactionHash { get; set; }
        public Receipt Receipt { get; set; }
        public Escrow() { }
        public Escrow(string txHash, Models.VM.DeployContractVM deploy)
        {
            ContractAddress = deploy.ContractAddress;
            SellerAddress = deploy.SellerAddress;
            BuyerAddress = deploy.BuyerAddress;
            EscrowHolderAddress = deploy.EscrowHolderAddress;
            TransactionHash = txHash;
        }
    }
    public class Receipt
    {
        public Receipt(Nethereum.RPC.Eth.DTOs.TransactionReceipt receipt)
        {


            ContractAddress = receipt.ContractAddress.ToString();
            BlockHash = receipt.BlockHash.ToString();
            BlockNumber = receipt.BlockNumber.HexValue;
            GasUsed = receipt.GasUsed.HexValue;
            Status = receipt.Status.HexValue;
        }
        public Receipt() { }
        public int ID { get; set; }
        public string ContractAddress { get; set; }
        public string BlockNumber { get; set; }
        public string BlockHash { get; set; }
        public string GasUsed { get; set; }
        public string Status { get; set; }

    }

}