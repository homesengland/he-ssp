import { ConditionService } from '../services/ConditionService'
import { CommonLib } from '../Common'

export class Condition {
  private common: CommonLib
  private conditionService: ConditionService

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
    this.conditionService = new ConditionService(eCtx)
  }

  public static onLoad(eCtx) {
    const eventLogic = new Condition(eCtx)
    eventLogic.conditionService.setFieldsAvailabilityOnLoad()
  }
}
