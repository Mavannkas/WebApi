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
    }
}
