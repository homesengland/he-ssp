using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.Tranches;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Model;
using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Extensions;

namespace HE.Investment.AHP.Domain.Delivery.Crm;

public class DeliveryPhaseCrmMapper : IDeliveryPhaseCrmMapper
{
    public IReadOnlyCollection<string> CrmFields => new[]
    {
        nameof(invln_DeliveryPhase.invln_phasename),
        nameof(invln_DeliveryPhase.CreatedOn),
        nameof(invln_DeliveryPhase.invln_iscompleted),
        nameof(invln_DeliveryPhase.invln_buildactivitytype),
        nameof(invln_DeliveryPhase.invln_rehabactivitytype),
        nameof(invln_DeliveryPhase.invln_reconfiguringexistingproperties),
        nameof(invln_DeliveryPhase.invln_acquisitiondate),
        nameof(invln_DeliveryPhase.invln_acquisitionmilestoneclaimdate),
        nameof(invln_DeliveryPhase.invln_startonsitedate),
        nameof(invln_DeliveryPhase.invln_startonsitemilestoneclaimdate),
        nameof(invln_DeliveryPhase.invln_completiondate),
        nameof(invln_DeliveryPhase.invln_completionmilestoneclaimdate),
        nameof(invln_DeliveryPhase.invln_urbrequestingearlymilestonepayments),
        nameof(invln_DeliveryPhase.invln_nbrh),
        "invln_AcquisitionPercentageValue",
        "invln_StartOnSitePercentageValue",
        "invln_CompletionPercentageValue",
        "invln_ClaimingtheMilestoneConfirmed",
        "invln_AllowAmendmentstoMilestoneProportions",
    };

    public DeliveryPhaseEntity MapToDomain(ApplicationBasicInfo application, OrganisationBasicInfo organisation, DeliveryPhaseDto dto, SchemeFunding schemeFunding)
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
            dto.allowAmendmentstoMilestoneProportions == true,
            schemeFunding,
            typeOfHomes,
            buildActivity,
            dto.isReconfigurationOfExistingProperties,
            MapHomesToDeliver(dto.numberOfHomes),
            AcquisitionMilestoneDetails.Create(
                MapDate(dto.acquisitionDate, AcquisitionDate.Create),
                MapDate(dto.acquisitionPaymentDate, MilestonePaymentDate.Create)),
            StartOnSiteMilestoneDetails.Create(
                MapDate(dto.startOnSiteDate, StartOnSiteDate.Create),
                MapDate(dto.startOnSitePaymentDate, MilestonePaymentDate.Create)),
            CompletionMilestoneDetails.Create(
                MapDate(dto.completionDate, CompletionDate.Create),
                MapDate(dto.completionPaymentDate, MilestonePaymentDate.Create)),
            new DeliveryPhaseId(dto.id),
            dto.createdOn,
            MapIsAdditionalPaymentRequested(dto.requiresAdditionalPayments),
            dto.claimingtheMilestoneConfirmed);
    }

    public DeliveryPhaseDto MapToDto(DeliveryPhaseEntity entity)
    {
        return new DeliveryPhaseDto
        {
            id = entity.Id.IsNew ? null : entity.Id.Value,
            applicationId = entity.Application.Id.Value,
            name = entity.Name.Value,
            isCompleted = entity.Status == SectionStatus.Completed,
            newBuildActivityType = MapNewBuildActivityType(entity.BuildActivity.Type),
            rehabBuildActivityType = MapRehabBuildActivityType(entity.BuildActivity.Type),
            isReconfigurationOfExistingProperties = entity.ReconfiguringExisting,
            acquisitionPercentageValue = entity.Tranches.PercentagesAmended.Acquisition?.Value,
            startOnSitePercentageValue = entity.Tranches.PercentagesAmended.StartOnSite?.Value,
            completionPercentageValue = entity.Tranches.PercentagesAmended.Completion?.Value,
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

    private static IEnumerable<HomesToDeliverInPhase> MapHomesToDeliver(IDictionary<string, int?>? numberOfHomes)
    {
        return numberOfHomes?.Where(x => x.Value.IsProvided())
            .Select(x => new HomesToDeliverInPhase(HomeTypeId.From(x.Key), x.Value ?? 0)) ?? Enumerable.Empty<HomesToDeliverInPhase>();
    }

    private static Dictionary<string, int?> MapHomesToDeliver(IList<HomesToDeliverInPhase> homesToDeliver)
    {
        return homesToDeliver.ToDictionary<HomesToDeliverInPhase, string, int?>(x => x.HomeTypeId.Value, x => x.ToDeliver);
    }

    private static DateTime? MapMilestoneDate<TDate>(MilestoneDetails<TDate>? milestoneDetails)
        where TDate : DateValueObject
    {
        return milestoneDetails?.MilestoneDate?.Value.ToDateTime(TimeOnly.MinValue);
    }

    private static DateTime? MapPaymentDate<TDate>(MilestoneDetails<TDate>? milestoneDetails)
        where TDate : DateValueObject
    {
        return milestoneDetails?.PaymentDate?.Value.ToDateTime(TimeOnly.MinValue);
    }

    private static TDate? MapDate<TDate>(DateTime? date, Func<DateOnly, TDate> dateFactory)
    {
        if (!date.HasValue)
        {
            return default;
        }

        return dateFactory(new DateOnly(date.Value.Year, date.Value.Month, date.Value.Day));
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
