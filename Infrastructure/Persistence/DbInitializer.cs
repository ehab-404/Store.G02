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

namespace Persistence
{
    public class DbInitializer(StoreDbContext _context , 
        StoreIdentityDbContext _storeIdentityDbContext
        
        , UserManager<AppUser> _userManager,

        RoleManager<IdentityRole> _roleManager
        ): IDbInitializer
    {
        //private readonly StoreDbContext _context;

        //public DbInitializer(StoreDbContext context)
        //{
        //    _context = context;
        //}

        public async Task InitializeAsync( )
        {
            //create db if not created 
            // and update db
           

            if (_context.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any())
            {
               await _context.Database.MigrateAsync();

            }



            //data seeding  if tables are empety


            //product brands

        if(!_context.productBrands.Any())
            {


                // 1. read all data from json file as json string  

                var brandsdata = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\DataSeeding\brands.json");


                //2. convert json string to List<ProductBrand> 

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsdata);

                if (brands is not null && brands.Count > 0)
                {

                    //add list to db if it is not null 
                    await _context.productBrands.AddRangeAsync(brands);


                }

            }






            //product type



            if (!_context.productTypes.Any())
            {


                // 1. read all data from json file as json string  

                var typesdata = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\DataSeeding\types.json");


                //2. convert json string to List<Producttypes> 

                var types = JsonSerializer.Deserialize<List<ProductType>>(typesdata);

                if (types is not null && types.Count > 0)
                {

                    //add list to db if it is not null 
                    await _context.productTypes.AddRangeAsync(types);


                }

            }







            //product




            if (!_context.products.Any())
            {


                // 1. read all data from json file as json string  

                var productsdata = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\DataSeeding\products.json");


                //2. convert json string to List<ProductBrand> 

                var products = JsonSerializer.Deserialize<List<Product>>(productsdata);

                if (products is not null && products.Count > 0)
                {

                    //add list to db if it is not null 
                    await _context.products.AddRangeAsync(products);


                }

            }




            //seeding delivery methods


            // 1. read all data from json file as json string  

            var deliveryMethodsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\DataSeeding\delivery.json");


            //2. convert json string to List<DeliveryMethod> 

            var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryMethodsData);

            if (deliveryMethods is not null && deliveryMethods.Count > 0)
            {

                //add list to db if it is not null 
                await _context.deliveryMethods.AddRangeAsync(deliveryMethods);


            }


            await   _context.SaveChangesAsync();





        }

        public async Task InitializeIdentityAsync()
        {
            // create if not exists and apply any pending migrations 

            if(_storeIdentityDbContext.Database.GetAppliedMigrations().Any())
            {

               await _storeIdentityDbContext.Database.MigrateAsync();




            }


            if(! _roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole()


                {
                    Name = "Admin"

                });


                await _roleManager.CreateAsync(new IdentityRole()


                {
                    Name = "SuperAdmin"

                });

            }

            //seeding 


            if(! _userManager.Users.Any())
            {

                var superAdminUser = new AppUser()
                {

                    DisplayName = "Super Admin",
                    Email = "Popehab057@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "01120206311"



                };

                var AdminUser = new AppUser()
                {

                    DisplayName = "Admin",
                    Email = "ehababdelmohsen01@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "01098566020"



                };



                await _userManager.CreateAsync(superAdminUser, "P@ssw0rd");
                await _userManager.CreateAsync(AdminUser, "P@ssw0rd");
                await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                await _userManager.AddToRoleAsync(AdminUser, "Admin");

            }



        }
    }
}
