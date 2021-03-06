﻿using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using ExpensesMobile.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(ExpensesMobile.UWP.Services.FileService))]
namespace ExpensesMobile.UWP.Services
{
    public class FileService : IFileService
    {
        public async Task AddFile(Action<ExpensesMobile.Models.File> postAction)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".pdf");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            var f = await file.OpenReadAsync();
            var stream = f.AsStream();
            var mStream = new MemoryStream();
            stream.CopyTo(mStream);
            postAction(new ExpensesMobile.Models.File
            {
                Name = file.Name,
                Bytes = mStream.ToArray(),
                Size = mStream.Length
            });
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
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);
        }
    }
}
