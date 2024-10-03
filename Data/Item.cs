using System.ComponentModel.DataAnnotations;

namespace LostAndFoundWeb.Data;

public abstract class Item
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    [MaxLength(400)]
    public string Description { get; set; }
    [Required]
    [MaxLength(20)]
    public string Category { get; set; }
    [Required]
    [MaxLength(100)]
    public string Location { get; set; }
}
