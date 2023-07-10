using HE.InvestmentLoans.BusinessLogic.ViewModel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.BusinessLogic.Project.QueryHandlers
{
    public record GetProjectDetailsQuery : IRequest<ProjectEntity>;
}
