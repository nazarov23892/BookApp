using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookApp.BLL.Generic;

namespace BookApp.BLL.Services.Cart.Concrete
{
    public class SessionCartService : ServiceErrors, ICartService
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
                .FirstOrDefault(l => l.Book.BookId == book.BookId) != null)
            {
                AddError(errorMessage: "item has already been added");
                return;
            }
            if (lines.Count() >= DomainConstants.OrderLineItemsLimit)
            {
                AddError(errorMessage: "limit of line items reached");
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
                AddError(
                    errorMessage: "item not found");
                return;
            }
            lines.Remove(line);
            Save();
        }

        public void SetQuantity(Guid bookId, int quantity)
        {
            if (quantity <= 0)
            {
                AddError(
                    errorMessage: "cannot be less or equal zero",
                    memberNames: nameof(quantity));
                return;
            }
            if (quantity > DomainConstants.MaxQuantityToBuy)
            {
                AddError(
                    errorMessage: $"quantity value cannot be more than ({DomainConstants.MaxQuantityToBuy})",
                    memberNames: nameof(quantity));
                return;
            }
            var line = lines
                .SingleOrDefault(l => l.Book.BookId == bookId);
            if (line == null)
            {
                AddError(
                    errorMessage: $"item not found",
                    memberNames: nameof(bookId));
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
