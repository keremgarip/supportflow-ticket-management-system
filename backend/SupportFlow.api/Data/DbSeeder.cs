using Microsoft.EntityFrameworkCore;
using SupportFlow.Api.Models;

namespace SupportFlow.Api.Data;

public class DbSeeder
{
    private readonly AppDbContext _context;

    public DbSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(
        CancellationToken cancellationToken = default)
    {
        await SeedCategoriesAsync(cancellationToken);
    }

    private async Task SeedCategoriesAsync(
        CancellationToken cancellationToken)
    {
        var categorySeeds = new[]
        {
            new
            {
                Name = "Technical Issue",
                Description =
                    "Technical problems involving software, hardware or system usage."
            },
            new
            {
                Name = "Billing",
                Description =
                    "Questions and problems related to invoices, payments or charges."
            },
            new
            {
                Name = "Account",
                Description =
                    "Problems related to user accounts, login or account settings."
            },
            new
            {
                Name = "Bug Report",
                Description =
                    "Reports of unexpected application behaviour or software defects."
            },
            new
            {
                Name = "Feature Request",
                Description =
                    "Suggestions for new features or improvements."
            },
            new
            {
                Name = "General Question",
                Description =
                    "Support questions that do not belong to another category."
            }
        };

        var existingNames = await _context.TicketCategories
            .AsNoTracking()
            .Select(category => category.Name)
            .ToListAsync(cancellationToken);

        var existingNameSet = new HashSet<string>(
            existingNames,
            StringComparer.OrdinalIgnoreCase);

        var missingCategories = categorySeeds
            .Where(seed => !existingNameSet.Contains(seed.Name))
            .Select(seed => new TicketCategory
            {
                Name = seed.Name,
                Description = seed.Description,
                IsActive = true
            })
            .ToList();

        if (missingCategories.Count == 0)
        {
            return;
        }

        await _context.TicketCategories.AddRangeAsync(
            missingCategories,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}