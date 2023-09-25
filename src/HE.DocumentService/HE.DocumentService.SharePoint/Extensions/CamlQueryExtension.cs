using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace HE.DocumentService.SharePoint.Extensions;

public static class CamlQueryExtension
{
    public static void SetPostion(this CamlQuery query, string? pagingInfo = null)
    {
        if (pagingInfo == null)
        {
            return;
        }

        query.ListItemCollectionPosition = new ListItemCollectionPosition
        {
            PagingInfo = pagingInfo
        };
    }
}
