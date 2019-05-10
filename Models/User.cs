using System;
using System.ComponentModel.DataAnnotations;

namespace FinDash.Models
{
    public class User : BaseEntity
    {
        [Key] public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public User()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}