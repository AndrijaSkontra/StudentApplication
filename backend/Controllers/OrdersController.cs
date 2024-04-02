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
using backend.ViewModel.OrderDTO;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public OrdersController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetOrderDto>>> GetOrder()
        {
            var orders = await _context.Order.Include(o => o.Pancakes).ToListAsync();

            var orderDTOs = orders.Select(o => new GetOrderDto()
            {
                Id = o.Id,
                Desc = o.Desc,
                Price = o.Price,
                Time = o.Time,
                Pancakes = o.Pancakes.Select(p => new PancakeForIngredientDto()
                {
                    Id = p.Id,
                    Price = p.Price
                }).ToList()
            }).ToList();

            return orderDTOs;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetSingleOrderDto>> GetOrder(int id)
        {
            var order = await _context.Order.Include(o => o.Pancakes).FirstAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }
            
            var orderDTO = new GetSingleOrderDto()
            {
                Id = order.Id,
                Desc = order.Desc,
                Price = order.Price,
                Time = order.Time,
                Discount = order.Discount,
                Pancakes = order.Pancakes.Select(p => new PancakeForSingleOrderDto()
                {
                    Id = p.Id,
                    Price = p.Price,
                }).ToList()
            };

            return orderDTO;
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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
        public async Task<ActionResult<Order>> PostOrder(CreateOrderDto orderDto)
        {

            if (orderDto.PancakesId.Count < 1)
            {
                return BadRequest("You must select at least one pancake!");
            }
            
            var order = new Order()
            {
                Desc = orderDto.Desc,
                Time = DateTime.Now,
                Pancakes = await _context.Pancake.Where(p => orderDto.PancakesId.Contains(p.Id)).ToListAsync(),
            };
            order.Price = order.Pancakes.Sum(p => p.Price);
            if (order.Price > 100)
            {
                order.Discount = order.Price * 0.05f;
            }
            else if (order.Price > 200)
            {
                order.Discount = order.Price * 0.1f;
            }
            
            var pancakeDiscount = order.Pancakes.Sum(p => p.Discount);

            if (pancakeDiscount > order.Discount)
            {
                Console.WriteLine($"\nPancake discount {pancakeDiscount} is higher than order discount {order.Discount}\n");
                order.Discount = pancakeDiscount;
                Console.WriteLine($"\nOrder discount is now {order.Discount}\n");
            }
            
            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, orderDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
    }
}
