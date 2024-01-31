using AuctionService.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Auction_Service.Data
{
    public class AuctionDbContext : DbContext
    {
        
        public AuctionDbContext(DbContextOptions options) : base(options) 
        { 

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }

        public DbSet<Auction> Auctions { get; set; }
    }
}
