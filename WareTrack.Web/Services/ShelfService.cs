using System.Linq.Expressions;
using AutoMapper;
using WareTrack.Web.Data;
using WareTrack.Web.Dtos;
using WareTrack.Web.Models;
using WareTrack.Web.Services.Base;

namespace WareTrack.Web.Services;

public class ShelfService : BaseService<ShelfDto, Shelf>, IShelfService
{
    public ShelfService(DataContext dataContext, IMapper mapper)
        : base(dataContext, mapper)
    {
    }

    protected override Expression<Func<Shelf, bool>> GetIdPredicate(int id)
    {
        return shelf => shelf.Id == id;
    }

    public async Task<List<ShelfDto>> GetShelvesByWarehouse(int warehouseId)
    {
        return await GetWhere(s => s.WarehouseId == warehouseId);
    }

    public async Task<List<ShelfDto>> GetShelfWithLocations()
    {
        return await GetAllWithIncludes(s => s.Locations);
    }

    public async Task<ShelfDto?> GetShelfWithFullHierarchy(int shelfId)
    {
        return await GetByIdWithIncludes(
            shelfId, 
            s => s.Warehouse, 
            s => s.Locations.Select(l => l.Item)
        );
    }
}