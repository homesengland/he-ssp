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

    [Required]
    public string PagingInfo { get; set; }

    public TableResult()
    {
        Items = new List<T>();
        TotalCount = 0;
    }

    public TableResult(IEnumerable<T> queryResult, int totalCount = 0, string pagingInfo = null, int? trimStringLength = null)
    {
        Items = TrimStrings(queryResult.ToList(), trimStringLength.HasValue ? trimStringLength.Value : 100);
        TotalCount = totalCount;
        PagingInfo = pagingInfo;
    }

    private List<T> TrimStrings(List<T> items, int length)
    {
        foreach (var item in items)
        {
            foreach (var prop in item.GetType().GetProperties())
            {
                if (prop.PropertyType == typeof(string))
                {
                    var val = prop.GetValue(item) as string;

                    if (val != null && val.Length > length)
                    {
                        val = val.Substring(0, length);
                        var lastIndexOfWhiteSpace = val.LastIndexOf(" ");
                        prop.SetValue(item, $"{val.Substring(0, lastIndexOfWhiteSpace)}...");
                    }
                }
            }
        }

        return items;
    }
}
