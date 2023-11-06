using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.InvestmentLoans.Common.Validation;
using MediatR;

namespace HE.Investment.AHP.Contract.FinancialDetails.Commands;
public record ProvidePurchasePriceCommand(FinancialDetailsId FinancialDetailsId, string PurchasePrice) : IRequest<OperationResult>;
