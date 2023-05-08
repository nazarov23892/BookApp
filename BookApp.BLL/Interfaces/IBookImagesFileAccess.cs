using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BookApp.BLL.Interfaces
{
    public interface IBookImagesFileAccess
    {
        public Stream CreateImage(string filename);
        public void RemoveImage(string filename);
    }
}
