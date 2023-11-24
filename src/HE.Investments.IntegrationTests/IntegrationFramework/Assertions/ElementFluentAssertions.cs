using AngleSharp.Dom;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace HE.Investments.Loans.IntegrationTests.IntegrationFramework.Assertions;

public class ElementFluentAssertions : ReferenceTypeAssertions<IElement?, ElementFluentAssertions>
{
    public ElementFluentAssertions(IElement? subject)
        : base(subject)
    {
    }

    protected override string Identifier => "Element";

    public AndConstraint<ElementFluentAssertions> BeGdsButton()
    {
        Execute.Assertion.Given(() => Subject)
            .ForCondition(x => x is not null && x.ClassName is not null && x.ClassName.Equals("govuk-button", StringComparison.OrdinalIgnoreCase))
            .FailWith("Html element {0} is not GDS Button", y => y!.LocalName);

        return new AndConstraint<ElementFluentAssertions>(this);
    }
}
