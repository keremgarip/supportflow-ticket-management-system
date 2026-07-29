using SupportFlow.Api.DTOs.Categories;

namespace SupportFlow.Api.Interfaces;

public interface ICategoryService
{
    Task<IReadOnlyList<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<CategoryDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<CategoryDto> CreateAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default);
    Task<CategoryDto?> UpdateAsync(int id, UpdateCategoryDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> NameExistsAsync(string name, int? excludedCategoryId = null, CancellationToken cancellationToken = default);
}