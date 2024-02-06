namespace HE.Investments.Common.Domain;

public interface IQuestion
{
    bool IsAnswered();

    bool IsNotAnswered() => !IsAnswered();
}
