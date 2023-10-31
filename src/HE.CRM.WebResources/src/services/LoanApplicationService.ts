import { CommonLib } from '../Common'
import { Securities } from "../OptionSet"

export class LoanApplicationService {
  common: CommonLib

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
  }

  public setFieldsVisibilityBasedOnSecurities() {
    var securities :any = this.common.getAttributeValue('invln_securities')
    debugger;
    if (securities != null) {
      if (securities.includes(Securities.other)) {
        this.common.hideControl('invln_customsecurity', false)
        this.common.setControlRequiredV2('invln_customsecurity', true)
      } else {
        this.common.hideControl('invln_customsecurity', true)
        this.common.setControlRequiredV2('invln_customsecurity', false)
      }
    }
  }


}
