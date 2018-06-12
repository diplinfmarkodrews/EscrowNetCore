using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Ipfs.Api;
namespace EscrowCore.Controllers
{
    [Produces("application/json")]
    [Route("api/IPFS")]
    public class IPFSController : Controller
    {
        public IPFSController()
        {
            var client = new IpfsClient();//add host of ipfsnode here as string

        }
    }
}