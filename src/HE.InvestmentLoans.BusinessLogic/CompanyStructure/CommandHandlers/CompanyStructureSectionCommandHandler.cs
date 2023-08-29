using HE.InvestmentLoans.BusinessLogic.CompanyStructure.Repositories;
using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.CompanyStructure.Commands;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.CompanyStructure.CommandHandlers;

public class CompanyStructureSectionCommandHandler :
    IRequestHandler<CompanyStructureSectionCommand>,
    IRequestHandler<UnCompleteCompanyStructureSectionCommand>,
    IRequestHandler<ProvideCompanyPurposeCommand>,
    IRequestHandler<ProvideMoreInformationAboutOrganizationCommand>,
    IRequestHandler<ProvideHowManyHomesBuiltCommand, OperationResult>
{
    private readonly ICompanyStructureRepository _repository;

    private readonly ILoanUserContext _loanUserContext;

    public CompanyStructureSectionCommandHandler(ICompanyStructureRepository repository, ILoanUserContext loanUserContext)
    {
        _repository = repository;
        _loanUserContext = loanUserContext;
    }

    public async Task Handle(CompanyStructureSectionCommand request, CancellationToken cancellationToken)
    {
        await Perform(x => x.CompleteSection(), request.LoanApplicationId, cancellationToken);
    }

    public async Task Handle(UnCompleteCompanyStructureSectionCommand request, CancellationToken cancellationToken)
    {
        await Perform(x => x.UnCompleteSection(), request.LoanApplicationId, cancellationToken);
    }

    public async Task Handle(ProvideCompanyPurposeCommand request, CancellationToken cancellationToken)
    {
        await Perform(x => x.ProvideCompanyPurpose(request.CompanyPurpose), request.LoanApplicationId, cancellationToken);
    }

    public async Task Handle(ProvideMoreInformationAboutOrganizationCommand request, CancellationToken cancellationToken)
    {
        await Perform(
            x =>
            {
                x.ProvideMoreInformation(request.OrganisationMoreInformation);
                x.ProvideFileWithMoreInformation(request.OrganisationMoreInformationFile);
            },
            request.LoanApplicationId,
            cancellationToken);
    }

    public async Task<OperationResult> Handle(ProvideHowManyHomesBuiltCommand request, CancellationToken cancellationToken)
    {
        return await Perform(
            x =>
            {
                var homesBuild = request.HomesBuilt.IsProvided() ? HomesBuilt.FromString(request.HomesBuilt!) : null;
                x.ProvideHowManyHomesBuilt(homesBuild);
            },
            request.LoanApplicationId,
            cancellationToken);
    }

    private async Task<OperationResult> Perform(Action<CompanyStructureEntity> action, LoanApplicationId loanApplicationId, CancellationToken cancellationToken)
    {
        var userAccount = await _loanUserContext.GetSelectedAccount();
        var companyStructure = await _repository.GetAsync(loanApplicationId, userAccount, cancellationToken);

        try
        {
            action(companyStructure);
        }
        catch (DomainValidationException domainValidationException)
        {
            return domainValidationException.OperationResult;
        }

        await _repository.SaveAsync(companyStructure, userAccount, cancellationToken);
        return OperationResult.Success();
    }
}
