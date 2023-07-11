using HE.InvestmentLoans.BusinessLogic.ViewModel;
using MediatR;
using System;

namespace HE.InvestmentLoans.BusinessLogic.Application.Project.CommandHandlers;

public record DeleteProjectCommand(Guid LoanApplicationId, Guid ProjectId) : IRequest<LoanApplicationViewModel>;
