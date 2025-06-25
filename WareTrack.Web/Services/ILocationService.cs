using System.Linq.Expressions;
using WareTrack.Web.Dtos;
using WareTrack.Web.Models;
using WareTrack.Web.Services.Base;

namespace WareTrack.Web.Services;

public interface ILocationService : IBaseService<LocationDto, Location>
{
    Task<List<LocationDto>> GetLocationsByShelf(int shelfId);
    Task<List<LocationDto>> GetLocationsWithItems();
    Task<List<LocationDto>> GetEmptyLocations();
    Task<List<LocationDto>> GetOccupiedLocations();
}