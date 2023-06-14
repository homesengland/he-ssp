using Microsoft.Xrm.Sdk;
using System.Collections.Generic;

namespace HE.Base.HE.Base.Common.EqualityComparers
{
    public class EntityIdComparer : IEqualityComparer<Entity>
    {
        public bool Equals(Entity x, Entity y)
        {
            return x.Id == y.Id && x.LogicalName == y.LogicalName;
        }

        public int GetHashCode(Entity obj)
        {
            return obj.Id.GetHashCode() + obj.LogicalName.GetHashCode();
        }
    }
}
