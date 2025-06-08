using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauranteApi.Models;

namespace RestauranteApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvaliacaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AvaliacaoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Avaliacao>>> GetAvaliacoes()
        {
            return await _context.Avaliacoes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Avaliacao>> GetAvaliacao(int id)
        {
            var avaliacao = await _context.Avaliacoes.FindAsync(id);
            if (avaliacao == null)
                return NotFound();

            return avaliacao;
        }

        [HttpPost]
        public async Task<ActionResult<Avaliacao>> PostAvaliacao(Avaliacao avaliacao)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Avaliacao)
                .FirstOrDefaultAsync(p => p.Id == avaliacao.PedidoId);

            if (pedido == null)
            {
                return NotFound("Pedido não encontrado, não é possível avaliá-lo.");
            }

            if (pedido.Avaliacao != null)
            {
                return Conflict("Este pedido já foi avaliado.");
            }

            pedido.Avaliacao = avaliacao;

            _context.Avaliacoes.Add(avaliacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAvaliacao), new { id = avaliacao.Id }, avaliacao);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAvaliacao(int id, Avaliacao avaliacao)
        {
            if (id != avaliacao.Id)
                return BadRequest();

            _context.Entry(avaliacao).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Avaliacoes.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAvaliacao(int id)
        {
            var avaliacao = await _context.Avaliacoes.FindAsync(id);
            if (avaliacao == null)
                return NotFound();

            _context.Avaliacoes.Remove(avaliacao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}