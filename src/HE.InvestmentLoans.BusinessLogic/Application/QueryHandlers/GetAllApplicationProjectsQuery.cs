using HE.InvestmentLoans.BusinessLogic.Project;
using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.InvestmentLoans.Contract.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.BusinessLogic.Application.QueryHandlers
{
    public record GetAllApplicationProjectsQuery : IRequest<List<ProjectEntity>>;
}
