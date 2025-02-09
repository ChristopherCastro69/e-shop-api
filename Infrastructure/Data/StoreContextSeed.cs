using System;
using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;


//Instead of using the .net cli to update our database, we can simply start our app and apply pending migratioons, and if need can seed into database
public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        if(!context.Products.Any())
        {
            var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");

            var products = JsonSerializer.Deserialize<List<Product>> (productsData);

            if(products == null )return;

            context.Products.AddRange(products);

            await context.SaveChangesAsync();
        }
    }
}
