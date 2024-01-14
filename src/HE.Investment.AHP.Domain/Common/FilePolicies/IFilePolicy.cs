namespace HE.Investment.AHP.Domain.Common.FilePolicies;

public interface IFilePolicy<in T>
{
    void Apply(T value);
}
