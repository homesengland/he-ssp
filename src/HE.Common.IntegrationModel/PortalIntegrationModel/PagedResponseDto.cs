using System.Collections.Generic;

namespace HE.Common.IntegrationModel.PortalIntegrationModel
{
    public class PagedResponseDto<T>
    {
        public PagingRequestDto paging { get; set; }

        public IList<T> items { get; set; }

        public int totalItemsCount { get; set; }
    }
}
