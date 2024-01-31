using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Delivery.Enums;

namespace HE.Investment.AHP.Contract.Delivery.Commands;

public record CompleteDeliverySectionCommand(AhpApplicationId ApplicationId, IsDeliveryCompleted IsDeliveryCompleted, bool IsCheckOnly = false) : IDeliveryCommand;
