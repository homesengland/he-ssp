using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.Investments.Common.Validators;
using MediatR;

namespace HE.Investment.AHP.Contract.FinancialDetails.Commands;
public record ProvidePurchasePriceCommand(FinancialDetailsId FinancialDetailsId, string PurchasePrice) : IRequest<OperationResult>;
