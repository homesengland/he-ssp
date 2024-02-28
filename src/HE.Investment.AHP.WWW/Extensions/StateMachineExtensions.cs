using Stateless;

namespace HE.Investment.AHP.WWW.Extensions;

public static class StateMachineExtensions
{
    public static StateMachine<TState, TTrigger>.StateConfiguration PermitSection<TState, TTrigger>(
        this StateMachine<TState, TTrigger>.StateConfiguration configuration,
        Action<StateMachine<TState, TTrigger>.StateConfiguration> machineConfiguration,
        Func<bool> guard)
    {
        if (guard())
        {
            machineConfiguration(configuration);
        }

        return configuration;
    }
}
