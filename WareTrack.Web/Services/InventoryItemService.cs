using System.Linq.Expressions;
using AutoMapper;
using WareTrack.Web.Data;
using WareTrack.Web.Dtos;
using WareTrack.Web.Models;
using WareTrack.Web.Services.Base;

namespace WareTrack.Web.Services;

public class InventoryItemService : BaseService<InventoryItemDto, InventoryItem>, IInventoryItemService
{
    public InventoryItemService(DataContext dataContext, IMapper mapper)
        : base(dataContext, mapper)
    {
    }

    protected override Expression<Func<InventoryItem, bool>> GetIdPredicate(int id)
    {
        return item => item.Id == id;
    }

    public async Task<List<InventoryItemDto>> GetNoZeroQuantitiesInventoryItems()
    {
        return await GetWhereWithIncludes(i => i.Quantity > 0, i => i.Location);
    }

    public async Task<List<InventoryItemDto>> GetInventoryItemsWithLocation()
    {
        return await GetAllWithIncludes(i => i.Location);
    }

    public async Task<InventoryItemDto?> GetInventoryItemWithFullHierarchy(int inventoryItemId)
    {
        return await GetByIdWithIncludes(
            inventoryItemId,
            i => i.Location,
            i => i.Location.Shelf,
            i => i.Location.Shelf.Warehouse
        );
    }
}