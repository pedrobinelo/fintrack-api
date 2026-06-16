using FinTrack_API.Models;

namespace FinTrack_API.DTOs
{
    public class TransactionDTO
    {
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; } 
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
    }
}
