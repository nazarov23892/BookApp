using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BookApp.BLL.Generic
{
    public interface IServiceErrors
    {
        public IEnumerable<ValidationResult> Errors { get; }
        public bool HasErrors { get; }
    }
}
