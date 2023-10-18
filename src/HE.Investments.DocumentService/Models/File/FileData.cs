using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HE.Investments.DocumentService.Models.File;

public class FileData
{
    public string Name { get; set; }

    public byte[] Data { get; set; }

    /// <summary>
    /// Extension of the file Name
    /// </summary>
    public string Ext => Path.GetExtension(Name ?? "").Replace(".", "");

    public FileData()
    {
    }

    public FileData(string name, Stream data)
    {
        Name = name;

        using var ms = new MemoryStream();
        data.CopyTo(ms);
        Data = ms.ToArray();
    }

    public FileData(string name, byte[] data)
    {
        Name = name;
        Data = data;
    }

    public FileData(IFormFile file)
    {
        using var ms = new MemoryStream();
        file.CopyTo(ms);
        Data = ms.ToArray();
        Name = file.FileName;
    }
}
