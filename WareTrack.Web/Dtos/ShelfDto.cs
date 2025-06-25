namespace WareTrack.Web.Dtos;

public class ShelfDto
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string? Description { get; set; }

    public WarehouseDto Warehouse { get; set; } = null!;
    public List<LocationDto>? Locations { get; set; }
}