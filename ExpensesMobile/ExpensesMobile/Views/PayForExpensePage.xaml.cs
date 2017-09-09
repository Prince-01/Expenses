using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpensesMobile.Models;
using ExpensesMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpensesMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PayForExpensePage : ContentPage
    {
        private PayForExpenseViewModel viewModel;

        public PayForExpensePage(PayForExpenseViewModel vm)
        {
            InitializeComponent();

            BindingContext = viewModel = vm;
        }

        private void Save_Clicked(object sender, EventArgs e)
        {
            double d = 0;
            if (!(string.IsNullOrEmpty(viewModel.PaidValue) ||
                double.TryParse(viewModel.PaidValue, out d))) return;
            MessagingCenter.Send(this, "PayForExpense", new Expense
            {
                PaidDate = viewModel.PaidDate,
                PaidValue = string.IsNullOrEmpty(viewModel.PaidValue) ? null : (double?)d
            });
            Navigation.PopModalAsync();
        }
    }
}