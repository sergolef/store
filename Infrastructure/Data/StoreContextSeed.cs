using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context){
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!context.ProductBrands.Any()){
                var brandData = File.ReadAllText(path + @"/Data/SeedData/brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                context.ProductBrands.AddRange(brands);
            }

            if (!context.ProductTypes.Any()){
                var typeData = File.ReadAllText(path + @"/Data/SeedData/types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typeData);
                context.ProductTypes.AddRange(types);
            }

            if(!context.Products.Any()){
                var productsData = File.ReadAllText(path + @"/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                context.Products.AddRange(products);
            }

             if(!context.DeliveryMethods.Any()){
                var productsData = File.ReadAllText(path + @"/Data/SeedData/delivery.json");
                var dmethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(productsData);
                context.DeliveryMethods.AddRange(dmethods);
            }

            if(context.ChangeTracker.HasChanges()){
                await context.SaveChangesAsync();
            }
        }

    }
}