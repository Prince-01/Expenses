using System;
using ExpensesMobile.Models;
using ExpensesMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpensesMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpensesPage : ContentPage
    {
        ExpensesViewModel viewModel;

        public ExpensesPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ExpensesViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Expense;
            if (item == null)
                return;

            await Navigation.PushAsync(new ExpenseDetailPage(new ExpenseDetailViewModel(item)));

            // Manually deselect item
            ExpensesListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewExpensePage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}