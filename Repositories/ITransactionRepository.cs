using FinTrack_API.DTOs;
using FinTrack_API.Models;

namespace FinTrack_API.Repositories
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetByUserIdAsync(int userId, DateTime? startDate, DateTime? endDate);
        Task<Transaction?> GetByIdAsync(int id, int userId);
        Task<Transaction> CreateAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(Transaction transaction);
        Task<TransactionSummaryDTO> GetSummaryAsync(int userId, DateTime? startDate, DateTime? endDate);
    }

}
