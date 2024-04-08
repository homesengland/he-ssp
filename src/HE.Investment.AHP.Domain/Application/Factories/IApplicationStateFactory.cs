using HE.Investment.AHP.Domain.Application.Entities;
using HE.Investments.Common.Contract;

namespace HE.Investment.AHP.Domain.Application.Factories;

public interface IApplicationStateFactory
{
    ApplicationState Create(ApplicationStatus status);
}
