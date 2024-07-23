using CarAuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CarAuctionService.Data
{
    public class AuctionDbContext: DbContext
    {
        public AuctionDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Auction> Auctions { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    IConfigurationRoot configuration = new ConfigurationBuilder()
        //   .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        //   .AddJsonFile("appsettings.json")
        //   .Build();
        //    optionsBuilder.UseSqlServer(configuration.GetConnectionString("RKPdata"));
        //}
    }
}
