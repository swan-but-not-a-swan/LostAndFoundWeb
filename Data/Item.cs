using System.ComponentModel.DataAnnotations;

namespace LostAndFoundWeb.Data;

public abstract class Item
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    [MaxLength(20)]
    public string WordOne { get; set; }
    [Required]
    [MaxLength(20)]
    public string WordTwo { get; set; }
    [Required]
    [MaxLength(20)]
    public string WordThree { get; set; }
    public List<string> Urls { get; set; } = new();
    [Required]
    [MaxLength(100)]
    public string Location { get; set; }
}
