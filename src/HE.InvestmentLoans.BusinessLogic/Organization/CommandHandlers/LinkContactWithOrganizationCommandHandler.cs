using HE.InvestmentLoans.BusinessLogic.User;
using HE.InvestmentLoans.BusinessLogic.User.Repositories;
using HE.InvestmentLoans.Contract.Exceptions;
using HE.InvestmentLoans.Contract.Organization;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Organization.CommandHandlers;
internal class LinkContactWithOrganizationCommandHandler : IRequestHandler<LinkContactWithOrganizationCommand>
{
    private readonly ILoanUserContext _loanUserContext;
    private readonly ILoanUserRepository _repository;

    public LinkContactWithOrganizationCommandHandler(ILoanUserContext loanUserContext, ILoanUserRepository repository)
    {
        _loanUserContext = loanUserContext;
        _repository = repository;
    }

    public async Task Handle(LinkContactWithOrganizationCommand request, CancellationToken cancellationToken)
    {
        if (await _loanUserContext.IsLinkedWithOrganization())
        {
            throw new DomainException($"Cannot link organization id: {request.Number} to loan user account id: {_loanUserContext.UserGlobalId}, becouse it is already linked to other organization", string.Empty);
        }

        await _repository.LinkContactToOrganisation(_loanUserContext.UserGlobalId, request.Number);
    }
}
