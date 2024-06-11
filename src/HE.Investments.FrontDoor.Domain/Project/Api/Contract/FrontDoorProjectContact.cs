namespace HE.Investments.FrontDoor.Domain.Project.Api.Contract;

public record FrontDoorProjectContact
{
    public Guid AccountId { get; init; }

    public string ContactFirstName { get; init; }

    public string ContactLastName { get; init; }

    public string ContactEmail { get; init; }

    public string? ContactTelephoneNumber { get; init; }
}
