using WareTrack.Web.Dtos;
using WareTrack.Web.Models;
using WareTrack.Web.Services.Base;

namespace WareTrack.Web.Services;

public interface IWarehouseService : IBaseService<WarehouseDto, Warehouse>
{
    Task<List<WarehouseDto>> GetWarehousesWithShelves();
}