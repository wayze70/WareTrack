using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareTrack.Web.Models;

[Table("InventoryItems")]
public class InventoryItem
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("name")]
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [Column("description")]
    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    [Column("quantity")]
    public int Quantity { get; set; }

    [Required]
    [Column("location_id")]
    public int? LocationId { get; set; }

    [ForeignKey("LocationId")]
    public Location Location { get; set; } = null!;
}