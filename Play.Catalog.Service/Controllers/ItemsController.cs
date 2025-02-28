using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        private static readonly List<ItemDto> items = new()
        {
            new ItemDto(Guid.NewGuid(), "Potion", "Heals 20 HP", 20, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Iron Sword", "Deals 20 Damage", 20, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Bronze Shield", "Deals 10 Damage", 10, DateTimeOffset.UtcNow)
        };

        [HttpGet]
        public IEnumerable<ItemDto> GetItems()
        {
            return items;
        }

        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id)
        {
            var item = items.Where(item => item.Id == id).SingleOrDefault();
            if (item is null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
        {
            var item = new ItemDto(Guid.NewGuid(), itemDto.Name, itemDto.Description, itemDto.Price, DateTimeOffset.UtcNow);
            items.Add(item);
            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = items.Where(item => item.Id == id).SingleOrDefault();
            if (existingItem is null)
            {
                return NotFound();
            }
            var item = existingItem with
            {
                Name = itemDto.Name,
                Description = itemDto.Description,
                Price = itemDto.Price
            };

            var index = items.FindIndex(existingItem => existingItem.Id == id);
            items[index] = item;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteItem(Guid id)
        {
            var existingItem = items.Where(item => item.Id == id).SingleOrDefault();
            if (existingItem is null)
            {
                return NotFound();
            }
            items.Remove(existingItem);
            return NoContent();
        }
    }
}