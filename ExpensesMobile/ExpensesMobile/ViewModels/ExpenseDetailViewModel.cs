﻿using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ExpensesMobile.Models;
using ExpensesMobile.Views;
using Xamarin.Forms;

namespace ExpensesMobile.ViewModels
{
    public class ExpenseDetailViewModel : BaseViewModel
    {
        public Expense Item { get; set; }
        public ExpenseDetailViewModel(Expense item = null)
        {
            Title = item.Name;
            Item = item;
            ExpenseFiles = new ObservableCollection<ExpenseFile>();

            foreach (var expenseFile in Item.ExpenseFiles)
            {
                ExpenseFiles.Add(expenseFile);
            }

            MessagingCenter.Subscribe<PayForExpensePage, Expense>(this, "PayForExpense", async (obj, it) =>
            {
                Item.PaidDate = it.PaidDate;
                Item.PaidValue = it.PaidValue;
                Item.Paid = true;
                await ExpensesDataStore.UpdateItemAsync(Item);
            });
        }

        public ObservableCollection<ExpenseFile> ExpenseFiles { get; set; }

        int quantity = 1;
        public int Quantity
        {
            get { return quantity; }
            set { SetProperty(ref quantity, value); }
        }

        public Command AddFile
        {
            get
            {
                return new Command(async() => await AddAFile());
            }
        }

        public async Task AddAFile()
        {
            await FileService.AddFile((file) =>
            {
                var expenseFiles = new ExpenseFile {ExpenseId = Item.ExpenseId, File = file.Bytes, Name = file.Name, Size = file.Size};
                Item.ExpenseFiles.Add(expenseFiles);
                ExpensesDataStore.UpdateItemAsync(Item);
                ExpenseFiles.Add(expenseFiles);
            });
        }

        public string DescriptionLine1 => $"Za \"{Item.Name}\", gdzie do zapłacenia było {Item.Value} w dniu {Item.DateFor} na rzecz {(Item.IAmPayer ? "kontrahenta" : "Ciebie")}.{(string.IsNullOrEmpty(Item.Description) ? "" : "\nOpis:")}";

        public string DescriptionLine2 => (Item.Paid ? $"Zostało zapłacone {Item.PaidValue} w dniu {Item.PaidDate}" : "Nie zostało jeszcze zapłacone") + (Item.Interactor == null ? "" : $"\nTwój kontrahent to: {Item.Interactor.Name}. {((Item.ExpenseFiles?.Count ?? 0) == 0 ? "" : "\nPliki dołączone:")}");
    }
}
