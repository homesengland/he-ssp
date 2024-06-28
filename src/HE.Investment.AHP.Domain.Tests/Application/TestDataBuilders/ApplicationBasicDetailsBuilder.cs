using HE.Investment.AHP.Contract.Application;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Tests.TestData;

namespace HE.Investment.AHP.Domain.Tests.Application.TestDataBuilders;

public class ApplicationBasicDetailsBuilder
{
    private AhpApplicationId _id = new(GuidTestData.GuidOne.ToString());
    private string _name = "Test Application";
    private ApplicationStatus _status = ApplicationStatus.Draft;
    private Tenure? _tenure = Tenure.AffordableRent;
    private decimal? _grant = 1000000m;
    private int? _unit = 100;

    public static ApplicationBasicDetailsBuilder New() => new();

    public ApplicationBasicDetailsBuilder WithId(AhpApplicationId id)
    {
        _id = id;
        return this;
    }

    public ApplicationBasicDetailsBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public ApplicationBasicDetailsBuilder WithStatus(ApplicationStatus status)
    {
        _status = status;
        return this;
    }

    public ApplicationBasicDetailsBuilder WithTenure(Tenure? tenure)
    {
        _tenure = tenure;
        return this;
    }

    public ApplicationBasicDetailsBuilder WithGrant(decimal? grant)
    {
        _grant = grant;
        return this;
    }

    public ApplicationBasicDetailsBuilder WithUnit(int? unit)
    {
        _unit = unit;
        return this;
    }

    public ApplicationBasicDetails Build()
    {
        return new ApplicationBasicDetails(_id, _name, _status, _tenure, _grant, _unit);
    }
}
