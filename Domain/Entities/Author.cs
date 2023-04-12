using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Author
    {
        public Guid AuthorId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public ICollection<BookAuthor> BooksLink { get; set; }
    }
}
