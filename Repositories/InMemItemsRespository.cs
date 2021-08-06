using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;

namespace WebApi.Repositories
{
    public class InMemItemsRespository : IItemsRepository
    {
        private readonly List<Item> _items = new()
        {
            new Item { Id = Guid.NewGuid(), Name = "Potion", Price = 9, CreateDate = DateTimeOffset.UtcNow },
            new Item { Id = Guid.NewGuid(), Name = "Iron sword", Price = 20, CreateDate = DateTimeOffset.UtcNow },
            new Item { Id = Guid.NewGuid(), Name = "Bronze Shield", Price = 19, CreateDate = DateTimeOffset.UtcNow }
        };

        public async Task<IEnumerable<Item>> GetItemsAsync()
        {
            return await Task.FromResult(_items);
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            return await Task.FromResult(_items.Where(item => item.Id == id).SingleOrDefault());
        }

        public async Task CreateItemAsync(Item item)
        {
            _items.Add(item);
           await Task.CompletedTask;
        }

        public async Task UpdateItemAsync(Item item)
        {
            var index = _items.FindIndex(existingItem => existingItem.Id == item.Id);
            _items[index] = item;
            await Task.CompletedTask;

        }

        public async Task DeleteItemAsync(Guid id)
        {
            var item = _items.FindIndex(item => item.Id == id);
            _items.RemoveAt(item);
            await Task.CompletedTask;

        }
    }
}
