import { IspService } from '../services/IspService'
import { CommonLib } from '../Common'

export class Isp {
  private common: CommonLib
  private ispService: IspService

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
    this.ispService = new IspService(eCtx)
  }

  public static onLoad(eCtx) {
    const eventLogic = new Isp(eCtx)
    eventLogic.ispService.setFieldsAvailabilityOnLoad()
  }
}
