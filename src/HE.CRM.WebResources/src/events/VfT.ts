import { VfTService } from '../services/VfTService'
import { CommonLib } from '../Common'

export class VfT {
  private common: CommonLib
  private vfTService: VfTService

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
    this.vfTService = new VfTService(eCtx)
  }

  public static onLoad(eCtx) {
    const eventLogic = new VfT(eCtx)
    eventLogic.vfTService.setFieldsAvailabilityOnLoad()
  }
}
