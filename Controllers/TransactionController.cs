using FinTrackAPI.DTOs;
using FinTrackAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinTrackAPI.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    [Authorize] // Requer autenticação para acessar as rotas de transações
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _transactionService;

        public TransactionController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet] // GET /api/transactions?startDate=2026-01-01&endDate=2026-01-31
        public async Task<IActionResult> GetAll([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var transactions = await _transactionService.GetTransactionsByUserIdAsync(userId, startDate, endDate);
            return Ok(transactions);
        }

        [HttpGet("{id}")] // GET /api/transactions/{id}
        public async Task<IActionResult> GetById(int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var transaction = await _transactionService.GetTransactionByIdAsync(id, userId);

            if (transaction == null) return NotFound(new { message = "Transação não encontrada." });
            return Ok(transaction);
        }

        [HttpPost] // POST /api/transactions
        public async Task<IActionResult> Create([FromBody] TransactionDTO dto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var transaction = await _transactionService.CreateTransactionAsync(dto, userId);

            if (transaction == null) return BadRequest(new { message = "Categoria inválida para este usuário." });
            return StatusCode(201, transaction);
        }

        [HttpPut("{id}")] // PUT /api/transactions/{id}
        public async Task<IActionResult> Update(int id, [FromBody] TransactionDTO dto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var success = await _transactionService.UpdateTransactionAsync(id, dto, userId);

            if (!success) return BadRequest(new { message = "Não foi possível atualizar a transação. Verifique os dados ou a categoria." });
            return Ok(new { message = "Transação atualizada com sucesso!" });
        }

        [HttpDelete("{id}")] // DELETE /api/transactions/{id}
        public async Task<IActionResult> Delete(int id)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var success = await _transactionService.DeleteTransactionAsync(id, userId);

            if (!success) return NotFound(new { message = "Transação não encontrada." });
            return Ok(new { message = "Transação deletada com sucesso!" });
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var summary = await _transactionService.GetSummaryAsync(userId, startDate, endDate);
            return Ok(summary);
        }
    }
}
