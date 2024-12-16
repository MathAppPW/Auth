using System.ComponentModel.DataAnnotations;

namespace Auth.Models;

public class User
{
    [Required] 
    [StringLength(32, MinimumLength = 32)]//model do bazy danych
    public string Id { get; set; }
    
    [Required]
    [StringLength(254, MinimumLength = 6)]
    public string Mail { get; set; }
    
    [Required]
    [StringLength(88, MinimumLength = 88)]
    public string PasswordHash { get; set; }
}