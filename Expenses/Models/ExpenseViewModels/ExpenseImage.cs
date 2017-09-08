namespace Expenses.Models.ExpenseViewModels
{
    public class ExpenseImage
    {
        public int ExpenseImageId { get; set; }
        public byte[] Image { get; set; }
        public int ExpenseId { get; set; }
        public virtual Expense Expense { get; set; }
    }
}