using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Tranches;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Extensions;
using DateValueObject = HE.Investments.Common.Domain.ValueObjects.DateValueObject;

namespace HE.Investment.AHP.Domain.Delivery.Crm;

public class DeliveryPhaseCrmMapper : IDeliveryPhaseCrmMapper
{
    public DeliveryPhaseEntity MapToDomain(
        ApplicationBasicInfo application,
        OrganisationBasicInfo organisation,
        DeliveryPhaseDto dto)
    {
        var typeOfHomes = MapTypeOfHomes(dto.typeOfHomes);
        var buildActivityType = MapBuildActivityType(dto.newBuildActivityType, dto.rehabBuildActivityType);
        var buildActivity = new BuildActivity(application.Tenure, typeOfHomes, buildActivityType);

        return new DeliveryPhaseEntity(
            application,
            new DeliveryPhaseName(dto.name),
            organisation,
            dto.isCompleted == true ? SectionStatus.Completed : SectionStatus.InProgress,
            new MilestonesPercentageTranches(
                dto.acquisitionPercentageValue != null ? new WholePercentage(dto.acquisitionPercentageValue.Value) : null,
                dto.startOnSitePercentageValue != null ? new WholePercentage(dto.startOnSitePercentageValue.Value) : null,
                dto.completionPercentageValue != null ? new WholePercentage(dto.completionPercentageValue.Value) : null),
            new MilestonesTranches(
                dto.sumOfCalculatedFounds,
                dto.acquisitionValue,
                dto.startOnSiteValue,
                dto.completionValue),
            MapStatusCode(dto.statusCode) == StatusCode.PendingAdjustment,
            typeOfHomes,
            buildActivity,
            dto.isReconfigurationOfExistingProperties,
            MapHomesToDeliver(dto.numberOfHomes),
            AcquisitionMilestoneDetails.Create(
                new AcquisitionDate(dto.acquisitionDate),
                new MilestonePaymentDate(dto.acquisitionPaymentDate)),
            StartOnSiteMilestoneDetails.Create(
                new StartOnSiteDate(dto.startOnSiteDate),
                new MilestonePaymentDate(dto.startOnSitePaymentDate)),
            CompletionMilestoneDetails.Create(
                new CompletionDate(dto.completionDate),
                new MilestonePaymentDate(dto.completionPaymentDate)),
            DeliveryPhaseId.From(dto.id),
            dto.createdOn,
            MapIsAdditionalPaymentRequested(dto.requiresAdditionalPayments),
            dto.claimingtheMilestoneConfirmed);
    }

    public DeliveryPhaseDto MapToDto(DeliveryPhaseEntity entity)
    {
        return new DeliveryPhaseDto
        {
            id = entity.Id.IsNew ? null : entity.Id.ToGuidAsString(),
            applicationId = entity.Application.Id.ToGuidAsString(),
            name = entity.Name.Value,
            isCompleted = entity.Status == SectionStatus.Completed,
            newBuildActivityType = MapNewBuildActivityType(entity.BuildActivity.Type),
            rehabBuildActivityType = MapRehabBuildActivityType(entity.BuildActivity.Type),
            isReconfigurationOfExistingProperties = entity.ReconfiguringExisting,
            acquisitionPercentageValue = entity.Tranches.Percentages.Acquisition?.Value,
            startOnSitePercentageValue = entity.Tranches.Percentages.StartOnSite?.Value,
            completionPercentageValue = entity.Tranches.Percentages.Completion?.Value,
            claimingtheMilestoneConfirmed = entity.Tranches.ClaimMilestone,
            acquisitionDate = MapMilestoneDate(entity.DeliveryPhaseMilestones.AcquisitionMilestone),
            acquisitionPaymentDate = MapPaymentDate(entity.DeliveryPhaseMilestones.AcquisitionMilestone),
            startOnSiteDate = MapMilestoneDate(entity.DeliveryPhaseMilestones.StartOnSiteMilestone),
            startOnSitePaymentDate = MapPaymentDate(entity.DeliveryPhaseMilestones.StartOnSiteMilestone),
            completionDate = MapMilestoneDate(entity.DeliveryPhaseMilestones.CompletionMilestone),
            completionPaymentDate = MapPaymentDate(entity.DeliveryPhaseMilestones.CompletionMilestone),
            typeOfHomes = MapTypeOfHomes(entity.TypeOfHomes),
            requiresAdditionalPayments = MapIsAdditionalPaymentRequested(entity.IsAdditionalPaymentRequested),
            numberOfHomes = MapHomesToDeliver(entity.HomesToDeliver.ToList()),
        };
    }

    private static TypeOfHomes? MapTypeOfHomes(string? value)
    {
        return value switch
        {
            "newBuild" => TypeOfHomes.NewBuild,
            "rehab" => TypeOfHomes.Rehab,
            _ => null,
        };
    }

    private static string? MapTypeOfHomes(TypeOfHomes? value)
    {
        return value switch
        {
            TypeOfHomes.NewBuild => "newBuild",
            TypeOfHomes.Rehab => "rehab",
            _ => null,
        };
    }

    private static BuildActivityType? MapBuildActivityType(int? newBuildActivityType, int? rehabBuildActivityType)
    {
        if (newBuildActivityType.HasValue)
        {
            return MapNewBuildActivityType(newBuildActivityType.Value);
        }

        if (rehabBuildActivityType.HasValue)
        {
            return MapRehabBuildActivityType(rehabBuildActivityType.Value);
        }

        return null;
    }

