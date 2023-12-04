using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Common.ValueObjects;
using HE.Investment.AHP.Domain.Documents.Config;
using HE.Investment.AHP.Domain.HomeTypes.Commands;
using HE.Investment.AHP.Domain.HomeTypes.Entities;
using HE.Investment.AHP.Domain.HomeTypes.Repositories;
using HE.Investments.Common.Validators;
using Microsoft.Extensions.Logging;

namespace HE.Investment.AHP.Domain.HomeTypes.CommandHandlers;

public class SaveDesignPlansCommandHandler : SaveHomeTypeSegmentCommandHandlerBase<SaveDesignPlansCommand>
{
    private readonly IAhpDocumentSettings _documentSettings;

    public SaveDesignPlansCommandHandler(
        IHomeTypeRepository homeTypeRepository,
        IAhpDocumentSettings documentSettings,
        ILogger<SaveDesignPlansCommandHandler> logger)
        : base(homeTypeRepository, logger)
    {
        _documentSettings = documentSettings;
    }

    protected override IReadOnlyCollection<HomeTypeSegmentType> SegmentTypes => new[] { HomeTypeSegmentType.DesignPlans };

    protected override IEnumerable<Action<SaveDesignPlansCommand, IHomeTypeEntity>> SaveActions => new[]
    {
        (request, homeType) => homeType.DesignPlans.ChangeMoreInformation(request.MoreInformation),
        SaveDesignPlanFiles,
    };

    private DesignPlanFileEntity CreateDesignFile(FileToUpload file)
    {
        return DesignPlanFileEntity.ForUpload(new FileName(file.Name), new FileSize(file.Lenght), file.Content, _documentSettings);
    }

    private void SaveDesignPlanFiles(SaveDesignPlansCommand request, IHomeTypeEntity homeType)
    {
        var designPlanFiles = new List<DesignPlanFileEntity>();
        new OperationResult(PerformWithValidation(request.Files.Select<FileToUpload, Action>(x => () => designPlanFiles.Add(CreateDesignFile(x))).ToArray()))
            .CheckErrors();

        homeType.DesignPlans.AddFilesToUpload(designPlanFiles);
    }
}
