using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Common.Enums;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record CompleteDeliverySectionCommand(AhpApplicationId ApplicationId, IsSectionCompleted IsSectionCompleted, bool IsCheckOnly = false) : IDeliveryCommand;
