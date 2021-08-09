using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Mappers;
using WebApi.Repositories;

namespace WebApi.Controllers
{


    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository _respository;
        private readonly DtoMapper _mapper;

        public ItemsController (IItemsRepository respository, DtoMapper mapper)
        {

            _mapper = mapper;
            _respository = respository;
        }

        // GET /items
        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync(string nameToMatch = null)
        {
            var items = await _respository.GetItemsAsync();

            if (!string.IsNullOrWhiteSpace(nameToMatch))
            {
                items = items.Where(item => item.Name.Contains(nameToMatch, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return _mapper.Map(items);
        }


        // GET /items/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await _respository.GetItemAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            return _mapper.Map(item);
        }

        // POST /items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto createItemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                CreateDate = DateTimeOffset.UtcNow,
                Price = createItemDto.Price
            };

            await _respository.CreateItemAsync(item);

            return CreatedAtAction(nameof(GetItemAsync), new { item.Id }, _mapper.Map(item));
        }

        // PUT /items/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = await _respository.GetItemAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            existingItem.Name = updateItemDto.Name;
            existingItem.Price = updateItemDto.Price;

            await _respository.UpdateItemAsync(existingItem);

            return NoContent();
        }

        // DELETE /items/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingItem =await _respository.GetItemAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            await _respository.DeleteItemAsync(id);

            return NoContent();
        }


    }
}
