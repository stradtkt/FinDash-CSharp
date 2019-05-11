using System;
using System.ComponentModel.DataAnnotations;

namespace FinDash.Models
{
    public class Profit : BaseEntity
    {
        [Key] public int ProfitId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }

        public Profit()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}