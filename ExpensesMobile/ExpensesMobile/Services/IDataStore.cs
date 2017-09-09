using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpensesMobile.Services
{
    public interface IDataStore<T>
    {
        Task<int> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(T item);
        Task<T> GetItemAsync(int id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<T>> GetItemsAsync(Predicate<T> pred, bool forceRefresh = false);

        Task InitializeAsync();
        Task<bool> PullLatestAsync();
        Task<bool> SyncAsync();
    }
}
