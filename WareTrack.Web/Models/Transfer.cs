using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WareTrack.Web.Models;

[Table("Transfers")]
public class Transfer
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Column("item_id")]
    public int ItemId { get; set; }

    [ForeignKey("ItemId")]
    public InventoryItem Item { get; set; } = null!;

    [Required]
    [Column("source_location_id")]
    public int SourceLocationId { get; set; }

    [ForeignKey("SourceLocationId")]
    public Location SourceLocation { get; set; } = null!;

    [Required]
    [Column("target_location_id")]
    public int TargetLocationId { get; set; }

    [ForeignKey("TargetLocationId")]
    public Location TargetLocation { get; set; } = null!;

    [Required]
    [Column("quantity")]
    public int Quantity { get; set; }

    [Required]
    [Column("moved_at")]
    public DateTime MovedAt { get; set; } = DateTime.UtcNow;
}