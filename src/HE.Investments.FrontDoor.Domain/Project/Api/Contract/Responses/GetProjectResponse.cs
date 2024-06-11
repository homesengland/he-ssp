namespace HE.Investments.FrontDoor.Domain.Project.Api.Contract.Responses;

internal sealed record GetProjectResponse
{
    public string ProjectId { get; init; }

    public string? LocalAuthorityName { get; init; }

    public string? LocalAuthorityCode { get; init; }

    public DateTimeOffset? CreatedOn { get; init; }

    public FrontDoorProjectContact FrontDoorProjectContact { get; init; }

    public Guid ProjectRecordId { get; init; }

    public Guid OrganisationId { get; init; }

    public Guid PortalOwnerId { get; init; }

    public string ProjectName { get; init; }

    public int? ProjectType { get; init; }

    public bool ProjectSupportsHousingDeliveryInEngland { get; init; }

    public int[]? ActivitiesInThisProject { get; init; }

    public int[]? InfrastructureDelivered { get; init; }

    public int? AmountOfAffordableHomes { get; init; }

    public int? PreviousResidentialBuildingExperience { get; init; }

    public bool? IdentifiedSite { get; init; }

    public int? GeographicFocus { get; init; }

    public int[]? Region { get; init; }

    public string? LocalAuthority { get; init; }

    public int? NumberOfHomesEnabledBuilt { get; init; }

    public bool? WouldYourProjectFailWithoutHeSupport { get; init; }

    public bool? FundingRequired { get; init; }

    public int? AmountOfFundingRequired { get; init; }

    public bool? IntentionToMakeAProfit { get; init; }

    public int? StartOfProjectMonth { get; init; }

    public int? StartOfProjectYear { get; init; }

    public int[]? ProposedInterventions { get; init; }

    public int? LeadDirectorate { get; init; }
}
