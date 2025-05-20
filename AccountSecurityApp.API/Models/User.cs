using System.ComponentModel.DataAnnotations;

namespace AccountSecurityApp.API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [StringLength(70)]
        public string Username { get; set; } = ""; 
        public string PasswordHash { get; set; } = ""; 
    }
}
