using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteApi.Models;

namespace RestauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestauranteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RestauranteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}/pedidos-melhor-avaliados")]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidosMelhorAvaliados(int id, int? top = null)
        {
            var pedidosDoRestaurante = await _context.Pedidos
                .Where(p => p.RestauranteId == id) 
                .Include(p => p.Itens)             
                .Include(p => p.Avaliacao)       
                .OrderByDescending(p => p.Avaliacao != null ? p.Avaliacao.Nota : 0) 
                .ThenBy(p => p.Avaliacao != null ? p.Avaliacao.Comentario : "")    
                .ToListAsync();

            if (!pedidosDoRestaurante.Any())
                return NotFound("Nenhum pedido encontrado para este restaurante.");

            var pedidosAvaliados = pedidosDoRestaurante
                .Where(p => p.Avaliacao != null)
                .ToList();

            if (top.HasValue && top.Value > 0)
            {
                pedidosAvaliados = pedidosAvaliados.Take(top.Value).ToList();
            }

            return Ok(pedidosAvaliados);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Restaurante>>> GetRestaurantes()
        {
            return await _context.Restaurantes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Restaurante>> GetRestaurante(int id)
        {
            var restaurante = await _context.Restaurantes.FindAsync(id);
            if (restaurante == null)
                return NotFound();

            return restaurante;
        }

        [HttpPost]
        public async Task<ActionResult<Restaurante>> PostRestaurante(Restaurante restaurante)
        {
            _context.Restaurantes.Add(restaurante);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRestaurante), new { id = restaurante.Id }, restaurante);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRestaurante(int id, Restaurante restaurante)
        {
            if (id != restaurante.Id)
                return BadRequest();

            _context.Entry(restaurante).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Restaurantes.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurante(int id)
        {
            var restaurante = await _context.Restaurantes.FindAsync(id);
            if (restaurante == null)
                return NotFound();

            _context.Restaurantes.Remove(restaurante);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}