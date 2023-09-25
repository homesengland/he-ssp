using AutoMapper;
using HE.DocumentService.SharePoint.Configurartion;
using HE.DocumentService.SharePoint.Interfaces;
using Microsoft.SharePoint.Client;

namespace HE.DocumentService.SharePoint.Services;

public class BaseService
{
    internal static readonly int RETRY_COUNT = 4;

    internal readonly ClientContext _spContext;
    internal readonly ISharePointConfiguration _spConfig;
    internal readonly IMapper _mapper;

    public BaseService(ISharePointContext spContext, ISharePointConfiguration spConfig, IMapper mapper)
    {
        _spConfig = spConfig;
        _mapper = mapper;
        _spContext = spContext.Context;
    }
}
