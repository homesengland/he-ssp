using System.Globalization;

namespace HE.Investments.Common.Workflow;

public class EncodedWorkflow<TState>
    where TState : struct, Enum
{
    private readonly IReadOnlyCollection<TState> _availableStates;

    public EncodedWorkflow(Predicate<TState> canBeAccessed)
    {
        _availableStates = Enum.GetValues(typeof(TState)).Cast<TState>().Where(x => canBeAccessed(x)).OrderBy(x => x).ToList();
        Value = new string(_availableStates.Select(Encode).ToArray());
    }

    public EncodedWorkflow(string value)
    {
        Value = value;
        _availableStates = value.Select(Decode).ToList();
    }

    public string Value { get; }

    public TState GetNextChangedWorkflowState(TState currentState, EncodedWorkflow<TState> lastWorkflow)
    {
        var stateDifferences = _availableStates
            .Where(x => Convert.ToInt32(x, CultureInfo.InvariantCulture) > Convert.ToInt32(currentState, CultureInfo.InvariantCulture))
            .Except(lastWorkflow._availableStates)
            .ToList();
        if (stateDifferences.Any())
        {
            return stateDifferences[0];
        }

        return Enum.GetValues(typeof(TState)).Cast<TState>().Last();
    }

    public override string ToString()
    {
        return Value;
    }

    private static char Encode(TState state)
    {
        return EncodedWorkflowHelpers.WorkflowCodeChars[Convert.ToInt32(state, CultureInfo.InvariantCulture)];
    }

    private static TState Decode(char state)
    {
        var enumValue = Array.IndexOf(EncodedWorkflowHelpers.WorkflowCodeChars, state);
        if (enumValue < 0)
        {
            throw new InvalidOperationException($"Workflow cannot be decoded '{state}' is not valid.");
        }

        var value = Enum.Parse<TState>(enumValue.ToString(CultureInfo.InvariantCulture));
        if (!Enum.IsDefined(typeof(TState), value))
        {
            throw new InvalidOperationException($"Workflow cannot be decoded '{state}' is not valid.");
        }

        return value;
    }
}
