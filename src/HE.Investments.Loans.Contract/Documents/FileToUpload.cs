namespace HE.Investments.Loans.Contract.Documents;

public record FileToUpload(string Name, long Length, Stream Content);
