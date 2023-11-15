using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HE.Investments.DocumentService.Models.File;

public class FileUploadModel
{
    [Required]
    public string ListTitle { get; set; }

    [Required]
    public string FolderPath { get; set; }

    // TODO: remove
    public IFormFile? File { get; set; }

    public Stream FileStream { get; set; }

    public string FileName { get; set; }

    [Required]
    public string Metadata { get; set; }

    public bool? Overwrite { get; set; }
}
