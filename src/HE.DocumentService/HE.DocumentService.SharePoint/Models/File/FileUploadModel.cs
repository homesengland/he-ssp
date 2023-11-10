using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HE.DocumentService.SharePoint.Models.File;

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
}
