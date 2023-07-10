using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Common.Authorization;
using HE.InvestmentLoans.Contract.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.BusinessLogic.Project.QueryHandlers
{
    public class GetProjectDetailsQueryHandler : IRequestHandler<GetProjectDetailsQuery, ProjectEntity>
    {
        private readonly IProjectContext projectContext;

        public GetProjectDetailsQueryHandler(ILoanUserContext loanUserContext)
        {
            _loanUserContext = loanUserContext;
        }

        public async Task<GetUserDetailsResponse> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
        {
            return new GetUserDetailsResponse(
                                _loanUserContext.Email,
                                _loanUserContext.UserGlobalId,
                                await _loanUserContext.GetSelectedAccountId(),
                                await _loanUserContext.GetAllAccountIds());
        }
        public Task<ProjectEntity> Handle(GetProjectDetailsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

}
