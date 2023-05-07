using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using BookApp.BLL.Interfaces;
using System.IO;

namespace WebApplication.Infrastructure
{
    public class FormFileConcrete: IFormFileForService
    {
        private readonly IFormFile formFile;

        public FormFileConcrete(IFormFile formFile)
        {
            this.formFile = formFile;
        }

        public long Length
        {
            get => formFile.Length;
        }

        public string Filename 
        {
            get => formFile.FileName;
        }

        public void CopyTo(Stream target)
        {
            formFile.CopyTo(target);
        }
    }
}
