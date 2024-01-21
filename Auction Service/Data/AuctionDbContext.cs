using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auction_Service.Data
{
    public class AuctionDbContext : DbContext
    {
        
        public AuctionDbContext(DbContextOptions options) : base(options) 
        { 

        }
        
        
        public DbSet<Auction> Auctions { get; set; }
    }
}
