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

        if (!result.Entities.Any())
        {
            await CreateTest();
        }

        result = await _service.RetrieveMultipleAsync(query, cancellationToken);

        // TODO: 88198 fetch programme in valid way
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

    private async Task CreateTest()
    {
        var programme = new Entity(ProgrammeFields.EntityName)
        {
            [ProgrammeFields.Name] = "AHP test",
            [ProgrammeFields.StartDate] = new DateTime(2021, 1, 1),
            [ProgrammeFields.EndDate] = new DateTime(2026, 1, 1),
        };

        await _service.CreateAsync(programme);
    }

    private string TryGetStringValue(Entity entity, string fieldName)
    {
        return entity.Contains(fieldName) ? entity[fieldName].ToString()! : string.Empty;
    }

    private DateTime? TryGetDateTimeValue(Entity entity, string fieldName)
    {
        if (DateTime.TryParse(TryGetStringValue(entity, fieldName), out var date))
        {
            return date;
        }

        return null;
    }
}
