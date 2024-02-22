using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.WWW.Models.Application;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Models.Delivery.Factories;

public interface IDeliveryPhaseCheckAnswersViewModelFactory
{
    IList<SectionSummaryViewModel> CreateSummary(
        AhpApplicationId applicationId,
        DeliveryPhaseDetails deliveryPhase,
        DeliveryPhaseHomes deliveryPhaseHomes,
        IUrlHelper urlHelper,
        bool isEditable,
        bool useWorkflowRedirection = true);
}
