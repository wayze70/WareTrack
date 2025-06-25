using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareTrack.Web.Models;

[Table("Shelves")]
public class Shelf
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
    [Column("warehouse_id")]
    public int WarehouseId { get; set; }

    [ForeignKey("WarehouseId")]
    public Warehouse Warehouse { get; set; } = null!;

    [InverseProperty("Shelf")]
    public ICollection<Location> Locations { get; set; } = new List<Location>();
}