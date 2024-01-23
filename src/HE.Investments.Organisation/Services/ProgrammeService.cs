using System.Globalization;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Organisation.CrmFields;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace HE.Investments.Organisation.Services;

public class ProgrammeService : IProgrammeService
{
    private readonly IOrganizationServiceAsync2 _service;

    public ProgrammeService(IOrganizationServiceAsync2 service)
    {
        _service = service;
    }

    public async Task<ProgrammeDto?> Get(CancellationToken cancellationToken)
    {
        var query = new QueryExpression(ProgrammeFields.EntityName) { ColumnSet = ProgrammeFields.Columns, };

        var result = await _service.RetrieveMultipleAsync(query, cancellationToken);

        // TODO #88198 - no idea how to fetch valid Programme
        var programme = result.Entities.FirstOrDefault();
        if (programme == null)
        {
            return null;
        }

        return new ProgrammeDto
        {
            name = TryGetStringValue(programme, ProgrammeFields.Name),
            startOn = TryGetDateTimeValue(programme, ProgrammeFields.StartDate),
            endOn = TryGetDateTimeValue(programme, ProgrammeFields.EndDate),
        };
    }

    private string TryGetStringValue(Entity entity, string fieldName)
    {
        return entity.Contains(fieldName) ? entity[fieldName].ToString()! : string.Empty;
    }

    private DateTime? TryGetDateTimeValue(Entity entity, string fieldName)
    {
        if (DateTime.TryParse(TryGetStringValue(entity, fieldName), CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            return date;
        }

        return null;
    }
}
