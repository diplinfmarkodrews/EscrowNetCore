using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using EscrowCore.Models;

namespace EscrowCore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder
                .UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Monty\source\repos\EscrowCore\EscrowCore\Data\Contracts.mdf;Integrated Security=True;Connect Timeout=30")
                .UseLoggerFactory(new LoggerFactory().AddConsole());
        }


        public DbSet<Escrow> DeployedContracts { get; set; }
        public DbSet<Receipt> DeployReceipt { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
