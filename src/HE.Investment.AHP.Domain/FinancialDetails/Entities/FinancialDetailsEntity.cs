using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;

namespace HE.Investment.AHP.Domain.FinancialDetails.Entities
{
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

        public void ProvidePurchasePrice(PurchasePrice price)
        {
            PurchasePrice = price;
        }

    }
}
