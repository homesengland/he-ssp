using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HE.Investments.DocumentService.Models.File;

public class FileTableFilter
{
    [Required]
    [DefaultValue("Loan application")]
    public string ListTitle { get; set; }

    [Required]
    [DefaultValue("invln_loanapplication")]
    public string ListAlias { get; set; }

    [Required]
    public List<string> FolderPaths { get; set; }

    [DefaultValue(null)]
    public string? PagingInfo { get; set; }
}
