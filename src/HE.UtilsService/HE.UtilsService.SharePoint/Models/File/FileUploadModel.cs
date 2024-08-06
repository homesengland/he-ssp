using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HE.UtilsService.SharePoint.Models.File;

public class FileUploadModel
{
    [Required]
    public string ListTitle { get; set; }

    [Required]
    public string FolderPath { get; set; }

    [Required]
    public IFormFile File { get; set; }

    [Required]
    public string Metadata { get; set; }

    public bool? Overwrite { get; set; }

    public string? PartitionId { get; set; }
}
