using System.ComponentModel.DataAnnotations;

namespace HE.DocumentService.SharePoint.Models.Table;

public class TableResult<T>
{
    public TableResult(IEnumerable<T> queryResult, int totalCount = 0, string? pagingInfo = null, int? trimStringLength = null)
    {
        Items = TrimStrings(queryResult.ToList(), trimStringLength ?? 100);
        TotalCount = totalCount;
        PagingInfo = pagingInfo;
    }

    public List<T> Items { get; set; }

    [Required]
    public int TotalCount { get; set; }

    public string? PagingInfo { get; set; }

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
                    var lastIndexOfWhiteSpace = val.LastIndexOf(" ", StringComparison.OrdinalIgnoreCase);
                    prop.SetValue(item, $"{val[..lastIndexOfWhiteSpace]}...");
                }
            }
        }

        return items;
    }
}
