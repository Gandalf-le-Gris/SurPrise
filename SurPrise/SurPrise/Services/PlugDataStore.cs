using SurPrise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurPrise.Services
{
    class PlugDataStore : IDataStore<PlugItem>
    {
        readonly List<PlugItem> items;

        public PlugDataStore()
        {
            items = new List<PlugItem>()
            {
                /*new PlugItem { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
                new PlugItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
                new PlugItem { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
                new PlugItem { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
                new PlugItem { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
                new PlugItem { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." }*/
            };
        }

        public async Task<bool> AddItemAsync(PlugItem item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(PlugItem item)
        {
            var oldPlugItem = items.Where((PlugItem arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldPlugItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldPlugItem = items.Where((PlugItem arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldPlugItem);

            return await Task.FromResult(true);
        }

        public async Task<PlugItem> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<PlugItem>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}
