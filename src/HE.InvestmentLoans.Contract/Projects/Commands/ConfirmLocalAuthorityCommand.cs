using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.InvestmentLoans.Contract.Projects.Commands;
public record ConfirmLocalAuthorityCommand(LoanApplicationId LoanApplicationId, ProjectId ProjectId, LocalAuthorityId LocalAuthorityId) : IRequest<OperationResult>;
