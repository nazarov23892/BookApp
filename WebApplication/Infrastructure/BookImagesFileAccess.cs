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
            string rootPath = webHostEnvironment.WebRootPath;
            string imageFolder = DomainConstants.BookImageFolder;
            string fullPath = Path.Combine(rootPath, imageFolder, filename);
            var stream = new FileStream(path: fullPath, mode: FileMode.Create);
            return stream;
        }
    }
}
