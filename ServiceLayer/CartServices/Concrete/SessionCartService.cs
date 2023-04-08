using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.CartServices.Concrete
{
    public class SessionCartService : ICartService
    {
        private readonly ICartLinesSessionSaver saver;
        private List<CartLine> lines = new List<CartLine>();

        public SessionCartService(ICartLinesSessionSaver saver)
        {
            this.saver = saver;
            Load();
        }

        public IEnumerable<CartLine> Lines 
        { 
            get => lines; 
        }

        public void Add(BookForCartDto book)
        {
            if (lines
                .FirstOrDefault(l=>l.Book.BookId == book.BookId) != null)
            {
                return;
            }
            lines.Add(new CartLine
            {
                Book = book,
                Quantity = 1
            });
            Save();
        }

        public void Clear()
        {
            lines.Clear();
            Save();
        }

        public void Remove(Guid bookId)
        {
            var line = lines
                .FirstOrDefault(l => l.Book.BookId == bookId);
            if (line == null)
            {
                return;
            }
            lines.Remove(line);
            Save();
        }

        public void SetQuantity(Guid bookId, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    paramName: nameof(quantity),
                    message: "cannot be less or equal zero");
            }
            var line = lines
                .SingleOrDefault(l => l.Book.BookId == bookId);
            if (line == null)
            {
                return;
            }
            line.Quantity = quantity;
            Save();
        }

        private void Save()
        {
            saver.Write(this.lines);
        }

        private void Load()
        {
            var loadedLines = saver.Read();
            lines.Clear();
            if (loadedLines != null)
            {
                lines.AddRange(loadedLines);
            }
        }
    }

    
}
