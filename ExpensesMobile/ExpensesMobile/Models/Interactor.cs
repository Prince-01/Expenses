using System.Collections.Generic;

namespace ExpensesMobile.Models
{
    public class Interactor
    {
        public int InteractorId { get; set; }
        public string Name { get; set; }
        public virtual List<Expense> Expenses { get; set; }
    }
}