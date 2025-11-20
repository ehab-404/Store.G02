using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Entities.Products;
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contexts;
using Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Domain.Entities.Identity;
using Domain.Entities.Orders;
using Domain.Entities;

namespace Persistence
{
    public class DbInitializer(StoreDbContext _context,
         StoreIdentityDbContext _identityContext,
         UserManager<AppUser> _userManager,
         RoleManager<IdentityRole> _roleManager) : IDbInitializer
    {
        public async Task InitializeAsync()
        {
            // check db exists or not. if not, it creates it and apply pending migration
            var pendingMigration = await _context.Database.GetPendingMigrationsAsync();

            if (pendingMigration.Any())
                await _context.Database.MigrateAsync();


            // Seeding
            if (!_context.ProductBrands.Any())
            {
                await SeedData<ProductBrand, int>("brands");
            }

            if (!_context.ProductTypes.Any())
            {
                await SeedData<ProductType, int>("types");
            }

            if (!_context.Products.Any())
            {
                await SeedData<Product, int>("products");
            }

            if (!_context.DeliveryMethods.Any())
            {
                await SeedData<DeliveryMethod, int>("delivery");
            }

            await _context.SaveChangesAsync();
        }

        public async Task InitializeIdentityAsync()
        {
            var identityPendingMigration = await _identityContext.Database.GetPendingMigrationsAsync();

            if (identityPendingMigration.Any())
                await _identityContext.Database.MigrateAsync();

            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "SuperAdmin"
                });
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "Admin"
                });
            }

            if (!_userManager.Users.Any())
            {
                var superAdmin = new AppUser()
                {
                    DiplayName = "SuperAdmin",
                    UserName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    PhoneNumber = "01098566020"
                };
                var admin = new AppUser()
                {
                    DiplayName = "Admin",
                    UserName = "Admin",
                    Email = "Admin@gmail.com",
                    PhoneNumber = "01120206311"
                };

                await _userManager.CreateAsync(superAdmin, "Pa$$w0rd");
                await _userManager.CreateAsync(admin, "Pa$$w0rd");

                await _userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                await _userManager.AddToRoleAsync(admin, "Admin");
            }
        }

        private async Task SeedData<TData, TKey>(string jsonName) where TData : BaseEntity<TKey>
        {
            var basePath = Directory.GetCurrentDirectory();

            //var fullPath = Path.Combine(basePath,"Infrastructure", "Persistence","Data", "DataSeeding", $"{jsonName}.json");
            var jsonData = await File.ReadAllTextAsync($"..\\Infrastructure\\Persistence\\Data\\DataSeeding\\{jsonName}.json");
            var dataList = JsonSerializer.Deserialize<List<TData>>(jsonData);

            if (dataList is not null && dataList.Count != 0)
                await _context.Set<TData>().AddRangeAsync(dataList);
        }
    }
}
