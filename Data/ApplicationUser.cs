using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LostAndFoundWeb.Data;
// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [Required]
    [MaxLength(10)]
    public string YearGroup { get; set; }
}

