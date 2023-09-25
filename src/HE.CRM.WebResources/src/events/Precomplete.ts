import { PrecompleteService } from '../services/PrecompleteService'
import { CommonLib } from '../Common'

export class Precomplete {
  private common: CommonLib
  private precompleteService: PrecompleteService

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
    this.precompleteService = new PrecompleteService(eCtx)
  }

  public static onLoad(eCtx) {
    const eventLogic = new Precomplete(eCtx)
    eventLogic.precompleteService.setFieldsAvailabilityOnLoad()
  }
}
