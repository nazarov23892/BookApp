using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookApp.BLL;
using BookApp.BLL.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace WebApplication.Infrastructure
{
    public class BookImagesFileAccess : IBookImagesFileAccess
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public BookImagesFileAccess(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public Stream CreateImage(string filename)
        {
            string fullPath;
            fullPath = GetFullPath(filename);
            var stream = new FileStream(path: fullPath, mode: FileMode.Create);
            return stream;
        }

        public void RemoveImage(string filename)
        {
            string fullPath = GetFullPath(filename);
            if (File.Exists(path: fullPath))
            {
                File.Delete(path: fullPath);
            }
        }

        private string GetFullPath(string filename)
        {
            string rootPath = webHostEnvironment.WebRootPath;
            string imageFolder = DomainConstants.BookImageFolder;
            return Path.Combine(rootPath, imageFolder, filename);
        }
    }
}
