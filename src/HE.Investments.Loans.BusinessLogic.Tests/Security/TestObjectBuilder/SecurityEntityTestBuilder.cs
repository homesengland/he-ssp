using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Loans.BusinessLogic.Security;
using HE.Investments.Loans.BusinessLogic.Tests.TestData;
using HE.Investments.Loans.Contract.Application.Enums;
using HE.Investments.Loans.Contract.Security.ValueObjects;

namespace HE.Investments.Loans.BusinessLogic.Tests.Security.TestObjectBuilder;

internal sealed class SecurityEntityTestBuilder
{
    private readonly SecurityEntity _entity;

    private SecurityEntityTestBuilder(SecurityEntity entity)
    {
        _entity = entity;
    }

    public static SecurityEntityTestBuilder New() => new(new SecurityEntity(
        LoanApplicationIdTestData.LoanApplicationIdOne,
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
