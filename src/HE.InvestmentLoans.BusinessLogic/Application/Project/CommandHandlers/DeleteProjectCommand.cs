using HE.InvestmentLoans.BusinessLogic.ViewModel;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Application.Project.CommandHandlers;

public record DeleteProjectCommand(Guid LoanApplicationId, Guid ProjectId) : IRequest<LoanApplicationViewModel>;
