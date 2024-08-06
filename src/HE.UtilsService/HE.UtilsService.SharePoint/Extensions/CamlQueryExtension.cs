using Microsoft.SharePoint.Client;

namespace HE.UtilsService.SharePoint.Extensions;

public static class CamlQueryExtension
{
    public static void SetPosition(this CamlQuery query, string? pagingInfo = null)
    {
        if (pagingInfo == null)
        {
            return;
        }

        query.ListItemCollectionPosition = new ListItemCollectionPosition
        {
            PagingInfo = pagingInfo,
        };
    }
}
