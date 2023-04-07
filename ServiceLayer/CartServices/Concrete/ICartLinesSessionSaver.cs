﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.CartServices.Concrete
{
    public interface ICartLinesSessionSaver
    {
        IEnumerable<CartLine> Read();
        void Write(IEnumerable<CartLine> lines);
    }
}
