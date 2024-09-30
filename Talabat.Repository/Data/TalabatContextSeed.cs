using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.OrderAggregate;

namespace Talabat.Repository.Data
{
    public static class TalabatContextSeed
    {
        public static async Task SeedDataAsync(TalabatContext dbContext)
        {
            if (!dbContext.ProductBrands.Any()) {
                //Seed Brand Data
                var BrandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
                if (Brands?.Count > 0)
                {
                    foreach (var Brand in Brands)
                    {
                        await dbContext.ProductBrands.AddAsync(Brand);
                    }
                }
            }
            // Seed Type Data
            if (!dbContext.ProductTypes.Any()) {
                var TypesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypesData);
                if (Types?.Count > 0)
                {
                    foreach (var Type in Types)
                    {
                        await dbContext.ProductTypes.AddAsync(Type);
                    }
                }
            }
            // Seed Product Data
            if (!dbContext.Products.Any()) {
                var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                if (Products?.Count > 0)
                {
                    foreach (var Product in Products)
                    {
                        await dbContext.Products.AddAsync(Product);
                    }
                }
            }
            if (!dbContext.DeliveryMethods.Any())
            {
                var DeliveryMethodData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodData);
                if (DeliveryMethods?.Count>0)
                {
                    foreach (var Method in DeliveryMethods)
                    {
                        await dbContext.DeliveryMethods.AddAsync(Method);
                    }
                }
            }
            await dbContext.SaveChangesAsync();
        }
    }
}
