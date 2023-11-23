namespace HE.Investments.Common.Domain;

public sealed class ModificationTracker
{
    public bool IsModified { get; private set; }

    public void MarkAsModified() => IsModified = true;

    public T Change<T>(T currentValue, T newValue)
    {
        if (!EqualityComparer<T>.Default.Equals(currentValue, newValue))
        {
            MarkAsModified();
            return newValue;
        }

        return currentValue;
    }
}
