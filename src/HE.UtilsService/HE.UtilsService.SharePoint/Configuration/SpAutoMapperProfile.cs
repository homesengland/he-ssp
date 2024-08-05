using System.Data;
using AutoMapper;
using HE.DocumentService.SharePoint.Models.File;

namespace HE.DocumentService.SharePoint.Configuration;

public class SpAutoMapperProfile : Profile
{
    public SpAutoMapperProfile()
    {
        CreateMap<DataRow, FileTableRow>()
            .ForMember(dst => dst.Id, opt => opt.MapFrom(s => s["ID"]))
            .ForMember(dst => dst.FileName, opt => opt.MapFrom(s => s["FileLeafRef"]))
            .ForMember(dst => dst.FolderPath, opt => opt.MapFrom(s => s["FileDirRef"]))
            .ForMember(dst => dst.Size, opt => opt.MapFrom(s => s["File_x0020_Size"]))
            .ForMember(dst => dst.Editor, opt => opt.MapFrom(s => s["Editor"]))
            .ForMember(dst => dst.Modified, opt => opt.MapFrom(s => s["Modified"]))
            .ForMember(dst => dst.Metadata, opt => opt.MapFrom(s => s["_ModerationComments"]));
    }
}
