using WareTrack.Web.Dtos;

namespace WareTrack.Web.Services;

public interface ITransferService
{
    Task<List<TransferDto>> GetTransfers(int page = 1, int pageSize = 10, string searchString = "");
    Task<int> GetTransfersCount(string searchString = "");
    Task<bool> TransferItem(int inventoryItemId, int targetLocationId, int quantity);
}