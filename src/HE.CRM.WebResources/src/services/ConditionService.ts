import { CommonLib } from '../Common'

export class ConditionService {
  common: CommonLib

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
  }

  public setFieldsAvailabilityOnLoad() {
    var standardCondition = this.common.getAttributeValue('invln_standardcondition')
    if (standardCondition == null) {
      this.common.hideControl('invln_standardcondition', true)
    }
  }

}
