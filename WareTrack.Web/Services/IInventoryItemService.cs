using WareTrack.Web.Dtos;
using WareTrack.Web.Models;
using WareTrack.Web.Services.Base;

namespace WareTrack.Web.Services;

public interface IInventoryItemService : IBaseService<InventoryItemDto, InventoryItem>
{
    Task<List<InventoryItemDto>> GetNoZeroQuantitiesInventoryItems();
    Task<List<InventoryItemDto>> GetInventoryItemsWithLocation();
    Task<InventoryItemDto?> GetInventoryItemWithFullHierarchy(int inventoryItemId);
}