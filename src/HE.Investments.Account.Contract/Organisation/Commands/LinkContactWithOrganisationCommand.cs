using MediatR;

namespace HE.Investments.Account.Contract.Organisation.Commands;

public record LinkContactWithOrganisationCommand(string CompaniesHouseNumber) : IRequest;
