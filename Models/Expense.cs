using System;
using System.ComponentModel.DataAnnotations;

namespace FinDash.Models
{
    public class Expense : BaseEntity
    {
        [Key] public int ExpenseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public Expense()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}