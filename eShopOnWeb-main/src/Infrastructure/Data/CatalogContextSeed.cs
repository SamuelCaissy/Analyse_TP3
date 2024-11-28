using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Infrastructure.Data;

public class CatalogContextSeed
{
    public static async Task SeedAsync(CatalogContext catalogContext,
        ILogger logger,
        int retry = 0)
    {
        var retryForAvailability = retry;
        try
        {
            if (catalogContext.Database.IsSqlServer())
            {
                catalogContext.Database.Migrate();
            }

            if (!await catalogContext.CatalogBrands.AnyAsync())
            {
                await catalogContext.CatalogBrands.AddRangeAsync(
                    GetPreconfiguredCatalogBrands());

                await catalogContext.SaveChangesAsync();
            }

            if (!await catalogContext.CatalogTypes.AnyAsync())
            {
                await catalogContext.CatalogTypes.AddRangeAsync(
                    GetPreconfiguredCatalogTypes());

                await catalogContext.SaveChangesAsync();
            }

            if (!await catalogContext.CatalogItems.AnyAsync())
            {
                await catalogContext.CatalogItems.AddRangeAsync(
                    GetPreconfiguredItems());

                await catalogContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            if (retryForAvailability >= 10) throw;

            retryForAvailability++;
            
            logger.LogError(ex.Message);
            await SeedAsync(catalogContext, logger, retryForAvailability);
            throw;
        }
    }

    static IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
    {
        return new List<CatalogBrand>
            {
                new("Brozilla"),
                new("ALPHA MALE")
            };
    }

    static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
    {
        return new List<CatalogType>
            {
                new("Proteine"),
                new("Créatine"),
                new("Accessoire"),
                new("Vêtements")
            };
    }

    static IEnumerable<CatalogItem> GetPreconfiguredItems()
    {
        return new List<CatalogItem>
            {
                new(1,1, "Protéine Chernobyl", "Protéine Chernobyl", 32,  "http://catalogbaseurltobereplaced/images/products/Chernobyl.png"),
                new(1,1, "Protéine Farheneit", "Protéine Farheneit", 32, "http://catalogbaseurltobereplaced/images/products/Farheneit.png"),
                new(1,2, "Créatine Alpha", "Créatine Alpha", 40,  "http://catalogbaseurltobereplaced/images/products/Alpha.png"),
                new(1,1, "Protéine Hollow purple", "Protéine Hollow purple", 32, "http://catalogbaseurltobereplaced/images/products/HollowPurple.png"),
                new(1,1, "Protéine Néo", "Protéine Néo", 32, "http://catalogbaseurltobereplaced/images/products/Neo.png"),
                new(1,1, "Protéine Samurai", "Protéine Samurai", 32, "http://catalogbaseurltobereplaced/images/products/Samurai.png"),
                new(1,1, "Protéine Thunderbolt", "Protéine Thunderbolt",  32, "http://catalogbaseurltobereplaced/images/products/Thunderbolt.png"),
                new(1,1, "Protéine Trident", "Protéine Trident", 32, "http://catalogbaseurltobereplaced/images/products/Trident.png"),
                new(1,1, "Protéine Yakuza", "Protéine Yakuza", 32, "http://catalogbaseurltobereplaced/images/products/Yakuza.png"),
            };
    }
}
