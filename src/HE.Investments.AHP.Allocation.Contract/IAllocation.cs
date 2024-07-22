using HE.Investment.AHP.Contract.Application;

namespace HE.Investments.AHP.Allocation.Contract;

public interface IAllocation
{
    string ReferenceNumber { get; }

    string LocalAuthority { get; }

    string ProgrammeName { get; }

    Tenure Tenure { get; }
}
