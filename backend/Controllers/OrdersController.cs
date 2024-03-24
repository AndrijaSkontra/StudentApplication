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

        // GET: api/Orders
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
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
            
            _context.Order.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.Id }, orderDto);
        }

        // DELETE: api/Orders/5
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
