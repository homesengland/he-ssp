using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investments.Account.Contract.Cache.Commands;

public class ClearUserCacheCommand : IRequest<OperationResult>
{
}
