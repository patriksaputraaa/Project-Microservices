using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Common.Service;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Entities;
using static Play.Inventory.Service.Dtos;

namespace Play.Inventory.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<InventoryItem> itemsRepository;
        private readonly CatalogClient catalogClient;

        public ItemsController(IRepository<InventoryItem> itemsRepository, CatalogClient catalogClient)
        {
            this.itemsRepository = itemsRepository;
            this.catalogClient = catalogClient;
        }

        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAllAsync()
        // {
        //     var items = (await itemsRepository.GetAllAsync()).Select(item => item.AsDto());
        //     return Ok(items);
        // }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetByIdAsync(Guid userId)
        {
            if (userId == Guid.Empty) 
            {
                return BadRequest();
            }
            var catalogItems = await catalogClient.GetCatalogItemsAsync();
            var items = (await itemsRepository.GetAllAsync())
                .Where(item => item.UserId == userId);
            
            var inventoryItemDtos = items.Select(item =>
            {
                var catalogItem = catalogItems.Single(catalogItem => catalogItem.Id == item.CatalogItemId);
                return item.AsDto(catalogItem.Name, catalogItem.Description);
            });
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<InventoryItemDto>> PostAsync(GrantItemsDto grantItemsDto)
        {
            var inventoryItem = await itemsRepository.GetByIdAsync(grantItemsDto.UserId);
            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem
                {
                    UserId = grantItemsDto.UserId,
                    CatalogItemId = grantItemsDto.CatalogItemId,
                    Quantity = grantItemsDto.Quantity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };
                await itemsRepository.CreateAsync(inventoryItem);
            }
            else
            {
                inventoryItem.Quantity += grantItemsDto.Quantity;
                await itemsRepository.UpdateAsync(inventoryItem);
            }
            return Ok();
        }
    }
}