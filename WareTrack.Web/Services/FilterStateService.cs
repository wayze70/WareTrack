using WareTrack.Web.Pages;

namespace WareTrack.Web.Services
{
    public class FilterStateService
    {
        // Warehouse filters
        public int? SelectedWarehouseId { get; set; }
        public int MinShelfCount { get; set; }
        
        // Shelf filters
        public int? SelectedShelfId { get; set; }
        
        // Location filters
        public int? SelectedLocationId { get; set; }
        public LocationPage.LocationStatusType? LocationStatus { get; set; }
        
        // Item filters
        public int? MinQuantity { get; set; }
        public bool OnlyZeroStock { get; set; }
        
        // Events to notify components of changes
        public event Action? OnChange;
        
        public void NotifyStateChanged() => OnChange?.Invoke();
        
        public void ClearAll()
        {
            SelectedWarehouseId = null;
            SelectedShelfId = null;
            SelectedLocationId = null;
            LocationStatus = null;
            MinQuantity = null;
            OnlyZeroStock = false;
            MinShelfCount = 0;
            
            NotifyStateChanged();
        }
    }
}