    private static BuildActivityType? MapNewBuildActivityType(int newBuildActivityType)
    {
        return newBuildActivityType switch
        {
            (int)invln_NewBuildActivityType.AcquisitionandWorks => BuildActivityType.AcquisitionAndWorks,
            (int)invln_NewBuildActivityType.LandInclusivePackagepackagedeal => BuildActivityType.LandInclusivePackage,
            (int)invln_NewBuildActivityType.OffTheShelf => BuildActivityType.OffTheShelf,
            (int)invln_NewBuildActivityType.WorksOnly => BuildActivityType.WorksOnly,
            (int)invln_NewBuildActivityType.Regeneration => BuildActivityType.Regeneration,
            _ => null,
        };
    }

    private static int? MapNewBuildActivityType(BuildActivityType? buildActivityType)
    {
        return buildActivityType switch
        {
            BuildActivityType.AcquisitionAndWorks => (int)invln_NewBuildActivityType.AcquisitionandWorks,
            BuildActivityType.LandInclusivePackage => (int)invln_NewBuildActivityType.LandInclusivePackagepackagedeal,
            BuildActivityType.OffTheShelf => (int)invln_NewBuildActivityType.OffTheShelf,
            BuildActivityType.WorksOnly => (int)invln_NewBuildActivityType.WorksOnly,
            BuildActivityType.Regeneration => (int)invln_NewBuildActivityType.Regeneration,
            _ => null,
        };
    }

    private static BuildActivityType? MapRehabBuildActivityType(int rehabBuildActivityType)
    {
        return rehabBuildActivityType switch
        {
            (int)invln_RehabActivityType.AcquisitionandWorksrehab => BuildActivityType.AcquisitionAndWorksRehab,
            (int)invln_RehabActivityType.Conversion => BuildActivityType.Conversion,
            (int)invln_RehabActivityType.ExistingSatisfactory => BuildActivityType.ExistingSatisfactory,
            (int)invln_RehabActivityType.LeaseandRepair => BuildActivityType.LeaseAndRepair,
            (int)invln_RehabActivityType.PurchaseandRepair => BuildActivityType.PurchaseAndRepair,
            (int)invln_RehabActivityType.Reimprovement => BuildActivityType.Reimprovement,
            (int)invln_RehabActivityType.WorksOnly => BuildActivityType.WorksOnlyRehab,
            _ => null,
        };
    }

    private static int? MapRehabBuildActivityType(BuildActivityType? buildActivityType)
    {
        return buildActivityType switch
        {
            BuildActivityType.AcquisitionAndWorksRehab => (int)invln_RehabActivityType.AcquisitionandWorksrehab,
            BuildActivityType.Conversion => (int)invln_RehabActivityType.Conversion,
            BuildActivityType.ExistingSatisfactory => (int)invln_RehabActivityType.ExistingSatisfactory,
            BuildActivityType.LeaseAndRepair => (int)invln_RehabActivityType.LeaseandRepair,
            BuildActivityType.PurchaseAndRepair => (int)invln_RehabActivityType.PurchaseandRepair,
            BuildActivityType.Reimprovement => (int)invln_RehabActivityType.Reimprovement,
            BuildActivityType.WorksOnlyRehab => (int)invln_RehabActivityType.WorksOnly,
            _ => null,
        };
    }

    private static StatusCode MapStatusCode(int? statusCode)
    {
        return statusCode switch
        {
            (int)invln_DeliveryPhase_StatusCode.AdjustmentAccepted => StatusCode.AdjustmentAccepted,
            (int)invln_DeliveryPhase_StatusCode.PendingAdjustment => StatusCode.PendingAdjustment,
            (int)invln_DeliveryPhase_StatusCode.RejectedAdjustment => StatusCode.RejectedAdjustment,
            _ => StatusCode.Default,
        };
    }

    private static IEnumerable<HomesToDeliverInPhase> MapHomesToDeliver(IDictionary<string, int?>? numberOfHomes)
    {
        return numberOfHomes?.Where(x => x.Value.IsProvided())
            .Select(x => new HomesToDeliverInPhase(HomeTypeId.From(x.Key), x.Value ?? 0)) ?? Enumerable.Empty<HomesToDeliverInPhase>();
    }

    private static Dictionary<string, int?> MapHomesToDeliver(IList<HomesToDeliverInPhase> homesToDeliver)
    {
        return homesToDeliver.ToDictionary<HomesToDeliverInPhase, string, int?>(x => x.HomeTypeId.ToGuidAsString(), x => x.Value);
    }

    private static DateTime? MapMilestoneDate<TDate>(MilestoneDetails<TDate>? milestoneDetails)
        where TDate : DateValueObject
    {
        return milestoneDetails?.MilestoneDate?.Value;
    }

    private static DateTime? MapPaymentDate<TDate>(MilestoneDetails<TDate>? milestoneDetails)
        where TDate : DateValueObject
    {
        return milestoneDetails?.PaymentDate?.Value;
    }

    private static IsAdditionalPaymentRequested? MapIsAdditionalPaymentRequested(string? value)
    {
        return value switch
        {
            "yes" => new IsAdditionalPaymentRequested(true),
            "no" => new IsAdditionalPaymentRequested(false),
            _ => null,
        };
    }

    private static string? MapIsAdditionalPaymentRequested(IsAdditionalPaymentRequested? value)
    {
        if (value?.IsRequested == true)
        {
            return "yes";
        }

        if (value?.IsRequested == false)
        {
            return "no";
        }

        return null;
    }
}
