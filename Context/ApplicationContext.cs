using System;
using System.Linq;
using EscrowService.Auitable;
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
        
        public override int SaveChanges()
        {
            var now = DateTime.UtcNow;

            foreach (var changedEntity in ChangeTracker.Entries())
            {
                if (changedEntity.Entity is BaseEntity entity)
                {
                    switch (changedEntity.State)
                    {
                        case EntityState.Added:
                            entity.CreatedDate = now;
                            entity.UpdatedDate = now;
                            break;

                        case EntityState.Modified:
                            Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                            entity.UpdatedDate = now;
                            break;
                    }
                }
            }

            return base.SaveChanges();
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
        public DbSet<TransactionType> TransactionTypes { get; set; }
    }
}