using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using Play.Catalog.Service.Entities;
using Play.Common;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> itemRepository;

        public ItemsController(IRepository<Item> itemRepository)
        {
            this.itemRepository = itemRepository;
        }

        private static readonly List<ItemDto> items = new()
        {
            new ItemDto(Guid.NewGuid(), "Potion", "Heals 20 HP", 20, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Iron Sword", "Deals 20 Damage", 20, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Bronze Shield", "Deals 10 Damage", 10, DateTimeOffset.UtcNow)
        };

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAll()
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
        public async Task<ActionResult> Put(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = await itemRepository.GetByIdAsync(id);
            if (existingItem is null)
            {
                return NotFound();
            }
            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;
            await itemRepository.UpdateAsync(existingItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var existingItem = await itemRepository.GetByIdAsync(id);
            if (existingItem is null)
            {
                return NotFound();
            }
            await itemRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}