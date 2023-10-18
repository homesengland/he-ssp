using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HE.Investments.DocumentService.Models.Table;

namespace HE.Investments.DocumentService.Models.File;

public class FileTableRow
{
    public int Id { get; set; }

    public string FileName { get; set; }

    public int Size { get; set; }

    public string? Metadata { get; set; }

    public FileMetadata? FileMetadata => Metadata != null ? JsonSerializer.Deserialize<FileMetadata>(Metadata) : null;
}
