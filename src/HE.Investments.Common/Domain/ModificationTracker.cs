namespace HE.Investments.Common.Domain;

public delegate void EntityModifiedEventHandler();

public sealed class ModificationTracker
{
    private readonly EntityModifiedEventHandler? _modifiedEventHandler;

    public ModificationTracker(EntityModifiedEventHandler? modifiedEventHandler = null)
    {
        _modifiedEventHandler = modifiedEventHandler;
    }

    public bool IsModified { get; private set; }

    public void MarkAsModified()
    {
        IsModified = true;
        _modifiedEventHandler?.Invoke();
    }

    public T Change<T>(T currentValue, T newValue, Action? onChanged = null, params Action<T>[] onChangedWithParamList)
    {
        if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
        {
            return currentValue;
        }

        MarkAsModified();
        onChanged?.Invoke();
        foreach (var onChangedWithParam in onChangedWithParamList)
        {
            onChangedWithParam.Invoke(newValue);
        }

        return newValue;
    }
}
