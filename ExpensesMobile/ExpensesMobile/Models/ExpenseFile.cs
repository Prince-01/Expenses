using SQLite;

namespace ExpensesMobile.Models
{
    public class ExpenseFile
    {
        [PrimaryKey, AutoIncrement]
        public int ExpenseFileId { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public byte[] File { get; set; }
        public int ExpenseId { get; set; }
        [Ignore]
        public virtual Expense Expense { get; set; }
    }
}