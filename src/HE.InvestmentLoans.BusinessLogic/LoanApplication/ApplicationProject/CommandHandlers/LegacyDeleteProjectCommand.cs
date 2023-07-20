using HE.InvestmentLoans.BusinessLogic.ViewModel;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.LoanApplication.ApplicationProject.CommandHandlers;

public record LegacyDeleteProjectCommand(Guid LoanApplicationId, Guid ProjectId) : IRequest<LoanApplicationViewModel>;
