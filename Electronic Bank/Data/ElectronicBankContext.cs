using Electronic_Bank.Models;
using Microsoft.EntityFrameworkCore;

namespace Electronic_Bank.Data
{
    public class ElectronicBankContext : DbContext
    {
        public ElectronicBankContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Currency> Currencys { get; set; }

    }
} 
