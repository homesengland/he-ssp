using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Commands;
public record DeleteProjectCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId) : IRequest<OperationResult>;
