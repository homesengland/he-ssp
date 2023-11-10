using HE.Investments.Common.Validators;
using MediatR;
using ApplicationId = HE.Investment.AHP.Domain.FinancialDetails.ValueObjects.ApplicationId;

namespace HE.Investment.AHP.Domain.FinancialDetails.Commands;
public record ProvidePurchasePriceCommand(ApplicationId ApplicationId, string PurchasePrice, bool IsPurchasePriceKnown) : IRequest<OperationResult>;
