using System;
using System.Threading.Tasks;
using ExpensesMobile.Models;

namespace ExpensesMobile.Services
{
    public interface IFileService
    {
        Task AddFile(Action<File> postAction);
        Task GetFile(string name, byte[] file);
        string GetLocalFilePath(string filename);
    }
}
