using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EscrowCore.Models.VM
{
    
    public class DeployContractVM
    {
        [Required]
        public string SellerAddress { get; set; }
        [Required]
        public string BuyerAddress { get; set; }
        [Required]
        public string EscrowHolderAddress { get; set; }
        [Required]
        public string EtherToPay { get; set; }
        [Required]
        public string GasLimit { get; set; }
        [Required]
        public DateTime ExpiryDate { get; set; }
        public string ContractAddress { get; set; }
        public int Network { get; set; } 
    }
}
