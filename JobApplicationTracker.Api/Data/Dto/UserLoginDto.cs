using System.ComponentModel.DataAnnotations;

public class UserLoginDto
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
    public string Key { get; set; }
}