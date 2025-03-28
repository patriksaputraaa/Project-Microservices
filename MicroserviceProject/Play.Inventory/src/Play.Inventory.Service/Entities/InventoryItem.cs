using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Play.Common.Service;

namespace Play.Inventory.Service.Entities
{
    public class InventoryItem : IEntity
    {
        public Guid Id { get ; init; }
        public Guid UserId { get ; init; } = Guid.Empty;
        public Guid CatalogItemId { get ; init; } = Guid.Empty;
        public int Quantity { get; set; } = 0;
        public DateTimeOffset AcquiredDate { get; init; }
    }
}