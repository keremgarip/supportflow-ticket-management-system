using Microsoft.EntityFrameworkCore;
using SupportFlow.Api.Data;
using SupportFlow.Api.DTOs.Categories;
using SupportFlow.Api.Interfaces;
using SupportFlow.Api.Models;

namespace SupportFlow.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<CategoryDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.TicketCategories
            .AsNoTracking()
            .OrderBy(category => category.Name)
            .Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                TicketCount = category.Tickets.Count
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<CategoryDto?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _context.TicketCategories
            .AsNoTracking()
            .Where(category => category.Id == id)
            .Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                TicketCount = category.Tickets.Count
            })
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<CategoryDto> CreateAsync(
        CreateCategoryDto dto,
        CancellationToken cancellationToken = default)
    {
        var category = new TicketCategory
        {
            Name = dto.Name.Trim(),
            Description = NormalizeDescription(dto.Description),
            IsActive = true
        };

        _context.TicketCategories.Add(category);
        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(category);
    }

    public async Task<CategoryDto?> UpdateAsync(
        int id,
        UpdateCategoryDto dto,
        CancellationToken cancellationToken = default)
    {
        var category = await _context.TicketCategories
            .SingleOrDefaultAsync(
                category => category.Id == id,
                cancellationToken);

        if (category is null)
        {
            return null;
        }

        category.Name = dto.Name.Trim();
        category.Description = NormalizeDescription(dto.Description);
        category.IsActive = dto.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(id, cancellationToken);
    }

    public async Task<bool> DeleteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var category = await _context.TicketCategories
            .Include(category => category.Tickets)
            .SingleOrDefaultAsync(
                category => category.Id == id,
                cancellationToken);

        if (category is null)
        {
            return false;
        }

        if (category.Tickets.Count > 0)
        {
            category.IsActive = false;
        }
        else
        {
            _context.TicketCategories.Remove(category);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> NameExistsAsync(
        string name,
        int? excludedCategoryId = null,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Trim().ToLower();

        return await _context.TicketCategories
            .AsNoTracking()
            .AnyAsync(
                category =>
                    category.Name.ToLower() == normalizedName &&
                    (!excludedCategoryId.HasValue ||
                     category.Id != excludedCategoryId.Value),
                cancellationToken);
    }

    private static CategoryDto MapToDto(TicketCategory category)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive,
            TicketCount = category.Tickets.Count
        };
    }

    private static string? NormalizeDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return null;
        }

        return description.Trim();
    }

    Task<CategoryDto?> ICategoryService.DeleteAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}