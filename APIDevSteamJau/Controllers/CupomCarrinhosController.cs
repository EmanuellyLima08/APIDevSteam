using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIDevSteamJau.Data;
using APIDevSteamJau.Models;

namespace APIDevSteamJau.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CupomCarrinhosController : ControllerBase
    {
        private readonly APIContext _context;

        public CupomCarrinhosController(APIContext context)
        {
            _context = context;
        }

        // GET: api/CupomCarrinhos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CupomCarrinho>>> GetCuponsCarrinhos()
        {
            return await _context.CuponsCarrinhos.ToListAsync();
        }

        // GET: api/CupomCarrinhos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CupomCarrinho>> GetCupomCarrinho(Guid id)
        {
            var cupomCarrinho = await _context.CuponsCarrinhos.FindAsync(id);

            if (cupomCarrinho == null)
            {
                return NotFound();
            }

            return cupomCarrinho;
        }

        // PUT: api/CupomCarrinhos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCupomCarrinho(Guid id, CupomCarrinho cupomCarrinho)
        {
            if (id != cupomCarrinho.CupomCarrinhoId)
            {
                return BadRequest();
            }

            _context.Entry(cupomCarrinho).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CupomCarrinhoExists(id))
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

        // POST: api/CupomCarrinhos
        [HttpPost]
        public async Task<ActionResult<CupomCarrinho>> PostCupomCarrinho(CupomCarrinho cupomCarrinho)
        {
            _context.CuponsCarrinhos.Add(cupomCarrinho);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCupomCarrinho", new { id = cupomCarrinho.CupomCarrinhoId }, cupomCarrinho);
        }

        // DELETE: api/CupomCarrinhos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCupomCarrinho(Guid id)
        {
            var cupomCarrinho = await _context.CuponsCarrinhos.FindAsync(id);
            if (cupomCarrinho == null)
            {
                return NotFound();
            }

            _context.CuponsCarrinhos.Remove(cupomCarrinho);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/CupomCarrinhos/AplicarCupom
        [HttpPost("AplicarCupom")]
        public async Task<IActionResult> AplicarCupom(Guid carrinhoId, Guid cupomId)
        {
            // Verificar se o carrinho existe
            var carrinho = await _context.Carrinhos
                .Include(c => c.Itens)
                .FirstOrDefaultAsync(c => c.CarrinhoId == carrinhoId);

            if (carrinho == null)
            {
                return NotFound("Carrinho não encontrado.");
            }

            // Verificar se o cupom existe
            var cupom = await _context.CuponsCarrinhos.FirstOrDefaultAsync(c => c.CupomCarrinhoId == cupomId);
            if (cupom == null)
            {
                return NotFound("Cupom não encontrado.");
            }

            // Verificar validade do cupom
            if (cupom.DataValidade < DateTime.Now || cupom.Ativo != true)
            {
                return BadRequest("Cupom expirado ou inativo.");
            }

            // Calcular o valor total do carrinho
            var total = carrinho.Itens.Sum(i => i.ValorUnitario * i.Quantidade);

            // Calcular o valor do desconto
            var desconto = (cupom.Desconto / 100) * total;
            var totalComDesconto = total - desconto;

            // Atualizar o carrinho com o valor com desconto
            carrinho.ValorTotal = totalComDesconto;

            // Decrementar o limite de uso do cupom
            cupom.LimiteUso = (cupom.LimiteUso ?? 1) - 1;
            if (cupom.LimiteUso <= 0)
            {
                cupom.Ativo = false; // Desativar o cupom após atingir o limite
            }

            // Salvar as alterações no banco de dados
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Mensagem = "Cupom aplicado com sucesso!",
                TotalOriginal = total,
                Desconto = desconto,
                TotalComDesconto = totalComDesconto
            });
        }

        // Verifica se o cupom existe no banco
        private bool CupomCarrinhoExists(Guid id)
        {
            return _context.CuponsCarrinhos.Any(e => e.CupomCarrinhoId == id);
        }
    }
}
