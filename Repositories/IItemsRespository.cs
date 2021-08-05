using System;
using System.Collections.Generic;
using WebApi.Entities;

namespace WebApi.Repositories
{
    public interface IItemsRepository
    {
        Item GetItem(Guid id);
        IEnumerable<Item> GetItems();
    }
}