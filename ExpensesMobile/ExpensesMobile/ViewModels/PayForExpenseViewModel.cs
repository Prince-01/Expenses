using System;

namespace ExpensesMobile.ViewModels
{
    public class PayForExpenseViewModel : BaseViewModel
    {
        private string _paidValue;
        public string PaidValue
        {
            get { return _paidValue; }
            set { SetProperty(ref _paidValue, value); }
        }

        private DateTime _paidDate;
        public DateTime PaidDate
        {
            get { return _paidDate; }
            set { SetProperty(ref _paidDate, value); }
        }
    }
}
