using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.Account.Contract.Organisation.Commands;

public record LinkContactWithOrganisationCommand(string CompaniesHouseNumber, bool? IsConfirmed) : IRequest<OperationResult>;
