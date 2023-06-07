using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using Microsoft.PowerPlatform.Dataverse.Client;
using HE.InvestmentLoans.CRM.Model;
using HE.InvestmentLoans.BusinessLogic.Exceptions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using HE.InvestmentLoans.BusinessLogic.Extensions;

namespace HE.InvestmentLoans.BusinessLogic._LoanApplication.Queries
{
    public class GetSingle : IRequest<LoanApplicationViewModel>
    {
        public Guid Id
        {
            get;
            set;
        }

        public class Handler : IRequestHandler<GetSingle, LoanApplicationViewModel>
        {
            private IHttpContextAccessor _httpContextAccessor;

            public Handler(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }


            public async Task<LoanApplicationViewModel> Handle(GetSingle request, CancellationToken cancellationToken)
            {
                var model = _httpContextAccessor.HttpContext?.Session.Get<LoanApplicationViewModel>(request.Id.ToString());

                if (model == null)
                {
                    throw new NotFoundException(nameof(LoanApplicationViewModel), request.Id);
                }

                return model;
            }
        }
    }
}
