using System;
using ExpensesMobile.Models;
using ExpensesMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpensesMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewExpensePage : ContentPage
    {
        private NewExpenseViewModel viewModel;

        public NewExpensePage()
        {
            InitializeComponent();

            BindingContext = viewModel = new NewExpenseViewModel
            {
                DateFor = DateTime.Now,
                PaidDate = DateTime.Now
            };
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            //create item
            if (string.IsNullOrEmpty(viewModel.Name))
            {
                await DisplayAlert("Error", "Please name it somehow.", "Ok");
                return;
            }

            if (viewModel.Value <= 0)
            {
                await DisplayAlert("Error", "Your value must be greater than 0.", "Ok");
                return;
            }

            var expense = new Expense
            {
                Created = DateTime.Now,
                DateFor = viewModel.DateFor,
                Description = viewModel.Description,
                IAmPayer = viewModel.IAmPayer,
                InteractorId = viewModel.InteractorId,
                Name = viewModel.Name,
                Paid = viewModel.Paid,
                PaidDate = viewModel.PaidDate,
                PaidValue = viewModel.PaidValue,
                Value = viewModel.Value
            };
            MessagingCenter.Send(this, "AddItem", expense);
            await Navigation.PopToRootAsync();
        }
    }
}