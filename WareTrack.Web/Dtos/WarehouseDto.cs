namespace WareTrack.Web.Dtos;

public class WarehouseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    
    public List<ShelfDto>? Shelves { get; set; }
}