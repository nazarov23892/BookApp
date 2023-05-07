using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookApp.BLL.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace WebApplication.Infrastructure
{
    public class EnvironmentConcrete : IEnvironment
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public EnvironmentConcrete(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public string RootPath 
        {
            get => webHostEnvironment.WebRootPath;
        }
    }
}
