using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HE.DocumentService.SharePoint.Models.File;

public class FileTableFilter
{
    [Required]
    [DefaultValue("Loan application")]
    public string ListTitle { get; set; }

    [Required]
    [DefaultValue("invln_loanapplication")]
    public string ListAlias { get; set; }

    [Required]
    [DefaultValue("0000000_DA2123DAE440EE11BDF3002248C653E1")]
    public string FolderPath { get; set; }

    [DefaultValue(null)]
    public string? PagingInfo { get; set; }
}
