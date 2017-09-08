using System;
using System.Collections.Generic;

namespace ExpensesMobile.Models
{
    public class Expense
    {
        public int ExpenseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? DateFor { get; set; }
        public double Value { get; set; }
        public bool Paid { get; set; }
        public DateTime? PaidDate { get; set; }
        public double? PaidValue { get; set; }
        public bool IAmPayer { get; set; }
        public int? InteractorId { get; set; }
        public virtual Interactor Interactor { get; set; } = new Interactor();
        public virtual List<ExpenseFile> ExpenseFiles { get; set; } = new List<ExpenseFile>();
    }
}
