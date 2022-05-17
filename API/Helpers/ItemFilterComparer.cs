using APP.Entity;
using System.Collections.Generic;

namespace APP.Helpers
{
    public class ItemFilterComparer : IEqualityComparer<Domain>
    {
        public bool Equals(Domain x, Domain y)
        {
            // Two items are equal if their keys are equal.
            return x.ListingID == y.ListingID && x.Company == y.Company 
                && x.Image_List == y.Image_List;
        }

        public int GetHashCode(Domain obj)
        {
#if (DEBUG)
            return obj.Company.GetHashCode();
#else
            return obj.Id.GetHashCode();
#endif
        }
    }
}
