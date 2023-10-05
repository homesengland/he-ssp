using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.Projects.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Projects.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
public class ProvideHomeTypesCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideHomeTypesCommand, OperationResult>
{
    public ProvideHomeTypesCommandHandler(IApplicationProjectsRepository repository, ILoanUserContext loanUserContext, ILogger<ProjectCommandHandlerBase> logger)
        : base(repository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(ProvideHomeTypesCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            project =>
            {
                if (request.HomeTypes.IsNotProvided())
                {
                    return;
                }

                // TODO
                project.ProvideHomesTypes(new HomeTypes(request.HomeTypes), new OtherHomeType(request.OtherHomeType));
            },
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
