using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Security.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Security;
public class SecurityEntity
{
    public SecurityEntity(LoanApplicationId applicationId, Debenture debenture, DirectLoans directLoans)
    {
        LoanApplicationId = applicationId;
        Debenture = debenture;
        DirectLoans = directLoans;
    }

    public LoanApplicationId LoanApplicationId { get; private set; }

    public Debenture Debenture { get; private set; }

    public DirectLoans DirectLoans { get; private set; }

    public void ProvideDebenture(Debenture debenture) => Debenture = debenture;

    public void ProvideDirectLoans(DirectLoans directLoans) => DirectLoans = directLoans;

    internal void Confirm()
    {
        if (Debenture is null)
        {
            throw new Exception();
        }

        if (DirectLoans is null)
        {
            throw new Exception();
        }

        // Change status: where is it?
    }
}
