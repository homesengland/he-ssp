namespace HE.Investment.AHP.Contract.Application.Queries;

public record GetApplicationsQueryResult(string OrganisationName, IList<ApplicationBasicDetails> Applications);
