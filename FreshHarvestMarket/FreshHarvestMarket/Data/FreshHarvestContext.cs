using FreshHarvestMarket.Models;
using Microsoft.EntityFrameworkCore;

namespace FreshHarvestMarket.Data
{
    /// <summary>
    /// Database connectivity for the Produce class
    /// </summary>
    public class FreshMarketContext : DbContext
    {
        public FreshMarketContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// Object representation of the Produce table
        /// </summary>
        public DbSet<Produce> Produce { get; set; } = null!;

        /// <summary>
        /// Object representation of the Order table
        /// </summary>
        public DbSet<Order> Orders { get; set; } = null!;

        /// <summary>
        /// Seeds all of the data when we create/update the database
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Produce>().HasData(
                new Produce()
                {
                    ProduceId = 1,
                    ProduceName = "Zuccihini",
                    ProduceDescription = "Fresh green zucchinis",
                    UnitPrice = 1.20m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 20
                },
                new Produce()
                {
                    ProduceId = 2,
                    ProduceName = "Tomatoes",
                    ProduceDescription = "Ripe red tomatoes",
                    UnitPrice = 2.00m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 25
                },
                new Produce()
                {
                    ProduceId = 3,
                    ProduceName = "Honey",
                    ProduceDescription = "Local raw honey",
                    UnitPrice = 6.00m,
                    ProduceCategory = "Other",
                    InventoryTotal = 15
                },
                new Produce()
                {
                    ProduceId = 4,
                    ProduceName = "Plums",
                    ProduceDescription = "Juicy plums",
                    UnitPrice = 2.50m,
                    ProduceCategory = "Fruit",
                    InventoryTotal = 18
                },
                new Produce()
                {
                    ProduceId = 5,
                    ProduceName = "Potatoes",
                    ProduceDescription = "Freshly harvested potatoes",
                    UnitPrice = 1.20m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 30
                },
                new Produce()
                {
                    ProduceId = 6,
                    ProduceName = "Blueberries",
                    ProduceDescription = "Sweet blueberries",
                    UnitPrice = 3.00m,
                    ProduceCategory = "Fruit",
                    InventoryTotal = 12
                },
                new Produce()
                {
                    ProduceId = 7,
                    ProduceName = "Sweet Corn",
                    ProduceDescription = "Fresh sweet corn",
                    UnitPrice = 1.75m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 20
                },
                new Produce()
                {
                    ProduceId = 8,
                    ProduceName = "Broccoli",
                    ProduceDescription = "Green broccoli florets",
                    UnitPrice = 2.25m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 15
                },
                new Produce()
                {
                    ProduceId = 9,
                    ProduceName = "Garlic",
                    ProduceDescription = "Fresh garlic bulbs",
                    UnitPrice = 0.80m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 40
                },
                new Produce()
                {
                    ProduceId = 10,
                    ProduceName = "Cherries",
                    ProduceDescription = "Sweet red cherries",
                    UnitPrice = 3.50m,
                    ProduceCategory = "Fruit",
                    InventoryTotal = 10
                },
                new Produce()
                {
                    ProduceId = 11,
                    ProduceName = "Carrots",
                    ProduceDescription = "Organic carrots",
                    UnitPrice = 1.50m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 25
                },
                new Produce()
                {
                    ProduceId = 12,
                    ProduceName = "Raspberries",
                    ProduceDescription = "Fresh raspberries",
                    UnitPrice = 3.00m,
                    ProduceCategory = "Fruit",
                    InventoryTotal = 12
                }
            );

            modelBuilder.Entity<Order>)().HasData(
                new Order() 
                {
                    OrderId = 1,
                    UserId = 1

                }
                );
        }
    }
}
