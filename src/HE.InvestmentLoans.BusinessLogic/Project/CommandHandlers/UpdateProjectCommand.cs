using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HE.InvestmentLoans.BusinessLogic.Project.CommandHandlers
{
    public record UpdateProjectCommand(ProjectEntity Model) : IRequest;
}
