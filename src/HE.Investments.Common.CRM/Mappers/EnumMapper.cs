namespace HE.Investments.Common.CRM.Mappers;

public abstract class EnumMapper<TSource>
    where TSource : struct
{
    protected abstract IDictionary<TSource, int?> Mapping { get; }

    protected virtual TSource? ToDomainMissing => default;

    protected virtual int? ToDtoMissing => default;

    private string Name => typeof(TSource).Name;

    public int? ToDto(TSource? value)
    {
        if (value == null)
        {
            return ToDtoMissing;
        }

        if (!Mapping.TryGetValue(value.Value, out var result))
        {
            throw new ArgumentException($"Cannot find {Name} DTO mapping for {value}");
        }

        return result;
    }

    public TSource? ToDomain(int? value)
    {
        return DomainEnumMapper.Map(value, Mapping, ToDomainMissing);
    }
}
