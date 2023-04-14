using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace ServiceLayer.Abstract
{
    public interface ISignInContext
    {
        public bool IsSignedIn { get; }
        public AppUser User { get; }
        public string UserId { get; }
    }
}
