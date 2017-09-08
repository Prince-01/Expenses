using ExpensesMobile.Models;
using ExpensesMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ExpensesMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpenseDetailPage : ContentPage
    {
        ExpenseDetailViewModel viewModel;

        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        public ExpenseDetailPage()
        {
            InitializeComponent();
        }

        public ExpenseDetailPage(ExpenseDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        private void OnFileSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var file = e.SelectedItem as ExpenseFile;
            viewModel.FileService.GetFile(file.Name, file.File);
        }
    }
}