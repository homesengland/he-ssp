namespace HE.Investment.AHP.Contract.HomeTypes;

public record ApplicationHomeTypes(string ApplicationName, IList<HomeTypeDetails> HomeTypes, bool IsReadOnly);
