using Ecommerce.Models;
using Microsoft.AspNetCore.Identity;


namespace RunGroupWebApp.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                if (!context.Product.Any())
                {
                    context.Product.AddRange(new List<Product>()
                    {
                        new Product()
                        {
                            Title = "Starbucks House Blend 200g",
                            ProfileImageUrl = "https://assets.sainsburys-groceries.co.uk/gol/7738859/1/640x640.jpg",
                            Description = "Indulge in Starbucks® House Blend Medium Roast Ground Coffee. A blend of fine Latin American beans roasted to a glistening, dark chestnut colour. With aroma, body and flavour all in balance with tastes of nuts and cocoa, Starbucks® Medium Roast coffees are smooth, balanced and rich with toffee notes.",
                            Price = 4
                            
                         },
                        new Product()
                        {
                            Title = "Miles - Kenya 227g",
                            ProfileImageUrl = "https://www.milesteaandcoffee.com/userfiles/product_image/645ce3c063196-kenya.jpg",
                            Description = "Kenya produces huge volumes of coffee every year and, once tasted, you'll soon see why. These single origin, 100% Arabica ground coffee beans have a distinct wine-like characteristic, with fruity notes and a consistent rich flavour. Our Kenya beans are roasted for a light to medium strength coffee.",
                            Price = 5
                        },
                        new Product()
                        {
                            Title = "Forest Edge Blend 100g",
                            ProfileImageUrl = "https://cdn.shopify.com/s/files/1/1689/7585/products/IMG_20210513_160030_2.jpg?v=1624967583",
                            Description = "With notes of dark chocolate and nut, this ground coffee by Forest Edge Roasting Co. is the perfect addition to a gift box for any coffee connoisseur. Ethically sourced and roasted in small batches.",
                            Price = 3
                        },
                        new Product()
                        {
                            Title = "Tim Hortons original blend 340g",
                            ProfileImageUrl = "https://i5.walmartimages.com/asr/0f3be485-ac36-4607-ad08-5a80581a9441.fb751ba64a828ebec05b77d79c66d9c7.jpeg",
                            Description = "Good neighbors wave. Great neighbors share a cup of Tim Hortons. For over 50 years, Tim Hortons has been making a great cup of coffee with 100% premium Arabica beans. We're proud to share Canada's favorite coffee with our favorite neighbors.",
                            Price = 6
                        }
                    });
                    context.SaveChanges();
                }
               
            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "admin@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "admin",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        Name = "addy aderson",
                        Address = new Address()
                        {
                            Street = "123 Main St",
                            City = "paradise city",
                            PostCode = "ddr6h4",
                        }
                    };
                    await userManager.CreateAsync(newAdminUser, "Tester99;;");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "test@gmail.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new AppUser()
                    {
                        UserName = "user",
                        Email = appUserEmail,
                        EmailConfirmed = true,
                        Name = "john doe",
                        Address = new Address()
                        {
                            Street = "123 other St",
                            City = "big city",
                            PostCode = "dfr6j8",
                        }
                    };
                    await userManager.CreateAsync(newAppUser, "Tester99;;");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}