using MediatR;

namespace HE.Investments.Account.Contract.Organisation.Commands;

public record LinkContactWithOrganizationCommand(string CompaniesHouseNumber) : IRequest;
