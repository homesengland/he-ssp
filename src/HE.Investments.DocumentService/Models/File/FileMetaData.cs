using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HE.Investments.DocumentService.Models.File;

public class FileMetadata
{
    public DateTime CreateDate { get; set; }

    public string Creator { get; set; }
}
