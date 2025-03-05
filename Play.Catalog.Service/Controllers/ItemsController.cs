using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        private readonly ItemRepository itemRepository = new();

        private static readonly List<ItemDto> items = new()
        {
            new ItemDto(Guid.NewGuid(), "Potion", "Heals 20 HP", 20, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Iron Sword", "Deals 20 Damage", 20, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Bronze Shield", "Deals 10 Damage", 10, DateTimeOffset.UtcNow)
        };

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await itemRepository.GetAllAsync()).Select(item => item.AsDto());
            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItem(Guid id)
        {
            var item = await itemRepository.GetByIdAsync(id);
            if (item is null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> Post(CreateItemDto createItemDto)
        {
            var item = new Item
            {
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };
            await itemRepository.CreateAsync(item);
            var itemDto = item.AsDto();
            return CreatedAtAction(nameof(GetItem), new { id = itemDto.Id }, itemDto);
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