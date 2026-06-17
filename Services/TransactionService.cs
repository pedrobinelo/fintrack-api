using FinTrackAPI.DTOs;
using FinTrackAPI.Models;
using FinTrackAPI.Repositories;
using FinTrackAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace FinTrackAPI.Services
{
    public class TransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly AppDbContext _context; // Para validar a categoria

        public TransactionService(ITransactionRepository transactionRepository, AppDbContext context)
        {
            _transactionRepository = transactionRepository;
            _context = context;
        }

        public async Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId, DateTime? startDate, DateTime? endDate)
        {
            return await _transactionRepository.GetByUserIdAsync(userId, startDate, endDate);
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int id, int userId)
        {
            return await _transactionRepository.GetByIdAsync(id, userId);
        }

        public async Task<Transaction> CreateTransactionAsync(TransactionDTO dto, int userId)
        {
            // Validação de segurança: a categoria pertence ao usuário?
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId && c.UserId == userId);
            if (!categoryExists) return null;

            var newTransaction = new Transaction
            {
                Description = dto.Description,
                Amount = dto.Amount,
                Type = dto.Type,
                Date = dto.Date,
                CategoryId = dto.CategoryId,
                UserId = userId
            };
            return await _transactionRepository.CreateAsync(newTransaction);
        }

        public async Task<bool> UpdateTransactionAsync(int id, TransactionDTO dto, int userId)
        {
            var existingTransaction = await _transactionRepository.GetByIdAsync(id, userId);
            if (existingTransaction == null) return false;

            // Validação de segurança: a categoria pertence ao usuário?
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId && c.UserId == userId);
            if (!categoryExists) return false;

            existingTransaction.Description = dto.Description;
            existingTransaction.Amount = dto.Amount;
            existingTransaction.Type = dto.Type;
            existingTransaction.Date = dto.Date;
            existingTransaction.CategoryId = dto.CategoryId;
            await _transactionRepository.UpdateAsync(existingTransaction);
            return true;
        }

        public async Task<bool> DeleteTransactionAsync(int id, int userId)
        {
            var existingTransaction = await _transactionRepository.GetByIdAsync(id, userId);
            if (existingTransaction == null) return false;
            await _transactionRepository.DeleteAsync(existingTransaction);
            return true;
        }
        public async Task<TransactionSummaryDTO> GetSummaryAsync(int userId, DateTime? startDate, DateTime? endDate)
        {
            return await _transactionRepository.GetSummaryAsync(userId, startDate, endDate);
        }
    }
}
