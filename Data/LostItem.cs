using System.ComponentModel.DataAnnotations;

namespace LostAndFoundWeb.Data;

public class LostItem:Item
{
    public ApplicationUser LostUser { get; set; }

    public bool HasBeenFound { get; set; }
}
