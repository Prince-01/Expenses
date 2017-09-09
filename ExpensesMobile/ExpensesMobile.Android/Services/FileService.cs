using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Widget;
using ExpensesMobile.Models;
using ExpensesMobile.Services;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Environment = System.Environment;

[assembly: Dependency(typeof(ExpensesMobile.Droid.Services.FileService))]
namespace ExpensesMobile.Droid.Services
{
    public class FileService : IFileService
    {
        internal static Action<ExpensesMobile.Models.File> _addFilePostAction;

        public async Task AddFile(Action<ExpensesMobile.Models.File> postAction)
        {
            _addFilePostAction = postAction;

            Intent intent = new Intent(Intent.ActionGetContent);
            intent.SetType("*/*");
            intent.AddCategory(Intent.CategoryOpenable);

            try
            {
                (Forms.Context as MainActivity).StartActivityForResult(
                    Intent.CreateChooser(intent, "Select a File to Upload"), 1,
                    Bundle.Empty);
            }
            catch (ActivityNotFoundException ex)
            {
                // Potentially direct the user to the Market with a Dialog
                Toast.MakeText(Forms.Context, "Please install a File Manager.",
                    ToastLength.Short).Show();
            }
        }

        public async Task GetFile(string name, byte[] file)
        {
            /*var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            var f = await file.OpenReadAsync();
            var stream = f.AsStream();
            var mStream = new MemoryStream();
            stream.CopyTo(mStream);
            return new ExpensesMobile.Models.File
            {
                Name = file.Name,
                Bytes = mStream.ToArray()
            };*/
        }

        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}