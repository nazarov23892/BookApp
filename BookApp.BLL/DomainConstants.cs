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
        public const int BookDescriptionMinLenght = 10;
        public const int BookDescriptionMaxLenght = 2048;
        public const string BookImageFolder = @"uploads\images";
        public const int BookImageMaxSizeMb = 1;
        public const long BookImageMaxSizeBytes = BookImageMaxSizeMb * 1024 * 1024;
    }
}
