using Microsoft.EntityFrameworkCore;

namespace Expenses.Models
{
    public class ExpensesContext : DbContext
    {
        public ExpensesContext (DbContextOptions<ExpensesContext> options)
            : base(options)
        {
        }

        public DbSet<Expenses.Models.ExpenseViewModels.Expense> Expense { get; set; }
    }
}
