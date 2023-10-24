using HE.Investment.AHP.Contract.FinancialDetails.ValueObjects;

namespace HE.Investment.AHP.BusinessLogic.FinancialDetails.Entities
{
    public class FinancialDetailsEntity
    {
        public FinancialDetailsEntity()
        {
            Id = new FinancialDetailsId(Guid.NewGuid());
        }

        public FinancialDetailsId Id { get; }

    }
}
