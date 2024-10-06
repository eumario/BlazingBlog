using System.ComponentModel.DataAnnotations;

namespace BlazingBlog.Models;

public class RegisterUser
{
    [Required]
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    [Required, EmailAddress, DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    [Required, MinLength(8), DataType(DataType.Password)]
    public string? Password { get; set; }
    [Required, MinLength(8), DataType(DataType.Password)]
    [Compare("Password")]
    public string? Confirm { get; set; }
    
}