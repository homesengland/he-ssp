using HE.InvestmentLoans.BusinessLogic.Security;
using HE.InvestmentLoans.BusinessLogic.Tests.TestData;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Contract.Application.Enums;
using HE.InvestmentLoans.Contract.Security.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Security.TestObjectBuilder;

internal sealed class SecurityEntityTestBuilder
{
    private readonly SecurityEntity _entity;

    private SecurityEntityTestBuilder(SecurityEntity entity)
    {
        _entity = entity;
    }

    public static SecurityEntityTestBuilder New() => new(new SecurityEntity(LoanApplicationIdTestData.LoanApplicationIdOne,
        null!,
        null!,
        null!,
        SectionStatus.NotStarted,
        ApplicationStatus.Draft));

    public SecurityEntityTestBuilder WithDebenture(Debenture debenture)
    {
        _entity.ProvideDebenture(debenture);

        return this;
    }

    public SecurityEntityTestBuilder WithDirectorLoans(DirectorLoans directLoans)
    {
        _entity.ProvideDirectorLoans(directLoans);

        return this;
    }

    public SecurityEntityTestBuilder WithDirectorLoansSubordinate(DirectorLoansSubordinate directorLoansSubordinate)
    {
        _entity.ProvideDirectorLoansSubordinate(directorLoansSubordinate);

        return this;
    }

    public SecurityEntityTestBuilder ThatIsCompleted()
    {
        if (_entity.Debenture.IsNotProvided())
        {
            _entity.ProvideDebenture(new Debenture("holder", true));
        }

        if (_entity.DirectorLoans.IsNotProvided())
        {
            _entity.ProvideDirectorLoans(new DirectorLoans(false));
        }

        if (_entity.DirectorLoans.Exists && _entity.DirectorLoansSubordinate.IsNotProvided())
        {
            _entity.ProvideDirectorLoansSubordinate(new DirectorLoansSubordinate(true, string.Empty));
        }

        _entity.CheckAnswers(YesNoAnswers.Yes);

        return this;
    }

    public SecurityEntity Build()
    {
        return _entity;
    }
}
