using AutoMapper;
using HE.DocumentService.SharePoint.Configurartion;
using HE.DocumentService.SharePoint.Interfaces;
using Microsoft.SharePoint.Client;

namespace HE.DocumentService.SharePoint.Services;

public class BaseService
{
    internal static readonly int RETRY_COUNT = 4;

    internal readonly ClientContext spContext;
    internal readonly ISharePointConfiguration spConfig;
    internal readonly IMapper mapper;

    public BaseService(ISharePointContext spContext, ISharePointConfiguration spConfig, IMapper mapper)
    {
        this.spConfig = spConfig;
        this.mapper = mapper;
        this.spContext = spContext.context;
    }
}
