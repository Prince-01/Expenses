using System;
using ExpensesMobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpensesMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewExpensePage : ContentPage
    {
        public Expense Item { get; set; }

        public NewExpensePage()
        {
            InitializeComponent();

            Item = new Expense
            {
                Name = "Item name",
                Description = "This is a nice description"
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopToRootAsync();
        }
    }
}