namespace ExpensesMobile.Models
{
    public class ExpenseFile
    {
        public int ExpenseFileId { get; set; }
        public string Name { get; set; }
        public byte[] File { get; set; }
        public int ExpenseId { get; set; }
        public virtual Expense Expense { get; set; }
    }
}