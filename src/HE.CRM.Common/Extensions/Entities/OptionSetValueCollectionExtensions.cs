using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;

namespace HE.CRM.Common.Extensions.Entities
{
    public static class OptionSetValueCollectionExtensions
    {
        public static IList<int> ToIntValueList(this OptionSetValueCollection collection)
        {
            return collection == null ? Enumerable.Empty<int>().ToList() : collection.Select(i => i.Value).ToList();
        }
    }
}
