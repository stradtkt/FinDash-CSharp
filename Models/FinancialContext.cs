using Microsoft.EntityFrameworkCore;

namespace FinDash.Models
{
    public class FinancialContext : DbContext
    {
        
        public FinancialContext(DbContextOptions options) : base(options) {}
        
        public DbSet<User> Users { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Profit> Profits { get; set; }
    }
}