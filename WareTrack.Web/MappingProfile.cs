using AutoMapper;
using WareTrack.Web.Dtos;
using WareTrack.Web.Models;

namespace WareTrack.Web;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Warehouse, WarehouseDto>();
        CreateMap<WarehouseDto, Warehouse>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Shelves, opt => opt.Ignore());

        CreateMap<Shelf, ShelfDto>();
        CreateMap<ShelfDto, Shelf>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Warehouse, opt => opt.Ignore())
            .ForMember(dest => dest.Locations, opt => opt.Ignore());

        CreateMap<Location, LocationDto>();
        CreateMap<LocationDto, Location>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Shelf, opt => opt.Ignore())
            .ForMember(dest => dest.Item, opt => opt.Ignore());

        CreateMap<InventoryItem, InventoryItemDto>();
        CreateMap<InventoryItemDto, InventoryItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Location, opt => opt.Ignore());
        
        CreateMap<Transfer, TransferDto>();
        CreateMap<TransferDto, Transfer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.Item.Id))
            .ForMember(dest => dest.SourceLocationId, opt => opt.MapFrom(src => src.SourceLocation.Id))
            .ForMember(dest => dest.TargetLocationId, opt => opt.MapFrom(src => src.TargetLocation.Id));
    
    }
}