using System;

namespace ExpensesMobile.ViewModels
{
    public class NewExpenseViewModel : BaseViewModel
    {
        public DateTime? DateFor { get; set; }
        public string Description { get; set; }
        public bool IAmPayer { get; set; }
        public int? InteractorId { get; set; }
        public string Name { get; set; }

        private bool _paid;
        public bool Paid
        {
            get { return _paid; }
            set
            {
                SetProperty(ref _paid, value);
                PaidValue = Value;
            }
        }

        public DateTime? PaidDate { get; set; }

        private double? _paidValue;
        public double? PaidValue
        {
            get { return _paidValue; }
            set { SetProperty(ref _paidValue, value); }
        }

        private double _value;
        public double Value
        {
            get { return _value; }
            set
            {
                SetProperty(ref _value, value);
                if (!Paid)
                    PaidValue = value;
            }
        }
    }
}
