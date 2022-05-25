using EscrowService.Models;
using Microsoft.EntityFrameworkCore;

namespace EscrowService.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trader>()
                .HasOne(e => e.User)
                .WithOne(e => e.Trader)
                .HasForeignKey<Trader>(e => e.UserId);
        }
        

        public DbSet<User> Users { get; set; }
        public DbSet<Trader> Traders { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Dispute> Disputes { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<TradersTransaction> TradersTransactions { get; set; }
    }
}