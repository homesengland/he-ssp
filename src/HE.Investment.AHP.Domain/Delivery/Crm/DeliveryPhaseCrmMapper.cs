using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Enums;
using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.Common;
using HE.Investment.AHP.Domain.Delivery.Entities;
using HE.Investment.AHP.Domain.Delivery.MilestonePayments;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.CRM.Model;
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
    };

    public DeliveryPhaseEntity MapToDomain(ApplicationBasicInfo application, OrganisationBasicInfo organisation, DeliveryPhaseDto dto)
    {
        var typeOfHomes = MapTypeOfHomes(dto.typeOfHomes);
        var buildActivityType = MapBuildActivityType(dto.newBuildActivityType, dto.rehabBuildActivityType);
        var buildActivity = new BuildActivity(application.Tenure, typeOfHomes, buildActivityType);

        return new DeliveryPhaseEntity(
            application,
            new DeliveryPhaseName(dto.name),
            organisation,
            dto.isCompleted == true ? SectionStatus.Completed : SectionStatus.InProgress,
            MilestoneTranches.NotProvided, // TODO: Task 89103: [CRM] Save tranches (Milestone framework)
            typeOfHomes,
            buildActivity,
            dto.isReconfigurationOfExistingProperties,
            MapHomesToDeliver(dto.numberOfHomes),
            MapDeliveryPhaseMilestones(organisation, buildActivity, dto),
            new DeliveryPhaseId(dto.id),
            dto.createdOn,
            MapIsAdditionalPaymentRequested(dto.requiresAdditionalPayments));
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
            (int)invln_newbuildactivitytype.AcquisitionandWorks => BuildActivityType.AcquisitionAndWorks,
            (int)invln_newbuildactivitytype.LandInclusivePackage_packagedeal => BuildActivityType.LandInclusivePackage,
            (int)invln_newbuildactivitytype.OffTheShelf => BuildActivityType.OffTheShelf,
            (int)invln_newbuildactivitytype.WorksOnly => BuildActivityType.WorksOnly,
            _ => null,
        };
    }

    private static int? MapNewBuildActivityType(BuildActivityType? buildActivityType)
    {
        return buildActivityType switch
        {
            BuildActivityType.AcquisitionAndWorks => (int)invln_newbuildactivitytype.AcquisitionandWorks,
            BuildActivityType.LandInclusivePackage => (int)invln_newbuildactivitytype.LandInclusivePackage_packagedeal,
            BuildActivityType.OffTheShelf => (int)invln_newbuildactivitytype.OffTheShelf,
            BuildActivityType.WorksOnly => (int)invln_newbuildactivitytype.WorksOnly,
            _ => null,
        };
    }

    private static BuildActivityType? MapRehabBuildActivityType(int rehabBuildActivityType)
    {
        return rehabBuildActivityType switch
        {
            (int)invln_rehabactivitytype.AcquisitionandWorks_rehab => BuildActivityType.AcquisitionAndWorksRehab,
            (int)invln_rehabactivitytype.Conversion => BuildActivityType.Conversion,
            (int)invln_rehabactivitytype.ExistingSatisfactory => BuildActivityType.ExistingSatisfactory,
            (int)invln_rehabactivitytype.LeaseandRepair => BuildActivityType.LeaseAndRepair,
            (int)invln_rehabactivitytype.PurchaseandRepair => BuildActivityType.PurchaseAndRepair,
            (int)invln_rehabactivitytype.Reimprovement => BuildActivityType.Reimprovement,
            (int)invln_rehabactivitytype.WorksOnly => BuildActivityType.WorksOnlyRehab,
            _ => null,
        };
    }

    private static int? MapRehabBuildActivityType(BuildActivityType? buildActivityType)
    {
        return buildActivityType switch
        {
            BuildActivityType.AcquisitionAndWorksRehab => (int)invln_rehabactivitytype.AcquisitionandWorks_rehab,
            BuildActivityType.Conversion => (int)invln_rehabactivitytype.Conversion,
            BuildActivityType.ExistingSatisfactory => (int)invln_rehabactivitytype.ExistingSatisfactory,
            BuildActivityType.LeaseAndRepair => (int)invln_rehabactivitytype.LeaseandRepair,
            BuildActivityType.PurchaseAndRepair => (int)invln_rehabactivitytype.PurchaseandRepair,
            BuildActivityType.Reimprovement => (int)invln_rehabactivitytype.Reimprovement,
            BuildActivityType.WorksOnlyRehab => (int)invln_rehabactivitytype.WorksOnly,
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

    private static DeliveryPhaseMilestones MapDeliveryPhaseMilestones(
        OrganisationBasicInfo organisation,
        BuildActivity buildActivity,
        DeliveryPhaseDto dto)
    {
        var acquisitionDetails = AcquisitionMilestoneDetails.Create(
            MapDate(dto.acquisitionDate, AcquisitionDate.Create),
            MapDate(dto.acquisitionPaymentDate, MilestonePaymentDate.Create));
        var startOnSiteDetails = StartOnSiteMilestoneDetails.Create(
            MapDate(dto.startOnSiteDate, StartOnSiteDate.Create),
            MapDate(dto.startOnSitePaymentDate, MilestonePaymentDate.Create));
        var completionDetails = CompletionMilestoneDetails.Create(
            MapDate(dto.completionDate, CompletionDate.Create),
            MapDate(dto.completionPaymentDate, MilestonePaymentDate.Create));

        return new DeliveryPhaseMilestones(
            organisation,
            buildActivity,
            acquisitionDetails,
            startOnSiteDetails,
            completionDetails);
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
