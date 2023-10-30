using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;
using HE.InvestmentLoans.Contract.Application.Enums;

namespace HE.Investment.AHP.Domain.FinancialDetails.Entities;

public class FinancialDetailsEntity
{
    public FinancialDetailsEntity(FinancialSchemeId financialSchemeId)
    {
        Id = new FinancialDetailsId(Guid.NewGuid());
        FinancialSchemeId = financialSchemeId;
    }

    public FinancialSchemeId FinancialSchemeId { get; }

    public FinancialDetailsId Id { get; }

    public PurchasePrice PurchasePrice { get; set; }

    public bool IsSchemaOnPublicLand { get; set; }

    public void ProvidePurchasePrice(PurchasePrice price)
    {
        PurchasePrice = price;
    }

    public void ProvideSchemaLandPublicity(string schemaLandPublicity)
    {
        IsSchemaOnPublicLand = schemaLandPublicity == YesNoAnswers.Yes.ToString();
    }
}
