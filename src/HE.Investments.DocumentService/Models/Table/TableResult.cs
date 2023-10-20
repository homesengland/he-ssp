using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HE.Investments.DocumentService.Models.Table;

public class TableResult<T>
{
    public IList<T> Items { get; set; }

    [Required]
    public int TotalCount { get; set; }

    public string? PagingInfo { get; set; }
}
