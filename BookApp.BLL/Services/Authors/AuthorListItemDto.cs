using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL.Services.Authors
{
    public class AuthorListItemDto
    {
        public Guid AuthorId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
