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

    public T Change<T>(T currentValue, T newValue, Action? onChanged = null)
    {
        if (!EqualityComparer<T>.Default.Equals(currentValue, newValue))
        {
            MarkAsModified();
            onChanged?.Invoke();
            return newValue;
        }

        return currentValue;
    }
}
