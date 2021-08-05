using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Dtos;
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

        public ItemsController(IItemsRepository respository, DtoMapper mapper)
        {

            _mapper = mapper;
            _respository = respository;
        }

        // GET /items
        [HttpGet]
        public IEnumerable<ItemDto> GetItems()
        {
            var items = _respository.GetItems();
            return _mapper.Map(items);
        }

        // GET /items/{id}
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id)
        {
            var item = _respository.GetItem(id);

            if (item is null)
            {
                return NotFound();
            }

            return _mapper.Map(item);
        }

        // POST /items
        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto createItemDto)
        {
            Item item = new()
            {
                Id = Guid.NewGuid(),
                Name = createItemDto.Name,
                CreateDate = DateTimeOffset.UtcNow,
                Price = createItemDto.Price
            };

            _respository.CreateItem(item);

            return CreatedAtAction(nameof(GetItem), new { item.Id }, _mapper.Map(item));
        }

        // PUT /items/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = _respository.GetItem(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            Item updatedItem = existingItem with
            {
                Name = updateItemDto.Name,
                Price = updateItemDto.Price,
            };

            _respository.UpdateItem(updatedItem);

            return NoContent();
        }

        // DELETE /items/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteItem(Guid id)
        {
            var existingItem = _respository.GetItem(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            _respository.DeleteItem(id);

            return NoContent();
        }
    }
}
