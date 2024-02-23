using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Domain.Site.ValueObjects;
using HE.Investments.TestsUtils.TestFramework;

namespace HE.Investment.AHP.Domain.Tests.Site.TestDataBuilders;

public class NationalDesignGuidePrioritiesBuilder : TestObjectBuilder<NationalDesignGuidePrioritiesBuilder, NationalDesignGuidePriorities>
{
    private NationalDesignGuidePrioritiesBuilder()
        : base(new NationalDesignGuidePriorities())
    {
    }

    protected override NationalDesignGuidePrioritiesBuilder Builder => this;

    public static NationalDesignGuidePrioritiesBuilder New() => new();

    public NationalDesignGuidePrioritiesBuilder WithPriorities(params NationalDesignGuidePriority[] value) => SetProperty(x => x.Values, value);
}
