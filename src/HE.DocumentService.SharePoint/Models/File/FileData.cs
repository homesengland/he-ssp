using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HE.DocumentService.SharePoint.Models.File;

public class FileData
{
    public string Name { get; set; }

    public byte[] Data { get; set; }

    /// <summary>
    /// Extension of the file Name
    /// </summary>
    public string Ext => Path.GetExtension(Name ?? "").Replace(".", "");

    public FileData() { }

    public FileData(string name, Stream data)
    {
        Name = name;

        using (MemoryStream ms = new MemoryStream())
        {
            data.CopyTo(ms);
            Data = ms.ToArray();
        }
    }
}
