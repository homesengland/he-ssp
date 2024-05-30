import { DeliveryPhaseService } from '../services/DeliveryPhaseService'
import { CommonLib } from '../Common'

var application;
export class DeliveryPhase {
  private common: CommonLib
  private deliveryPhaseService: DeliveryPhaseService

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
    this.deliveryPhaseService = new DeliveryPhaseService(eCtx)
  }

  public static async onLoad(eCtx) {
    const eventLogic = new DeliveryPhase(eCtx);
    application = await eventLogic.deliveryPhaseService.GetApplication();
  }

  public static onSave(eCtx) {
    const eventLogic = new DeliveryPhase(eCtx);
    eventLogic.deliveryPhaseService.ShowMessageOnSave(eCtx, application);
  }
}
