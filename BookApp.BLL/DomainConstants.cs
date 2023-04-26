using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApp.BLL
{
    public static class DomainConstants
    {
        public const int MaxQuantityToBuy = 10;
        public const double MinPrice = 0.99;
        public const double MaxPrice = 9_999.99;
        public const int OrderLineItemsLimit = 10;
        public const string UsersRoleName = "Users";
    }
}
