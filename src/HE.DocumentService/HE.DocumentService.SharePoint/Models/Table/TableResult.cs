using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HE.DocumentService.SharePoint.Models.Table;

public class TableResult<T>
{
    public List<T> Items { get; set; }

    [Required]
    public int TotalCount { get; set; }

    public string? PagingInfo { get; set; }

    public TableResult()
    {
        Items = new List<T>();
        TotalCount = 0;
    }

    public TableResult(IEnumerable<T> queryResult, int totalCount = 0, string? pagingInfo = null, int? trimStringLength = null)
    {
        Items = TrimStrings(queryResult.ToList(), trimStringLength ?? 100);
        TotalCount = totalCount;
        PagingInfo = pagingInfo;
    }

    private List<T> TrimStrings(List<T> items, int length)
    {
        foreach (var item in items)
        {
            if (item == null)
            {
                break;
            }

            foreach (var prop in item.GetType().GetProperties())
            {
                if (prop.PropertyType == typeof(string) && prop.GetValue(item) is string val && val.Length > length)
                {
                    val = val[..length];
                    var lastIndexOfWhiteSpace = val.LastIndexOf(" ", System.StringComparison.OrdinalIgnoreCase);
                    prop.SetValue(item, $"{val[..lastIndexOfWhiteSpace]}...");
                }
            }
        }

        return items;
    }
}
