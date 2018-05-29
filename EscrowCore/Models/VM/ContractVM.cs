using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EscrowCore.Models.VM
{
    public class ContractVM : BaseVM
    {
        public DeployContractVM DeployContractVM { get; set; }
        public Receipt Receipt { get; set; }
        public IEnumerable<Escrow> ContractList { get; set; }
        public ContractVM()
        {
            DeployContractVM = new DeployContractVM();
            Receipt = new Receipt();
            ContractList = new List<Escrow>();
        }
    }
}
