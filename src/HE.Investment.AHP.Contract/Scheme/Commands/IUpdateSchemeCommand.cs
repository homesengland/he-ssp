using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.Scheme.Commands;

public interface IUpdateSchemeCommand
{
    public AhpApplicationId ApplicationId { get; }
}
