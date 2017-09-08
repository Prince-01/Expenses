namespace ExpensesMobile.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            LoadApplication(new ExpensesMobile.App());
        }
    }
}