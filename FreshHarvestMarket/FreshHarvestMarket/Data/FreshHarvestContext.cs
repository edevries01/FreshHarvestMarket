using FreshHarvestMarket.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FreshHarvestMarket.Data
{
    /// <summary>
    /// Database connectivity for the Produce class
    /// </summary>
    public class FreshHarvestContext : IdentityDbContext<User>
    {
        public FreshHarvestContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// Representation of the Produce table
        /// </summary>
        public DbSet<Produce> Produce { get; set; } = null!;

        /// <summary>
        /// Representation of the Order table
        /// </summary>
        public DbSet<Order> Orders { get; set; } = null!;

        /// <summary>
        /// Representations of the OrderItems table
        /// </summary>
        public DbSet<OrderItem> OrderItems { get; set; } = null!;

        /// <summary>
        /// Representation of the Favorites table
        /// </summary>
        public DbSet<Favorite> Favorites { get; set; } = null!;

        /// <summary>
        /// Representation of the discounts table
        /// </summary>
        public DbSet<Discount> Discounts { get; set; } = null!;

        /// <summary>
        /// Seeds all of the data when we create/update the database
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Ensure the 1-1 relationship of discount and produce works right
            modelBuilder.Entity<Produce>()
                .HasOne(p => p.Discount)
                .WithOne(d => d.Produce)
                .HasForeignKey<Discount>(d => d.ProduceId);

            modelBuilder.Entity<Produce>().HasData(
                new Produce()
                {
                    ProduceId = 1,
                    ProduceName = "Zucchini",
                    ProduceDescription = "Fresh green zucchini from Williamsburg, Iowa",
                    UnitPrice = 1.20m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 20,
                    ImageUrl = "zucchini.jpg"
                },
                new Produce()
                {
                    ProduceId = 2,
                    ProduceName = "Tomatoes",
                    ProduceDescription = "Ripe red tomatoes from Victor, Iowa",
                    UnitPrice = 2.00m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 25,
                    ImageUrl = "tomatoes.jpg"
                },
                new Produce()
                {
                    ProduceId = 3,
                    ProduceName = "Honey",
                    ProduceDescription = "Local raw honey from Victor, Iowa",
                    UnitPrice = 6.00m,
                    ProduceCategory = "Other",
                    InventoryTotal = 15,
                    ImageUrl = "honey.jpg"
                },
                new Produce()
                {
                    ProduceId = 4,
                    ProduceName = "Plums",
                    ProduceDescription = "Juicy plums from Amana, Iowa",
                    UnitPrice = 2.50m,
                    ProduceCategory = "Fruit",
                    InventoryTotal = 18,
                    ImageUrl = "plums.jpg"
                },
                new Produce()
                {
                    ProduceId = 5,
                    ProduceName = "Potatoes",
                    ProduceDescription = "Freshly harvested potatoes from Ladora, Iowa",
                    UnitPrice = 1.20m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 30,
                    ImageUrl = "potatoes.jpg"
                },
                new Produce()
                {
                    ProduceId = 6,
                    ProduceName = "Blueberries",
                    ProduceDescription = "Sweet blueberries from Tiffin, Iowa",
                    UnitPrice = 3.00m,
                    ProduceCategory = "Fruit",
                    InventoryTotal = 12,
                    ImageUrl = "blueberries.jpg"
                },
                new Produce()
                {
                    ProduceId = 7,
                    ProduceName = "Sweet Corn",
                    ProduceDescription = "Fresh sweet corn from Marengo, Iowa",
                    UnitPrice = 1.75m,
                    InventoryTotal = 20,
                    ImageUrl = "sweetcorn.jpg"
                },
                new Produce()
                {
                    ProduceId = 8,
                    ProduceName = "Broccoli",
                    ProduceDescription = "Green broccoli florets from Millersburg, Iowa",
                    UnitPrice = 2.25m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 15,
                    ImageUrl = "broccoli.jpg"
                },
                new Produce()
                {
                    ProduceId = 9,
                    ProduceName = "Garlic",
                    ProduceDescription = "Fresh garlic bulbs from Montezuma, Iowa",
                    UnitPrice = 0.80m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 40,
                    ImageUrl = "garlic.jpg"
                },
                new Produce()
                {
                    ProduceId = 10,
                    ProduceName = "Cherries",
                    ProduceDescription = "Sweet red cherries from Solon, Iowa",
                    UnitPrice = 3.50m,
                    ProduceCategory = "Fruit",
                    InventoryTotal = 10,
                    ImageUrl = "cherries.jpg"
                },
                new Produce()
                {
                    ProduceId = 11,
                    ProduceName = "Carrots",
                    ProduceDescription = "Organic carrots from Kalona, Iowa",
                    UnitPrice = 1.50m,
                    ProduceCategory = "Vegetable",
                    InventoryTotal = 25,
                    ImageUrl = "carrots.jpg"
                },
                new Produce()
                {
                    ProduceId = 12,
                    ProduceName = "Raspberries",
                    ProduceDescription = "Fresh raspberries from Swisher, Iowa",
                    UnitPrice = 3.00m,
                    ProduceCategory = "Fruit",
                    InventoryTotal = 12,
                    ImageUrl = "raspberries.jpg"
                }
            );

            modelBuilder.Entity<Order>().HasData(
                new Order()
                {
                    OrderId = 1,
                    OrderTotal = 12.50m,
                    OrderDate = new DateTime(2026, 3, 1),
                    PickupDate = new DateTime(2026, 3, 2),
                    IsPickedUp = false
                },
                new Order()
                {
                    OrderId = 2,
                    OrderTotal = 22.00m,
                    OrderDate = new DateTime(2026, 3, 3),
                    PickupDate = new DateTime(2026, 3, 4),
                    IsPickedUp = false
                }
            );

            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem
                {
                    OrderItemId = 1,
                    OrderId = 1,
                    ProduceId = 1,
                    Quantity = 3
                },
                new OrderItem
                {
                    OrderItemId = 2,
                    OrderId = 1,
                    ProduceId = 2,
                    Quantity = 2
                },
                new OrderItem
                {
                    OrderItemId = 3,
                    OrderId = 2,
                    ProduceId = 3,
                    Quantity = 4
                }
            );

            modelBuilder.Entity<Discount>().HasData(
                new Discount
                {
                    DiscountId = 1,
                    DiscountAmount = 50,
                    ProduceId = 1
                },
                new Discount
                {
                    DiscountId = 2,
                    DiscountAmount = 3,
                    ProduceId = 3
                });


        }

        /// <summary>
        /// Taken from textbook
        /// Seeds an admin user, creates admin role if it does not exist
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            using (var scoped = serviceProvider.CreateScope())
            {
                UserManager<User> userManager = scoped.ServiceProvider.GetRequiredService<UserManager<User>>();
                RoleManager<IdentityRole> roleManager = scoped.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                string username = "admin";
                string pwd = "Capybara";
                string roleName = "Admin";

                // if role doesn't exist, create it
                if (await roleManager.FindByNameAsync(roleName) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }

                if (await userManager.FindByNameAsync(username) == null)
                {
                    User user = new User() { UserName = username };
                    var result = await userManager.CreateAsync(user, pwd);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, roleName);
                    }
                }
            }
        }
    }
}
