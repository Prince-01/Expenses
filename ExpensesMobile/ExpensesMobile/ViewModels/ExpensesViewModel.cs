using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ExpensesMobile.Helpers;
using ExpensesMobile.Models;
using ExpensesMobile.Views;
using Xamarin.Forms;

namespace ExpensesMobile.ViewModels
{
    public class ExpensesViewModel : BaseViewModel
    {
        public ObservableRangeCollection<Expense> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ExpensesViewModel()
        {
            Title = "Browse";
            Items = new ObservableRangeCollection<Expense>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewExpensePage, Expense>(this, "AddItem", async (obj, item) =>
            {
                var _item = item as Expense;
                Items.Add(_item);
                await ExpensesDataStore.AddItemAsync(_item);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await ExpensesDataStore.GetItemsAsync(true);
                Items.ReplaceRange(items);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to load items.",
                    Cancel = "OK"
                }, "message");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
