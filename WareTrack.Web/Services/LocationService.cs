using System.Linq.Expressions;
using AutoMapper;
using WareTrack.Web.Data;
using WareTrack.Web.Dtos;
using WareTrack.Web.Models;
using WareTrack.Web.Services.Base;

namespace WareTrack.Web.Services;

public class LocationService : BaseService<LocationDto, Location>, ILocationService
{
    public LocationService(DataContext dataContext, IMapper mapper)
        : base(dataContext, mapper)
    {
    }

    protected override Expression<Func<Location, bool>> GetIdPredicate(int id)
    {
        return location => location.Id == id;
    }

    public async Task<List<LocationDto>> GetLocationsByShelf(int shelfId)
    {
        return await GetWhere(l => l.ShelfId == shelfId);
    }

    public async Task<List<LocationDto>> GetLocationsWithItems()
    {
        return await GetAllWithIncludes(l => l.Item);
    }

    public async Task<List<LocationDto>> GetEmptyLocations()
    {
        return await GetWhereWithIncludes(l => l.Item == null, l => l.Shelf);
    }

    public async Task<List<LocationDto>> GetOccupiedLocations()
    {
        return await GetWhereWithIncludes(l => l.Item != null, l => l.Item, l => l.Shelf);
    }
}