using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace BookApp.BLL.Interfaces
{
    public interface IFormFileForService
    {
        public long Length { get; }
        public string Filename { get; }
        public void CopyTo(Stream target);
    }
}
