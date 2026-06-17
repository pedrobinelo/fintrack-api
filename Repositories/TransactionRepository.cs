using FinTrackAPI.DTOs;
using FinTrackAPI.Data;
using FinTrackAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FinTrackAPI.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly AppDbContext _context;
        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        private IQueryable<Transaction> ApplyDateFilter(IQueryable<Transaction> query, DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue)
            {
                // Força o horário para o primeiro milissegundo do dia (00:00:00)
                var start = startDate.Value.Date;
                query = query.Where(t => t.Date.Date >= start);
            }

            if (endDate.HasValue)
            {
                // Força o horário para o último milissegundo do dia (23:59:59.999)
                var end = endDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(t => t.Date.Date <= end);
            }

            return query;
        } 

        public async Task<List<Transaction>> GetByUserIdAsync(int userId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Transactions
                .Include(t => t.Category) // Traz junto os dados da categoria associada
                .Where(t => t.UserId == userId);

            // Aplica os filtros de data, se fornecidos
            query = ApplyDateFilter(query, startDate, endDate);

            return await query.OrderByDescending(t => t.Date).ToListAsync();
        }

        public async Task<Transaction?> GetByIdAsync(int id, int userId)
        {
            return await _context.Transactions
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        }

        public async Task<Transaction> CreateAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            _context.Entry(transaction).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Transaction transaction)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<TransactionSummaryDTO> GetSummaryAsync(int userId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Transactions
                .Where(t => t.UserId == userId);

            query = ApplyDateFilter(query, startDate, endDate);

            var totalIncome = await query
                .Where(t => t.Type == TransactionType.Income)
                .SumAsync(t => t.Amount);

            var totalExpense = await query
                .Where(t => t.Type == TransactionType.Expense)
                .SumAsync(t => t.Amount);

            return new TransactionSummaryDTO
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                Balance = totalIncome - totalExpense,
                StartDate = startDate,
                EndDate = endDate
            };
        }
    }
}
