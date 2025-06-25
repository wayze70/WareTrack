namespace WareTrack.Web.Dtos;

public class InventoryItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int Quantity { get; set; }
    
    public LocationDto Location { get; set; }
}