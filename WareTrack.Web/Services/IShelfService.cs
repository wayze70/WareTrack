using WareTrack.Web.Dtos;
using WareTrack.Web.Models;
using WareTrack.Web.Services.Base;

namespace WareTrack.Web.Services;

public interface IShelfService : IBaseService<ShelfDto, Shelf>
{
    Task<List<ShelfDto>> GetShelvesByWarehouse(int warehouseId);
    Task<List<ShelfDto>> GetShelfWithLocations();
}