namespace HE.Investment.AHP.Domain.Common.FilePolicies;

public interface IFilePolicy<T>
{
    void Apply(T value);
}
