using System.Threading.Tasks;
using ExpensesMobile.Models;

namespace ExpensesMobile.Services
{
    public interface IFileService
    {
        Task<File> AddFile();
        Task GetFile(string name, byte[] file);
    }
}
