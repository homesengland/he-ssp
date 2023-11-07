using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Contract.Application.Extensions;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using HE.Investments.Common.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;

public class CheckAnswersCompanyStructureSectionCommandHandler : CompanyStructureBaseCommandHandler, IRequestHandler<CheckAnswersCompanyStructureSectionCommand, OperationResult>
{
    public CheckAnswersCompanyStructureSectionCommandHandler(ICompanyStructureRepository repository, ILoanUserContext loanUserContext, ILogger<CompanyStructureBaseCommandHandler> logger)
        : base(repository, loanUserContext, logger)
    {
    }

    public async Task<OperationResult> Handle(CheckAnswersCompanyStructureSectionCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            companyStructure =>
            {
                companyStructure.CheckAnswers(request.YesNoAnswer.ToYesNoAnswer());

                return Task.CompletedTask;
            },
            request.LoanApplicationId,
            cancellationToken);
    }
}
