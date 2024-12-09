using System.ComponentModel.DataAnnotations;

namespace Auth.Models;

public class UserDto
{
    [Required]
    [StringLength(254, MinimumLength = 6)]
    public string Mail { get; set; }

    [Required]
    [StringLength(64, MinimumLength = 8)]
    public string Password { get; set; }
}