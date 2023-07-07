namespace HE.InvestmentLoans.BusinessLogic._LoanApplication.Commands
{
    using MediatR;
    using System.Threading;
    using System.Threading.Tasks;
    using System;
    using HE.InvestmentLoans.BusinessLogic.ViewModel;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using HE.InvestmentLoans.BusinessLogic.Exceptions;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.Linq;
    using HE.InvestmentLoans.Common.Exceptions;
    using HE.InvestmentLoans.BusinessLogic._LoanApplication.Extensions;

    public class Update : IRequest<LoanApplicationViewModel>
    {
        public LoanApplicationViewModel Model
        {
            get;
            set;
        }

        public Func<LoanApplicationViewModel, Task<bool>> TryUpdateModelAction
        {
            get;
            set;
        }

        public class Handler : IRequestHandler<Update, LoanApplicationViewModel>
        {
           
          
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler( IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }

            public async Task<LoanApplicationViewModel> Handle(Update request, CancellationToken cancellationToken)
            {
                if (request.Model == null)
                {
                    throw new NotFoundException(nameof(LoanApplicationViewModel), "");
                }

                request.Model.Timestamp = DateTime.Now;

                if (request.TryUpdateModelAction != null)
                {
                    _ = await request.TryUpdateModelAction(request.Model);
                }


                _httpContextAccessor.HttpContext?.Session.Set<LoanApplicationViewModel>(request.Model.ID.ToString(), request.Model);

                return request.Model;
            }
        }
    }
}
