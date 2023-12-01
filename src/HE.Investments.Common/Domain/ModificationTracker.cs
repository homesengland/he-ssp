namespace HE.Investments.Common.Domain;

public sealed class ModificationTracker
{
    public bool IsModified { get; private set; }

    public void MarkAsModified() => IsModified = true;

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
