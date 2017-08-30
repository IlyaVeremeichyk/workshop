using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortfolioManagerProxy.Models.Comparers
{
    public class PortfolioItemModelComparer : IEqualityComparer<PortfolioItemModel>
    {
        public bool Equals(PortfolioItemModel lhs, PortfolioItemModel rhs)
        {
            if (lhs == null && rhs == null)
                return true;

            if (lhs == null || rhs == null)
                return false;

            if (lhs.ItemId == rhs.ItemId && lhs.UserId == rhs.UserId 
                && lhs.Symbol == rhs.Symbol && lhs.SharesNumber == rhs.SharesNumber)
                return true;

            return false;
        }

        public int GetHashCode(PortfolioItemModel obj)
        {
            unchecked
            {
                int hash = (int)2166136261;

                hash = (hash * 16777619) ^ obj.ItemId.GetHashCode();
                hash = (hash * 16777619) ^ obj.UserId.GetHashCode();
                hash = (hash * 16777619) ^ obj.Symbol.GetHashCode();
                hash = (hash * 16777619) ^ obj.SharesNumber.GetHashCode();
                return hash;
            }
        }
    }
}