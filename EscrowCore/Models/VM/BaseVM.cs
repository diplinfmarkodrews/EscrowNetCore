using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EscrowCore.Models.VM
{
    public enum StatusFlag
    {
        Success,
        Warning,
        Info,
        Error
    }
    public class BaseVM
    {
        public string Message { get; set; }
        public StatusFlag Flag { get; set; }
    }
}
