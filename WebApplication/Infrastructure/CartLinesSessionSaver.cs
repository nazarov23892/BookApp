using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using BookApp.BLL.Services.Cart;

namespace WebApplication.Infrastructure
{
    public class CartLinesSessionSaver : ICartLinesSessionSaver
    {
        private readonly ISession session;
        private const string cartLinesKey = "cart_lines";

        public CartLinesSessionSaver(IHttpContextAccessor httpContextAccessor)
        {
            session = httpContextAccessor.HttpContext.Session;
        }

        public IEnumerable<CartLine> Read()
        {
            var lines = session.ReadJson<IEnumerable<CartLine>>(key: cartLinesKey);
            return lines;
        }

        public void Write(IEnumerable<CartLine> lines)
        {
            session.WriteJson(
                key: cartLinesKey,
                value: lines);
        }
    }
}
