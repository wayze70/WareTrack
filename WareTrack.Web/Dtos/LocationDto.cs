namespace WareTrack.Web.Dtos;

public class LocationDto
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string? Description { get; set; }
    
    public ShelfDto Shelf { get; set; } = null!;
    public InventoryItemDto? Item { get; set; }
}