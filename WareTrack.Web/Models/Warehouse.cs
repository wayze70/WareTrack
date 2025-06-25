using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareTrack.Web.Models;

[Table("Warehouses")]
public class Warehouse
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
    
    
    public ICollection<Shelf> Shelves { get; set; } = new List<Shelf>();
}