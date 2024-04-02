using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Repository;
using backend.ViewModel;
using backend.ViewModel.IngredientDTO;

namespace backend.Controllers;


[Route("api/[controller]")]
[ApiController]
public class IngredientsController : ControllerBase
{
    private readonly ApplicationContext _context;

    public IngredientsController(ApplicationContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetIngredientDto>>> GetIngredients()
    {
        var ingredients = await _context.Ingredients.Include(i => i.Pancakes).ToListAsync();

        var ingredientDTOs = ingredients.Select(i => new GetIngredientDto()
        {
            Id = i.Id,
            Name = i.Name,
            Price = i.Price,
            IsHealthy = i.IsHealthy,
            IngredientType = i.IngredientType,
            Pancakes = i.Pancakes.Select(p => new PancakeForIngredientDto()
            {
                Id = p.Id,
                Price = p.Price
            }).ToList()
        }).ToList();

        return ingredientDTOs;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Ingredient>> GetIngredient(int id)
    {
        var ingredient = await _context.Ingredients.FindAsync(id);
    
        if (ingredient == null)
        {
            return NotFound();
        }
    
        return ingredient;
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutIngredient(int id, CreateIngredientDto ingredientDto)
    {
        var ingredient = await _context.Ingredients.FindAsync(id);

        if (ingredient == null)
        {
            return NotFound();
        }

        ingredient.Name = ingredientDto.Name;
        ingredient.Price = ingredientDto.Price;
        ingredient.IsHealthy = ingredientDto.IsHealthy;
        ingredient.IngredientType = ingredientDto.IngredientType;

        _context.Entry(ingredient).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!IngredientExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<CreateIngredientDto>> PostIngredient(CreateIngredientDto ingredientDto)
    {
        var ingredient = new Ingredient()
        {
            Name = ingredientDto.Name,
            Price = ingredientDto.Price,
            IsHealthy = ingredientDto.IsHealthy,
            IngredientType = ingredientDto.IngredientType,
            Pancakes = new List<Pancake>()
        };
        
        _context.Ingredients.Add(ingredient);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetIngredient", new { id = ingredient.Id }, ingredient);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIngredient(int id)
    {
        var ingredient = await _context.Ingredients.FindAsync(id);
        if (ingredient == null)
        {
            return NotFound();
        }

        _context.Ingredients.Remove(ingredient);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool IngredientExists(int id)
    {
        return _context.Ingredients.Any(e => e.Id == id);
    }
}

