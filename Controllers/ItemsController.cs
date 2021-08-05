using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Repositories;

namespace WebApi.Controllers
{


    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly InMemItemsRespository _respository;

        public ItemsController()
        {
            _respository = new InMemItemsRespository();
        }

        // GET /items
        [HttpGet]
        public IEnumerable<Item> GetItems()
        {
            var items = _respository.GetItems();
            return items;
        }

        // GET /items/id
        [HttpGet("{id}")]
        public ActionResult<Item> GetItem(Guid id)
        {
            var item = _respository.GetItem(id);

            if (item is null)
            {
                return NotFound();
            }

            return item;
        }
    }
}
