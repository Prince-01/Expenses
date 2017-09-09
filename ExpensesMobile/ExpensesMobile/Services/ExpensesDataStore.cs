using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpensesMobile.Models;
using SQLite;
using Xamarin.Forms;

[assembly: Dependency(typeof(ExpensesMobile.Services.ExpensesDataStore))]
namespace ExpensesMobile.Services
{
    public class ExpensesDataStore : IDataStore<Expense>
    {
        bool _isInitialized;

        private readonly SQLiteAsyncConnection _database;
        
        public ExpensesDataStore()
        {
            _database = new SQLiteAsyncConnection(DependencyService.Get<IFileService>()
                .GetLocalFilePath("ExpensesDatabase.db3"));

            //_database.DropTableAsync<Interactor>().Wait();
            //_database.DropTableAsync<Expense>().Wait();
            //_database.DropTableAsync<ExpenseFile>().Wait();

            _database.CreateTableAsync<Interactor>().Wait();
            _database.CreateTableAsync<Expense>().Wait();
            _database.CreateTableAsync<ExpenseFile>().Wait();
        }

        public async Task<int> AddItemAsync(Expense item)
        {
            await InitializeAsync();

            return await _database.InsertAsync(item);
        }

        public async Task<bool> UpdateItemAsync(Expense item)
        {
            await InitializeAsync();

            await _database.UpdateAsync(item);
            var expenseFiles = await _database.Table<ExpenseFile>().ToListAsync();

            foreach (var expenseFile in item.ExpenseFiles.Where(ef => ef.ExpenseFileId == 0))
            {
                expenseFile.ExpenseFileId = await _database.InsertAsync(expenseFile);
            }

            foreach (var expenseFile in expenseFiles.Where(ef => item.ExpenseFiles.All(eff => ef.ExpenseFileId != eff.ExpenseFileId)))
            {
                await _database.DeleteAsync(expenseFile);
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Expense item)
        {
            await InitializeAsync();

            await _database.DeleteAsync(item);

            return await Task.FromResult(true);
        }

        public async Task<Expense> GetItemAsync(int id)
        {
            await InitializeAsync();

            var expense = await _database.Table<Expense>().Where(e => e.ExpenseId == id).FirstAsync();
            if (expense.InteractorId != null)
            {
                expense.Interactor = await _database.Table<Interactor>()
                    .Where(i => i.InteractorId == (int) expense.InteractorId).FirstAsync();
            }
            expense.ExpenseFiles = await _database.Table<ExpenseFile>().Where(ef => ef.ExpenseId == id)
                .ToListAsync();

            return expense;

        }

        public async Task<IEnumerable<Expense>> GetItemsAsync(bool forceRefresh = false)
        {
            return await GetItemsAsync(e => true);
        }

        public async Task<IEnumerable<Expense>> GetItemsAsync(Predicate<Expense> pred, bool forceRefresh = false)
        {
            await InitializeAsync();

            var expenses = (await _database.Table<Expense>().ToListAsync()).Where(e => pred(e)).ToList();
            var interactors = await _database.Table<Interactor>().ToListAsync();

            foreach (var expense in expenses.Where(e => e.InteractorId != null))
            {
                expense.Interactor = interactors.FirstOrDefault(i => i.InteractorId == expense.InteractorId);
            }

            return expenses;
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
            if (_isInitialized)
                return;

            // do some initialization

            _isInitialized = true;
        }
    }
}
