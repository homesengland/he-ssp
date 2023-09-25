using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using MediatR;
using Microsoft.Extensions.Hosting;

namespace HE.InvestmentLoans.Contract.Projects.Commands;
public record class ChangeProjectNameCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, string Name) : IRequest<OperationResult>;
