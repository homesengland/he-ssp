using System.Text.Json;
using System.Text.Json.Serialization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.User;

namespace HE.Investment.AHP.Domain.HomeTypes.Crm;

public class HomeTypeCrmContext : IHomeTypeCrmContext
{
    private static readonly string HomeTypeCrmFields =
        string.Join(
                ",",
                nameof(invln_HomeType.invln_typeofhousing),
                nameof(invln_HomeType.invln_hometypename),
                nameof(invln_HomeType.CreatedOn),
                nameof(invln_HomeType.invln_ishometypecompleted),
                nameof(invln_HomeType.invln_designplancomments),
                nameof(invln_HomeType.invln_happiprinciples),
                nameof(invln_HomeType.invln_typeofhousingfordisabledvulnerablepeople),
                nameof(invln_HomeType.invln_clientgroup),
                nameof(invln_HomeType.invln_numberofhomeshometype),
                nameof(invln_HomeType.invln_numberofbedrooms),
                nameof(invln_HomeType.invln_maxoccupancy),
                nameof(invln_HomeType.invln_numberofstoreys),
                nameof(invln_HomeType.invln_homesusedformoveonaccommodation),
                nameof(invln_HomeType.invln_homesdesignedforuseofparticular),
                nameof(invln_HomeType.invln_buildingtype),
                nameof(invln_HomeType.invln_custombuild),
                nameof(invln_HomeType.invln_facilities),
                nameof(invln_HomeType.invln_iswheelchairstandardmet),
                nameof(invln_HomeType.invln_accessibilitycategory),
                nameof(invln_HomeType.invln_floorarea),
                nameof(invln_HomeType.invln_doallhomesmeetNDSS),
                nameof(invln_HomeType.invln_whichndssstandardshavebeenmet),
                "invln_mmcapplied", // TODO: AB#86986 use nameof when earlybound will be provided
                nameof(invln_HomeType.invln_mmccategories),
                nameof(invln_HomeType.invln_mmccategory1subcategories),
                nameof(invln_HomeType.invln_mmccategory2subcategories),
                nameof(invln_HomeType.invln_typeofolderpeopleshousing),
                nameof(invln_HomeType.invln_localcommissioningbodiesconsulted),
                nameof(invln_HomeType.invln_homesusedforshortstay),
                nameof(invln_HomeType.invln_revenuefunding),
                nameof(invln_HomeType.invln_revenuefundingsources),
                nameof(invln_HomeType.invln_moveonarrangementsforshortstayhomes),
                nameof(invln_HomeType.invln_typologylocationanddesing),
                nameof(invln_HomeType.invln_supportedhousingexitplan),
                nameof(invln_HomeType.invln_MarketValueofEachProperty),
                nameof(invln_HomeType.invln_MarketRentperWeek),
                nameof(invln_HomeType.invln_ProspectiveRentperWeek),
                nameof(invln_HomeType.invln_prospectiverentasofmarketrent),
                nameof(invln_HomeType.invln_targetrentover80ofmarketrent),
                nameof(invln_HomeType.invln_rtsoexempt),
                nameof(invln_HomeType.invln_reasonsforrtsoexemption),
                nameof(invln_HomeType.invln_SharedOwnershipInitialSale),
                nameof(invln_HomeType.invln_FirstTrancheSalesReceipt),
                nameof(invln_HomeType.invln_proposedrentasaofunsoldshare))
            .ToLowerInvariant();

    private readonly JsonSerializerOptions _serializerOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

    private readonly ICrmService _service;

    private readonly IUserContext _userContext;

    public HomeTypeCrmContext(ICrmService service, IUserContext userContext)
    {
        _service = service;
        _userContext = userContext;
    }

    public async Task<IList<HomeTypeDto>> GetAllOrganisationHomeTypes(
        string applicationId,
        Guid organisationId,
        CancellationToken cancellationToken)
    {
        var request = new invln_gettypeofhomeslistRequest
        {
            invln_userid = string.Empty,
            invln_organisationid = organisationId.ToString(),
            invln_applicationid = applicationId,
            invln_fieldstoretrieve = HomeTypeCrmFields,
        };

        return await GetAll(request, cancellationToken);
    }

    public async Task<IList<HomeTypeDto>> GetAllUserHomeTypes(
        string applicationId,
        Guid organisationId,
        CancellationToken cancellationToken)
    {
        var request = new invln_gettypeofhomeslistRequest
        {
            invln_userid = _userContext.UserGlobalId,
            invln_organisationid = organisationId.ToString(),
            invln_applicationid = applicationId,
            invln_fieldstoretrieve = HomeTypeCrmFields,
        };

        return await GetAll(request, cancellationToken);
    }

    public async Task<HomeTypeDto?> GetOrganisationHomeTypeById(
        string applicationId,
        string homeTypeId,
        Guid organisationId,
        CancellationToken cancellationToken)
    {
        var request = new invln_getsinglehometypeRequest
        {
            invln_userid = string.Empty,
            invln_organisationid = organisationId.ToString(),
            invln_applicationid = applicationId,
            invln_hometypeid = homeTypeId,
            invln_fieldstoretrieve = HomeTypeCrmFields,
        };

        return await GetSingle(request, cancellationToken);
    }

    public async Task<HomeTypeDto?> GetUserHomeTypeById(
        string applicationId,
        string homeTypeId,
        Guid organisationId,
        CancellationToken cancellationToken)
    {
        var request = new invln_getsinglehometypeRequest
        {
            invln_userid = _userContext.UserGlobalId,
            invln_organisationid = organisationId.ToString(),
            invln_applicationid = applicationId,
            invln_hometypeid = homeTypeId,
            invln_fieldstoretrieve = HomeTypeCrmFields,
        };

        return await GetSingle(request, cancellationToken);
    }

    public async Task Remove(string applicationId, string homeTypeId, Guid organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_deletehometypeRequest
        {
            invln_userid = _userContext.UserGlobalId,
            invln_organisationid = organisationId.ToString(),
            invln_applicationid = applicationId,
            invln_hometypeid = homeTypeId,
        };

        await _service.ExecuteAsync<invln_deletehometypeRequest, invln_deletehometypeResponse>(
            request,
            x => x.ResponseName,
            cancellationToken);
    }

    public async Task<string> Save(HomeTypeDto homeType, Guid organisationId, CancellationToken cancellationToken)
    {
        var request = new invln_sethometypeRequest
        {
            invln_organisationid = organisationId.ToString(),
            invln_userid = _userContext.UserGlobalId,
            invln_applicationid = homeType.applicationId,
            invln_hometype = JsonSerializer.Serialize(homeType, _serializerOptions),
            invln_fieldstoset = HomeTypeCrmFields,
        };

        return await _service.ExecuteAsync<invln_sethometypeRequest, invln_sethometypeResponse>(
            request,
            x => x.invln_hometypeid,
            cancellationToken);
    }

    private async Task<HomeTypeDto?> GetSingle(invln_getsinglehometypeRequest request, CancellationToken cancellationToken)
    {
        return await _service.ExecuteAsync<invln_getsinglehometypeRequest, invln_getsinglehometypeResponse, HomeTypeDto?>(
            request,
            x => x.invln_hometype,
            cancellationToken);
    }

    private async Task<IList<HomeTypeDto>> GetAll(invln_gettypeofhomeslistRequest request, CancellationToken cancellationToken)
    {
        return await _service.ExecuteAsync<invln_gettypeofhomeslistRequest, invln_gettypeofhomeslistResponse, IList<HomeTypeDto>>(
            request,
            x => x.invln_hometypeslist,
            cancellationToken);
    }
}
