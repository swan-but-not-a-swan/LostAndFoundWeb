using System.ComponentModel.DataAnnotations;

namespace LostAndFoundWeb.Data;

public class FoundItem:Item
{
    public ApplicationUser Finder { get; set; }
    [Required]
    public bool OwnerFound { get; set; }
}
