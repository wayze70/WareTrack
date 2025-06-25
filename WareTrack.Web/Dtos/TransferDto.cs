namespace WareTrack.Web.Dtos
{
    public class TransferDto
    {
        public int Id { get; set; }
        public DateTime MovedAt { get; set; }
        public InventoryItemDto Item { get; set; }
        public LocationDto SourceLocation { get; set; }
        public LocationDto TargetLocation { get; set; }
        public int Quantity { get; set; }
    }
}