using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.Common.ValueObjects;

public class FileSize : ValueObject
{
    public FileSize(long bytes)
    {
        if (bytes < 0)
        {
            throw new ArgumentException("Bytes cannot be negative", nameof(bytes));
        }

        Bytes = bytes;
    }

    public long Bytes { get; }

    public int Megabytes => (int)(Bytes / 1024L / 1024L);

    public static bool operator >(FileSize left, FileSize right) => left.Bytes > right.Bytes;

    public static bool operator <(FileSize left, FileSize right) => left.Bytes < right.Bytes;

    public static FileSize FromMegabytes(int megabytes) => new(megabytes * 1024L * 1024L);

    public override string ToString()
    {
        return $"{Bytes} Bytes";
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Bytes;
    }
}
