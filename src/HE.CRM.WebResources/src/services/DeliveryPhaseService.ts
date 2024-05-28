import { CommonLib } from '../Common'

export class DeliveryPhaseService {
  common: CommonLib

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
  }

  public ShowMessageOnSave() {
    let buildActivityType = this.common.getAttribute("invln_buildactivitytype").getValue();
    let rehabActivityType = this.common.getAttribute("invln_rehabactivitytype").getValue();
  }

}
