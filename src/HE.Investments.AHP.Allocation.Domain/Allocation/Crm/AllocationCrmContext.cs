using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.CRM.Services;
using HE.Investments.Common.Extensions;

namespace HE.Investments.AHP.Allocation.Domain.Allocation.Crm;

public class AllocationCrmContext : IAllocationCrmContext
{
    private static readonly string ApplicationCrmFields =
        string.Join(
                ",",
                nameof(invln_scheme.invln_schemename),
                nameof(invln_scheme.invln_Tenure),
                nameof(invln_scheme.invln_applicationid))
            .ToLowerInvariant();

    private readonly ICrmService _service;

    public AllocationCrmContext(ICrmService service)
    {
        _service = service;
    }

    public async Task<AllocationClaimsDto> GetById(string id, string organisationId, string userId, CancellationToken cancellationToken)
    {
        // todo AB#102108
        // var request = new invln_getallocationphaseclaimsRequest
        //     invln_userid = userId,
        //     invln_organisationid = organisationId.TryToGuidAsString(),
        //     invln_applicationid = id.ToGuidAsString(),
        var request = new invln_getahpapplicationRequest
        {
            invln_userid = userId,
            invln_organisationid = organisationId.TryToGuidAsString(),
            invln_applicationid = id.ToGuidAsString(),
            invln_appfieldstoretrieve = ApplicationCrmFields,
        };

        return await Get(request, cancellationToken);
    }

    private async Task<AllocationClaimsDto> Get(invln_getahpapplicationRequest request, CancellationToken cancellationToken)
    {
        var response = await _service.ExecuteAsync<invln_getahpapplicationRequest, invln_getahpapplicationResponse, IList<AhpApplicationDto>>(
            request,
            r => r.invln_retrievedapplicationfields,
            cancellationToken);

        if (!response.Any())
        {
            throw new NotFoundException("AhpApplication", request.invln_applicationid);
        }

        return GetMockedAllocation(response[0]);
    }

    private AllocationClaimsDto GetMockedAllocation(AhpApplicationDto applicationDto)
    {
        return new AllocationClaimsDto()
        {
            Id = applicationDto.id,
            Name = applicationDto.name,
            ReferenceNumber = "G00001",
            GrantDetails = new GrantDetailsDto()
            {
                TotalGrantAllocated = 150000m,
                AmountPaid = 50000m,
                AmountRemaining = 100000m,
            },
            LocalAuthority = new LocalAuthorityDto() { code = "000003", name = "Reading" },
            Tenure = applicationDto.tenure!.Value,
            ProgrammeId = "d5fe3baa-eeae-ee11-a569-0022480041cf",
            ListOfPhaseClaims =
            [
                new()
                {
                    Id = "d5fe3baa-eeae-ee11-a569-0022480041cf",
                    AllocationId = applicationDto.id,
                    Name = "Phase 1",
                    NumberOfHomes = 100,
                    BuildActivityType = 1,
                    AcquisitionMilestone = new MilestoneClaimDto()
                    {
                        Type = 1,
                        Status = 1,
                        AmountOfGrantApportioned = 10000m,
                        PercentageOfGrantApportioned = 40,
                        ForecastClaimDate = DateTime.Today.AddDays(-7),
                    },
                    StartOnSiteMilestone = new MilestoneClaimDto()
                    {
                        Type = 2,
                        Status = 1,
                        AmountOfGrantApportioned = 20000m,
                        PercentageOfGrantApportioned = 40,
                        ForecastClaimDate = DateTime.Today.AddDays(-6),
                    },
                    CompletionMilestone = new MilestoneClaimDto()
                    {
                        Type = 3,
                        Status = 1,
                        AmountOfGrantApportioned = 30000m,
                        PercentageOfGrantApportioned = 20,
                        ForecastClaimDate = DateTime.Today.AddDays(7),
                    },
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    AllocationId = applicationDto.id,
                    Name = "Phase 2",
                    NumberOfHomes = 200,
                    BuildActivityType = 2,
                    AcquisitionMilestone = new MilestoneClaimDto()
                    {
                        Type = 1,
                        Status = 1,
                        AmountOfGrantApportioned = 20000m,
                        PercentageOfGrantApportioned = 20,
                        ForecastClaimDate = DateTime.Today,
                    },
                    StartOnSiteMilestone = new MilestoneClaimDto()
                    {
                        Type = 2,
                        Status = 1,
                        AmountOfGrantApportioned = 30000m,
                        PercentageOfGrantApportioned = 30,
                        ForecastClaimDate = DateTime.Today.AddDays(10),
                    },
                    CompletionMilestone = new MilestoneClaimDto()
                    {
                        Type = 3,
                        Status = 1,
                        AmountOfGrantApportioned = 40000m,
                        PercentageOfGrantApportioned = 50,
                        ForecastClaimDate = DateTime.Today.AddDays(40),
                    },
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    AllocationId = applicationDto.id,
                    Name = "Phase 3",
                    NumberOfHomes = 200,
                    BuildActivityType = 2,
                    AcquisitionMilestone = new MilestoneClaimDto()
                    {
                        Type = 1,
                        Status = 1,
                        AmountOfGrantApportioned = 20000m,
                        PercentageOfGrantApportioned = 20,
                        ForecastClaimDate = DateTime.Today,
                        AchievementDate = DateTime.Today,
                    },
                    StartOnSiteMilestone = new MilestoneClaimDto()
                    {
                        Type = 2,
                        Status = 1,
                        AmountOfGrantApportioned = 30000m,
                        PercentageOfGrantApportioned = 30,
                        ForecastClaimDate = DateTime.Today.AddDays(10),
                        AchievementDate = DateTime.Today.AddDays(10),
                    },
                    CompletionMilestone = new MilestoneClaimDto()
                    {
                        Type = 3,
                        Status = 1,
                        AmountOfGrantApportioned = 40000m,
                        PercentageOfGrantApportioned = 50,
                        ForecastClaimDate = DateTime.Today.AddDays(40),
                        AchievementDate = DateTime.Today.AddDays(20),
                    },
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    AllocationId = applicationDto.id,
                    Name = "Phase 4",
                    NumberOfHomes = 200,
                    BuildActivityType = 2,
                    AcquisitionMilestone = new MilestoneClaimDto()
                    {
                        Type = 1,
                        Status = 1,
                        AmountOfGrantApportioned = 20000m,
                        PercentageOfGrantApportioned = 20,
                        ForecastClaimDate = DateTime.Today,
                    },
                    StartOnSiteMilestone = new MilestoneClaimDto()
                    {
                        Type = 2,
                        Status = 1,
                        AmountOfGrantApportioned = 30000m,
                        PercentageOfGrantApportioned = 30,
                        ForecastClaimDate = DateTime.Today.AddDays(10),
                    },
                    CompletionMilestone = new MilestoneClaimDto()
                    {
                        Type = 3,
                        Status = 1,
                        AmountOfGrantApportioned = 40000m,
                        PercentageOfGrantApportioned = 50,
                        ForecastClaimDate = DateTime.Today.AddDays(40),
                    },
                },
            ],
        };
    }
}
