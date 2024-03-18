using HE.Investments.Account.Shared.User;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.Shared.Project.Data;
using HE.Investments.FrontDoor.Shared.Project.Repositories;

namespace HE.Investments.Loans.IntegrationTests.MockedServices;

public class PrefillDataRepositoryMock : IPrefillDataRepository
{
    private static readonly IDictionary<FrontDoorProjectId, ProjectPrefillData> MockedData = new Dictionary<FrontDoorProjectId, ProjectPrefillData>();

    private readonly IPrefillDataRepository _decorated;

    public PrefillDataRepositoryMock(IPrefillDataRepository decorated)
    {
        _decorated = decorated;
    }

    public static void MockProjectData(ProjectPrefillData prefillData)
    {
        MockedData[prefillData.Id] = prefillData;
    }

    public async Task<ProjectPrefillData> GetProjectPrefillData(FrontDoorProjectId projectId, UserAccount userAccount, CancellationToken cancellationToken)
    {
        if (MockedData.TryGetValue(projectId, out var prefillData))
        {
            return prefillData;
        }

        return await _decorated.GetProjectPrefillData(projectId, userAccount, cancellationToken);
    }

    public async Task MarkProjectAsUsed(FrontDoorProjectId projectId, CancellationToken cancellationToken)
    {
        if (!MockedData.ContainsKey(projectId))
        {
            await _decorated.MarkProjectAsUsed(projectId, cancellationToken);
        }
    }
}
