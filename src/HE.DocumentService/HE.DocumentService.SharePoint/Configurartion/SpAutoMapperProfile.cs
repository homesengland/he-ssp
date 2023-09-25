using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using System.Data;
using HE.DocumentService.SharePoint.Models.File;

namespace HE.DocumentService.SharePoint.Configurartion;

public class SpAutoMapperProfile : Profile
{
    public SpAutoMapperProfile()
    {
        CreateMap<DataRow, FileTableRow>()
        .ForMember(dst => dst.Id, opt => opt.MapFrom(s => s["ID"]))
        .ForMember(dst => dst.FileName, opt => opt.MapFrom(s => s["FileLeafRef"]))
        .ForMember(dst => dst.Size, opt => opt.MapFrom(s => s["File_x0020_Size"]));
    }
}
