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
            GetV = new object[2];
            GetV[0] = vM.BuyerAddress;
            GetV[1] = vM.SellerAddress;
            //GetV[2] = amount;

        }

        public object[] GetV { get; set; }
        public string SenderAddress { get; set; }
        public Nethereum.Hex.HexTypes.HexBigInteger GasLimit { get; set; }

    }
    public class ContractModel
    {
        public JToken ABI { get; private set; }
        public JToken ByteCode { get; private set; }
        public JToken DeployedByteCode { get; private set; }
        public JToken Source { get; set; }
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
                ABI = contractjs["abi"];
                ByteCode = contractjs["bytecode"];
                DeployedByteCode = contractjs["deployedBytecode"];
                Source = contractjs["source"];

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
    public static class ContractAccess
    {

        private const string path = @"..\truffle\build\contracts\Escrow.json";
        private static ContractModel ContractModel = new ContractModel(path);
        private static AccountAccess Account = new AccountAccess();
        private static Web3 web3 = new Web3("http://localhost:7545");



        public static async Task<string> DeployContract(ContractParam param)
        {

            var transActionHash = await web3.Eth.DeployContract.SendRequestAsync(ContractModel.ABI.ToString(), ContractModel.ByteCode.ToString(), param.SenderAddress, param.GasLimit, param.GetV);
            return transActionHash;

        }
        public static async Task<string> GetGasLimit(ContractParam param)
        {
            Nethereum.Hex.HexTypes.HexBigInteger gasEstimate = await web3.Eth.DeployContract.EstimateGasAsync(ContractModel.ABI.ToString(), ContractModel.ByteCode.ToString(), param.SenderAddress, param.GetV);
            return gasEstimate.ToString();
        }
        public static async Task<Nethereum.RPC.Eth.DTOs.TransactionReceipt> PollReceipt(string transactionHash)
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

