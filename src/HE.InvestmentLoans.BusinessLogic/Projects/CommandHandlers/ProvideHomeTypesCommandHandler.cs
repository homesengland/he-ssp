using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.Repositories;
using HE.InvestmentLoans.BusinessLogic.Projects.ValueObjects;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Projects.Commands;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Projects.CommandHandlers;
public class ProvideHomeTypesCommandHandler : ProjectCommandHandlerBase, IRequestHandler<ProvideHomeTypesCommand, OperationResult>
{
    public ProvideHomeTypesCommandHandler(IApplicationProjectsRepository repository, ILoanUserContext loanUserContext)
        : base(repository, loanUserContext)
    {
    }

    public async Task<OperationResult> Handle(ProvideHomeTypesCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            project =>
            {
                if (request.HomeTypes.Length == 0)
                {
                    return;
                }

                ICollection<HomeType> homeTypes = new List<HomeType>();
                foreach (var homeType in request.HomeTypes)
                {
                    homeTypes.Add(new HomeType(homeType));
                }

                project.ProvideHomeTypes(homeTypes);
            },
            request.LoanApplicationId,
            request.ProjectId,
            cancellationToken);
    }
}
