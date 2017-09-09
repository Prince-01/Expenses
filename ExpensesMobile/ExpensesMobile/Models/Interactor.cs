using System.Collections.Generic;
using SQLite;

namespace ExpensesMobile.Models
{
    public class Interactor
    {
        [PrimaryKey, AutoIncrement]
        public int InteractorId { get; set; }
        public string Name { get; set; }
        [Ignore]
        public virtual List<Expense> Expenses { get; set; }
    }
}