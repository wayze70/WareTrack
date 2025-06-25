using Microsoft.EntityFrameworkCore;
using WareTrack.Web.Data;
using WareTrack.Web.Models;


namespace WareTrack.Web.Services
{
    public class DatabaseSeeder
    {
        private readonly DataContext _context;
        private readonly Random _random = new Random();

        public DatabaseSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedDatabase()
        {
            // Clear existing data if needed
            await ClearDatabase();

            await SeedWarehouses();
            await SeedShelves();
            await SeedLocations();
            await SeedInventoryItems();
            await SeedTransfers();

            Console.WriteLine("Database seeded successfully!");
        }

        private async Task ClearDatabase()
        {
            // Delete in reverse order to respect foreign key constraints
            _context.Transfers.RemoveRange(_context.Transfers);
            _context.InventoryItems.RemoveRange(_context.InventoryItems);
            _context.Locations.RemoveRange(_context.Locations);
            _context.Shelves.RemoveRange(_context.Shelves);
            _context.Warehouses.RemoveRange(_context.Warehouses);

            await _context.SaveChangesAsync();
        }

        private async Task SeedWarehouses()
        {
            var warehouses = new List<Warehouse>
            {
                new Warehouse
                    { Name = "Hlavní sklad Praha", Description = "Centrální distribuční centrum pro Prahu a okolí" },
                new Warehouse { Name = "Sklad Brno", Description = "Regionální sklad pro Moravu" },
                new Warehouse { Name = "Sklad Ostrava", Description = "Průmyslový sklad pro severní Moravu" },
                new Warehouse { Name = "Logistické centrum Plzeň", Description = "Zásobování pro západní Čechy" },
                new Warehouse { Name = "Sklad Liberec", Description = "Regionální sklad pro severní Čechy" },
                new Warehouse { Name = "Distribuční centrum Olomouc", Description = "Středomoravský distribuční uzel" },
                new Warehouse { Name = "Sklad České Budějovice", Description = "Zásobování pro jižní Čechy" },
                new Warehouse { Name = "Hradec Králové", Description = "Východočeský regionální sklad" },
                new Warehouse { Name = "Sklad Zlín", Description = "Distribuční centrum pro východní Moravu" },
                new Warehouse { Name = "Karlovy Vary", Description = "Sklad pro západočeský kraj" }
            };

            await _context.Warehouses.AddRangeAsync(warehouses);
            await _context.SaveChangesAsync();
        }

        private async Task SeedShelves()
        {
            var warehouses = await _context.Warehouses.ToListAsync();
            var shelves = new List<Shelf>();
            var shelfTypes = new[] { "REG", "PAL", "KON", "BOX", "CHL" };

            foreach (var warehouse in warehouses)
            {
                // 15-25 shelves per warehouse
                int shelfCount = _random.Next(3, 5);
                for (int i = 1; i <= shelfCount; i++)
                {
                    shelves.Add(new Shelf
                    {
                        Code = $"{shelfTypes[_random.Next(shelfTypes.Length)]}{i:D3}",
                        Description = $"Regál {i} v oddělení {_random.Next(1, 6)}",
                        WarehouseId = warehouse.Id
                    });
                }
            }

            await _context.Shelves.AddRangeAsync(shelves);
            await _context.SaveChangesAsync();
        }

        private async Task SeedLocations()
        {
            var shelves = await _context.Shelves.ToListAsync();
            var locations = new List<Location>();

            foreach (var shelf in shelves)
            {
                // 10-20 locations per shelf
                int locationCount = _random.Next(3, 5);
                for (int i = 1; i <= locationCount; i++)
                {
                    var position = $"{(char)('A' + _random.Next(0, 6))}{_random.Next(1, 10)}";
                    locations.Add(new Location
                    {
                        Code = $"{shelf.Code}-{position}",
                        Description = $"Pozice {position} na regálu {shelf.Code}",
                        ShelfId = shelf.Id
                    });
                }
            }

            await _context.Locations.AddRangeAsync(locations);
            await _context.SaveChangesAsync();
        }

        private async Task SeedInventoryItems()
        {
            var locations = await _context.Locations.ToListAsync();
            var items = new List<InventoryItem>();

            string[] categories =
            {
                "Elektronika", "Oblečení", "Potraviny", "Drogerie", "Nábytek", "Nářadí", "Hračky", "Sportovní vybavení"
            };
            string[] products =
            {
                "Notebook Lenovo", "Mobilní telefon Samsung", "Televize LG", "Mikrovlnná trouba", "Pračka Bosch",
                "Tričko pánské", "Kalhoty dámské", "Bunda zimní", "Ponožky", "Čepice",
                "Chléb konzumní", "Rohlíky", "Mléko", "Sýr Eidam", "Jogurt bílý",
                "Šampon", "Mýdlo", "Zubní pasta", "Prací prášek", "Čistící prostředek",
                "Stůl kancelářský", "Židle", "Skříň šatní", "Postel", "Pohovka",
                "Vrtačka", "Kladivo", "Šroubovák", "Pila", "Sada klíčů",
                "Panenka", "Stavebnice", "Autíčko", "Plyšový medvěd", "Společenská hra",
                "Tenisová raketa", "Fotbalový míč", "Běžecké boty", "Cyklistické kolo", "Lyže"
            };

            foreach (var location in locations)
            {
                // 70% chance to have an item
                if (_random.NextDouble() < 0.7)
                {
                    var productIdx = _random.Next(products.Length);
                    var categoryIdx = productIdx / 5; // Maps products to their categories

                    items.Add(new InventoryItem
                    {
                        Name = products[productIdx],
                        Description = $"{products[productIdx]} - {categories[categoryIdx]}",
                        Quantity = _random.Next(1, 101),
                        LocationId = location.Id
                    });
                }
            }

            await _context.InventoryItems.AddRangeAsync(items);
            await _context.SaveChangesAsync();
        }

        private async Task SeedTransfers()
        {
            var items = await _context.InventoryItems.ToListAsync();
            var locations = await _context.Locations.ToListAsync();
            var transfers = new List<Transfer>();

            // Create 200-300 transfers
            int transferCount = _random.Next(200, 301);

            for (int i = 0; i < transferCount; i++)
            {
                var item = items[_random.Next(items.Count)];
                var sourceLocation = await _context.Locations.FindAsync(item.LocationId);
                var targetLocation = locations
                    .Where(l => l.Id != item.LocationId)
                    .OrderBy(x => _random.Next())
                    .FirstOrDefault();

                if (targetLocation != null)
                {
                    var transferDate = DateTime.UtcNow.AddDays(-_random.Next(1, 366));
                    transfers.Add(new Transfer
                    {
                        ItemId = item.Id,
                        SourceLocationId = sourceLocation.Id,
                        TargetLocationId = targetLocation.Id,
                        Quantity = _random.Next(1, Math.Min(item.Quantity + 1, 10)),
                        MovedAt = transferDate
                    });
                }
            }

            await _context.Transfers.AddRangeAsync(transfers);
            await _context.SaveChangesAsync();
        }
    }
}