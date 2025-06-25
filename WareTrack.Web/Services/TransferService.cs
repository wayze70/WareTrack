using Microsoft.EntityFrameworkCore;
using WareTrack.Web.Data;
using WareTrack.Web.Dtos;
using WareTrack.Web.Models;
using AutoMapper;

namespace WareTrack.Web.Services
{
    public class TransferService : ITransferService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TransferService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TransferDto>> GetTransfers(int page = 1, int pageSize = 10, string searchString = "")
        {
            var query = _context.Transfers.AsNoTracking()
                .Include(t => t.Item)
                .Include(t => t.SourceLocation)
                .ThenInclude(l => l.Shelf)
                .ThenInclude(s => s.Warehouse)
                .Include(t => t.TargetLocation)
                .ThenInclude(l => l.Shelf)
                .ThenInclude(s => s.Warehouse)
                .OrderByDescending(t => t.MovedAt)
                .AsQueryable();

            // Apply search filtering if provided
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                string searchLower = searchString.ToLower();
                query = query.Where(t =>
                    t.Item.Name.ToLower().Contains(searchLower) ||
                    t.SourceLocation.Code.ToLower().Contains(searchLower) ||
                    t.TargetLocation.Code.ToLower().Contains(searchLower) ||
                    t.SourceLocation.Shelf.Warehouse.Name.ToLower().Contains(searchLower) ||
                    t.TargetLocation.Shelf.Warehouse.Name.ToLower().Contains(searchLower)
                );
            }

            // Apply pagination
            var transfers = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return _mapper.Map<List<TransferDto>>(transfers);
        }

        public async Task<int> GetTransfersCount(string searchString = "")
        {
            var query = _context.Transfers.AsQueryable();

            // Apply same search filtering for count
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                string searchLower = searchString.ToLower();
                query = query.Where(t =>
                    t.Item.Name.ToLower().Contains(searchLower) ||
                    t.SourceLocation.Code.ToLower().Contains(searchLower) ||
                    t.TargetLocation.Code.ToLower().Contains(searchLower) ||
                    t.SourceLocation.Shelf.Warehouse.Name.ToLower().Contains(searchLower) ||
                    t.TargetLocation.Shelf.Warehouse.Name.ToLower().Contains(searchLower)
                );
            }

            return await query.CountAsync();
        }

        public async Task<bool> TransferItem(int inventoryItemId, int targetLocationId, int quantity)
        {
            InventoryItem? item = await _context.InventoryItems
                .Include(i => i.Location)
                .FirstOrDefaultAsync(i => i.Id == inventoryItemId);

            if (item == null)
                return false;

            Location? targetLocation = await _context.Locations.FindAsync(targetLocationId);
            if (targetLocation == null)
                return false;

            if (quantity <= 0 || quantity > item.Quantity)
                return false;

            // Check if item already in the target location (avoid unnecessary moves)
            if (item.Location.Id == targetLocationId)
            {
                return true;
            }

            // Create transfer record with proper handling for null source location
            Transfer transfer = new Transfer
            {
                MovedAt = DateTime.UtcNow,
                Item = item,
                SourceLocationId = item.Location?.Id ?? 0, // Use 0 or another placeholder for "no location"
                TargetLocationId = targetLocation.Id,
                Quantity = quantity
            };

            if (quantity == item.Quantity)
            {
                item.Location = targetLocation;
            }
            else
            {
                item.Quantity -= quantity;
                InventoryItem newItem = new InventoryItem
                {
                    Name = item.Name,
                    Description = item.Description,
                    Quantity = quantity,
                    Location = targetLocation
                };

                _context.InventoryItems.Add(newItem);
            }

            // Save transfer record
            _context.Transfers.Add(transfer);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}