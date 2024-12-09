using System.ComponentModel.DataAnnotations;

namespace Auth.Models;

public class User
{
    [Required] 
    [StringLength(32, MinimumLength = 32)]
    public string Id { get; set; }
    
    [Required]
    [StringLength(254, MinimumLength = 6)]
    public string Mail { get; set; }
    
    [Required]
    [StringLength(60, MinimumLength = 60)]
    public string PasswordHash { get; set; }
}