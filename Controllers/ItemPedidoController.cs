using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteApi.Models;

namespace RestauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemPedidoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ItemPedidoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemPedido>>> GetItensPedido()
        {
            return await _context.ItensPedido.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemPedido>> GetItemPedido(int id)
        {
            var item = await _context.ItensPedido.FindAsync(id);
            if (item == null)
                return NotFound();

            return item;
        }

        [HttpPost]
        public async Task<ActionResult<ItemPedido>> PostItemPedido(ItemPedido item)
        {
            _context.ItensPedido.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItemPedido), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemPedido(int id, ItemPedido item)
        {
            if (id != item.Id)
                return BadRequest();

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ItensPedido.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemPedido(int id)
        {
            var item = await _context.ItensPedido.FindAsync(id);
            if (item == null)
                return NotFound();

            _context.ItensPedido.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}