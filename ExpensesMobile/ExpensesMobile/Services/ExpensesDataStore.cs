using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpensesMobile.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(ExpensesMobile.Services.ExpensesDataStore))]
namespace ExpensesMobile.Services
{
    public class ExpensesDataStore : IDataStore<Expense>
    {
        bool isInitialized;
        List<Expense> items;

        public async Task<bool> AddItemAsync(Expense item)
        {
            await InitializeAsync();

            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Expense item)
        {
            await InitializeAsync();

            var _item = items.Where((Expense arg) => arg.ExpenseId == item.ExpenseId).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Expense item)
        {
            await InitializeAsync();

            var _item = items.Where((Expense arg) => arg.ExpenseId == item.ExpenseId).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<Expense> GetItemAsync(string id)
        {
            await InitializeAsync();

            return await Task.FromResult(items.FirstOrDefault(s => s.ExpenseId.ToString() == id));
        }

        public async Task<IEnumerable<Expense>> GetItemsAsync(bool forceRefresh = false)
        {
            await InitializeAsync();

            return await Task.FromResult(items);
        }

        public Task<bool> PullLatestAsync()
        {
            return Task.FromResult(true);
        }


        public Task<bool> SyncAsync()
        {
            return Task.FromResult(true);
        }

        public async Task InitializeAsync()
        {
            if (isInitialized)
                return;

            items = new List<Expense>();
            var _items = new List<Expense>
            {
                new Expense {ExpenseId = 1, Name = "Za korki", Interactor = new Interactor {Name = "Rrrrr"}},
                new Expense {ExpenseId = 2, Name = "Za bulki", Interactor = new Interactor {Name = "Kutas"}},
                new Expense {ExpenseId = 3, Name = "Za loda", Description = "OPISEK", Interactor = new Interactor {Name = "Adad"}},
                new Expense {ExpenseId = 4, Name = "Za frytki", Interactor = new Interactor {Name = "Pipa"}},
                new Expense {ExpenseId = 5, Name = "Za robote"},
            };

            foreach (var item in _items)
            {
                items.Add(item);
            }

            isInitialized = true;
        }
    }
}
