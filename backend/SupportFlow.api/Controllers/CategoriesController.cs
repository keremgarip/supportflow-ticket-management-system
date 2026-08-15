using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupportFlow.Api.DTOs.Categories;
using SupportFlow.Api.Interfaces;
using SupportFlow.Api.Helpers;

namespace SupportFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(IReadOnlyList<CategoryDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetAll(
        CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetAllAsync(
            cancellationToken);

        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(
        typeof(CategoryDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryDto>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var category = await _categoryService.GetByIdAsync(
            id,
            cancellationToken);

        if (category is null)
        {
            return NotFound(new
            {
                success = false,
                message = $"Category with ID {id} was not found."
            });
        }

        return Ok(category);
    }

    [Authorize(Policy = AppPolicies.AdminOnly)]
    [HttpPost]
    [ProducesResponseType(
        typeof(CategoryDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CategoryDto>> Create(
        CreateCategoryDto dto,
        CancellationToken cancellationToken)
    {
        var nameExists = await _categoryService.NameExistsAsync(
            dto.Name,
            cancellationToken: cancellationToken);

        if (nameExists)
        {
            return Conflict(new
            {
                success = false,
                message = "A category with the same name already exists."
            });
        }

        var category = await _categoryService.CreateAsync(
            dto,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = category.Id },
            category);
    }

    [Authorize(Policy = AppPolicies.AdminOnly)]
    [HttpPut("{id:int}")]
    [ProducesResponseType(
        typeof(CategoryDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CategoryDto>> Update(
        int id,
        UpdateCategoryDto dto,
        CancellationToken cancellationToken)
    {
        var existingCategory = await _categoryService.GetByIdAsync(
            id,
            cancellationToken);

        if (existingCategory is null)
        {
            return NotFound(new
            {
                success = false,
                message = $"Category with ID {id} was not found."
            });
        }

        var nameExists = await _categoryService.NameExistsAsync(
            dto.Name,
            id,
            cancellationToken);

        if (nameExists)
        {
            return Conflict(new
            {
                success = false,
                message = "A category with the same name already exists."
            });
        }

        var updatedCategory = await _categoryService.UpdateAsync(
            id,
            dto,
            cancellationToken);

        return Ok(updatedCategory);
    }

    [Authorize(Policy = AppPolicies.AdminOnly)]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var deleted = await _categoryService.DeleteAsync(
            id,
            cancellationToken);

        if (!deleted)
        {
            return NotFound(new
            {
                success = false,
                message = $"Category with ID {id} was not found."
            });
        }

        return NoContent();
    }
}