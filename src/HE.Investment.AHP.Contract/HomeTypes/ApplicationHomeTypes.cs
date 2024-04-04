using HE.Investment.AHP.Contract.Application;

namespace HE.Investment.AHP.Contract.HomeTypes;

public record ApplicationHomeTypes(ApplicationDetails Application, IList<HomeTypeDetails> HomeTypes);
