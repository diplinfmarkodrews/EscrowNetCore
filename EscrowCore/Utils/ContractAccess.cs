using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum;
using Newtonsoft.Json.Linq;
using Nethereum.Parity;
using EscrowCore.Models.VM;


namespace EscrowCore.Utils
{
    public class ContractParam
    {
        public ContractParam(DeployContractVM vM)
        {

            SenderAddress = vM.SellerAddress;
            GasLimit = new Nethereum.Hex.HexTypes.HexBigInteger(vM.GasLimit);
            var amount = new Nethereum.Hex.HexTypes.HexBigInteger(vM.EtherToPay);
            GetV = new object[3];
            GetV[0] = vM.BuyerAddress;
            GetV[1] = vM.EscrowHolderAddress;
            GetV[2] = vM.EtherToPay;

        }

        public object[] GetV { get; set; }
        public string SenderAddress { get; set; }
        public Nethereum.Hex.HexTypes.HexBigInteger GasLimit { get; set; }

    }
    public class ContractModel
    {
        public string ABI { get; private set; }
        public string ByteCode { get; private set; }
        public string DeployedByteCode { get; private set; }
        public string Source { get; set; }
        public string Path { get; set; }
        public ContractModel(string path)
        {

            readContractFromFile(path);
        }
        private bool readContractFromFile(string path)
        {
            try
            {

                string contents = System.IO.File.ReadAllText(path);
                var contractjs = (JObject)JsonConvert.DeserializeObject(contents);
                ABI = contractjs["abi"].ToString();
                ByteCode = contractjs["bytecode"].ToString();
                DeployedByteCode = contractjs["deployedBytecode"].ToString();
                Source = contractjs["source"].ToString();
                Path = path;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
    public class AccountAccess
    {
        public List<Nethereum.HdWallet.Wallet> Wallets;
        public AccountAccess()
        {
            Wallets = new List<Nethereum.HdWallet.Wallet>();
            
            for (int i = 0; i < 10; i++)
            {
               
                
                Wallets.Add(new Nethereum.HdWallet.Wallet("credit program number case jar embrace miracle find sleep display light razor", "m / 44'/60' / 0'/0/" + i));

            }
        }

    }
    public class ChainAccess
    {
        public Nethereum.Parity.Web3Parity Web3 { get; set; }
        public ChainAccess(AccountAccess account, string urlNode)
        {
            //Web3 = new Web3Parity(account.Wallets.FirstOrDefault(), "http://localhost:8180");
        }

    }
    public class ContractAccess
    {
        //Ganache
        private static string privKeySeller1 = "b41e3d6f337d1c67269dccd0aa917aad534ec641bcfdf562b1eed6fddc1fe652";
        private static string privKeyBuyer2 = "b9a5ab341cce3af5c4844b1eae23bdfbe21314705ef4aac4303e391d8edfccae";
        private static string privKeyEscrow3 = "23f3c3c52361eb074cf990fc0410609d0746d1c4004131f7b2804526c4dbb07f";


        private const string path = @"..\truffle\build\contracts\Escrow.json";
        private ContractModel ContractModel;
      
        private static Nethereum.Web3.Accounts.Account accountGanache;
       
        

        private static Nethereum.Web3.Accounts.Managed.ManagedAccount accountRopsten;
        private static Web3 web3;
        private static string account = "0x005e44B5ce1E91c2ee3b6e13B52F174b664b8124";
        private static string password = "c4Gpy05647gtWQp056";

        public ContractAccess()
        {
           
          
            //accountGanache = new Nethereum.Web3.Accounts.Account(privKeySeller1);
            accountRopsten = new Nethereum.Web3.Accounts.Managed.ManagedAccount(account, password);
            web3 = new Web3(accountRopsten, "http://localhost:8545");// = new Web3(accountGanache, "http://localhost:7545"); //Account #3 Ganache
            ContractModel = new ContractModel(path);
        }
        public async Task<string> DeployContract(ContractParam param)
        {
            
            var transActionHash = await web3.Eth.DeployContract.SendRequestAsync(ContractModel.ABI, ContractModel.ByteCode, param.SenderAddress, param.GasLimit, param.GetV);
            return transActionHash;

        }
        public async Task<string> GetGasLimit(ContractParam param)
        {
            Nethereum.Hex.HexTypes.HexBigInteger gasEstimate = await web3.Eth.DeployContract.EstimateGasAsync(ContractModel.ABI.ToString(), ContractModel.ByteCode.ToString(), param.SenderAddress, param.GetV);
            return gasEstimate.ToString();
        }
        public async Task<Nethereum.RPC.Eth.DTOs.TransactionReceipt> PollReceipt(string transactionHash)
        {
            try
            {
                var receipt = await web3.TransactionManager.TransactionReceiptService.PollForReceiptAsync(transactionHash);

                while (receipt == null)
                {
                    Thread.Sleep(5000);
                    receipt = await web3.TransactionManager.TransactionReceiptService.PollForReceiptAsync(transactionHash);
                }

                return receipt;
            }
            catch (Exception e)
            {
                return null;
            }
        }


    }


}

