using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Application.Extensions;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;

public class CheckAnswersCompanyStructureSectionCommandHandler : CompanyStructureBaseCommandHandler, IRequestHandler<CheckAnswersCompanyStructureSectionCommand, OperationResult>
{
    public CheckAnswersCompanyStructureSectionCommandHandler(ICompanyStructureRepository repository, ILoanUserContext loanUserContext)
        : base(repository, loanUserContext)
    {
    }

    public async Task<OperationResult> Handle(CheckAnswersCompanyStructureSectionCommand request, CancellationToken cancellationToken)
    {
        return await Perform(x => x.CheckAnswers(request.YesNoAnswer.ToYesNoAnswer()), request.LoanApplicationId, cancellationToken);
    }
}
