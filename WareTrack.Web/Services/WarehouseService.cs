using AutoMapper;
using System.Linq.Expressions;
using WareTrack.Web.Data;
using WareTrack.Web.Dtos;
using WareTrack.Web.Models;
using WareTrack.Web.Services.Base;

namespace WareTrack.Web.Services;

public class WarehouseService : BaseService<WarehouseDto, Warehouse>, IWarehouseService
{
    public WarehouseService(DataContext dataContext, IMapper mapper)
        : base(dataContext, mapper)
    {
    }

    protected override Expression<Func<Warehouse, bool>> GetIdPredicate(int id)
    {
        return warehouse => warehouse.Id == id;
    }

    public async Task<List<WarehouseDto>> GetWarehousesWithShelves()
    {
        return await GetAllWithIncludes(w => w.Shelves);
    }
}