using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Repository;
using backend.ViewModel;
using backend.ViewModel.PancakeDTO;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PancakesController : ControllerBase
{
    private readonly ApplicationContext _context;

    public PancakesController(ApplicationContext context)
    {
        _context = context;
    }

    // GET: api/Pancakes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetPancakeDto>>> GetPancake()
    {
        var pancakes = await _context.Pancake.Include(pa => pa.Ingredients).ToListAsync();
        
        var pancakeDTOs = pancakes.Select(p => new GetPancakeDto()
        {
            Id = p.Id,
            Price = p.Price,
            Ingredients = p.Ingredients.Select(i => new IngredientForPancakeDto()
            {
                Id = i.Id,
                Name = i.Name,
                IsHealthy = i.IsHealthy,
                IngredientType = i.IngredientType,
                Price = i.Price
            }).ToList()
        }).ToList();

        return pancakeDTOs;
    }

    // GET: api/Pancakes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Pancake>> GetPancake(int id)
    {
        var pancake = await _context.Pancake.FindAsync(id);

        if (pancake == null)
        {
            return NotFound();
        }

        return pancake;
    }

    // PUT: api/Pancakes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPancake(int id, Pancake pancake)
    {
        if (id != pancake.Id)
        {
            return BadRequest();
        }

        _context.Entry(pancake).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PancakeExists(id))
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

    // POST: api/Pancakes
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<GetPancakeDto>> PostPancake(int id)
    {
        var ingredient = await _context.Ingredients.FindAsync(id);
        if (ingredient == null)
        {
            return NotFound("Ingredient not found");
        }

        if (ingredient.IngredientType != IngredientType.Base)
        {
            return BadRequest("Ingredient is not a base ingredient");
        }

        var pancake = new Pancake { Ingredients = new List<Ingredient>() };
        
        pancake.Ingredients.Add(ingredient);
        pancake.Price = pancake.Ingredients.Sum(i => i.Price);
        
        _context.Pancake.Add(pancake);
        await _context.SaveChangesAsync();

        var pancakeDto = new GetPancakeDto()
        {
            Id = pancake.Id,
            Ingredients = pancake.Ingredients.Select(i => new IngredientForPancakeDto()
            {
                Id = i.Id,
                IngredientType = i.IngredientType,
                IsHealthy = i.IsHealthy,
                Name = i.Name,
                Price = i.Price
                
            }).ToList(),
            Price = pancake.Price
        };

        return CreatedAtAction("GetPancake", new { id = pancakeDto.Id }, pancakeDto);
    }
    
    [HttpPut]
    [Route("Ingredient")]
    public async Task<ActionResult<GetPancakeDto>> AddIngredient(int pancakeId, int ingredientId)
    {
        var pancake = await _context.Pancake.Include(p => p.Ingredients).FirstOrDefaultAsync(p => p.Id == pancakeId);
        if (pancake == null)
        {
            return NotFound("Pancake not found");
        }

        var ingredient = await _context.Ingredients.FindAsync(ingredientId);
        if (ingredient == null)
        {
            return NotFound("Ingredient not found");
        }

        pancake.Ingredients.Add(ingredient);
        pancake.Price = pancake.Ingredients.Sum(i => i.Price);

        await _context.SaveChangesAsync();

        var pancakeDto = new GetPancakeDto()
        {
            Id = pancake.Id,
            Ingredients = pancake.Ingredients.Select(i => new IngredientForPancakeDto()
            {
                Id = i.Id,
                IngredientType = i.IngredientType,
                IsHealthy = i.IsHealthy,
                Name = i.Name,
                Price = i.Price
            }).ToList(),
            Price = pancake.Price
        };

        return Ok(pancakeDto);
    }

    [HttpPost]
    [Route("CreatePancake")]
    public async Task<ActionResult<GetPancakeDto>> CreatePancake(List<int> nums)
    {
        var pancake = new Pancake()
        {
            Ingredients = await _context.Ingredients.Where(i => nums.Contains(i.Id)).ToListAsync()
        };
        
        if (pancake.Ingredients.Count < 1)
        {
            return BadRequest("You must select at least one ingredient!");
        }

        var baseIngredients = pancake.Ingredients.Count(i => i.IngredientType == IngredientType.Base);
        if (baseIngredients != 1)
        {
            return BadRequest("You need to have exactly one base ingredient!");
        }
        
        var stuffingIngredients = pancake.Ingredients.Count(i => i.IngredientType == IngredientType.Stuffing);
        if (stuffingIngredients < 1)
        {
            return BadRequest("You need to have at least one stuffing ingredient!");
        }
        
        pancake.Price = pancake.Ingredients.Sum(i => i.Price);
        
        _context.Pancake.Add(pancake);
        await _context.SaveChangesAsync();
        
        var pancakeDto = new GetPancakeDto()
        {
            Id = pancake.Id,
            Ingredients = pancake.Ingredients.Select(i => new IngredientForPancakeDto()
            {
                Id = i.Id,
                IngredientType = i.IngredientType,
                IsHealthy = i.IsHealthy,
                Name = i.Name,
                Price = i.Price
            }).ToList(),
            Price = pancake.Price
        };
        
        return pancakeDto;
    }

    // DELETE: api/Pancakes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePancake(int id)
    {
        var pancake = await _context.Pancake.FindAsync(id);
        if (pancake == null)
        {
            return NotFound();
        }

        _context.Pancake.Remove(pancake);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PancakeExists(int id)
    {
        return _context.Pancake.Any(e => e.Id == id);
    }
}

