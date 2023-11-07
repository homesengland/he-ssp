using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.HomeTypes.Commands;

public abstract record SaveHomeTypeSegmentCommandBase(string ApplicationId, string HomeTypeId) : IRequest<OperationResult>;
