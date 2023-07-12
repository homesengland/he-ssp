using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using Microsoft.AspNetCore.Http;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.BusinessLogic._LoanApplication.Extensions;
using HE.InvestmentLoans.Common.Utils;

namespace HE.InvestmentLoans.BusinessLogic._LoanApplication.Commands;

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
        private readonly IDateTimeProvider _dateTime;

        public Handler(IHttpContextAccessor httpContextAccessor, IDateTimeProvider dateTime)
        {
            _httpContextAccessor = httpContextAccessor;
            _dateTime = dateTime;
        }

        public async Task<LoanApplicationViewModel> Handle(Update request, CancellationToken cancellationToken)
        {
            if (request.Model == null)
            {
                throw new NotFoundException(nameof(LoanApplicationViewModel), "");
            }

            request.Model.Timestamp = _dateTime.Now;

            if (request.TryUpdateModelAction != null)
            {
                _ = await request.TryUpdateModelAction(request.Model);
            }


            _httpContextAccessor.HttpContext?.Session.Set(request.Model.ID.ToString(), request.Model);

            return request.Model;
        }
    }
}
