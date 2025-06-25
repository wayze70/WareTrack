using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareTrack.Web.Models;

[Table("Locations")]
public class Location
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("code")]
    [MaxLength(10)]
    public string Code { get; set; } = null!;

    [Column("description")]
    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    [Column("shelf_id")]
    public int ShelfId { get; set; }

    [ForeignKey("ShelfId")]
    public Shelf Shelf { get; set; } = null!;

    [InverseProperty("Location")]
    public InventoryItem? Item { get; set; }
}