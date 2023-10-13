using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HE.Investments.DocumentService.Models.File;

public class FormFileUploadModel : FileUploadModel
{
    [Required]
    public new IFormFile File { get; set; }
}
