import { DeliveryPhaseService } from '../services/DeliveryPhaseService'
import { CommonLib } from '../Common'

export class DeliveryPhase {
  private common: CommonLib
  private deliveryPhaseService: DeliveryPhaseService

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
    this.deliveryPhaseService = new DeliveryPhaseService(eCtx)
  }

  public static onSave(eCtx) {
    const eventLogic = new DeliveryPhase(eCtx);
    eventLogic.deliveryPhaseService.ShowMessageOnSave();
  }
}
