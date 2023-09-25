import { CommonLib } from '../Common'
import { ExternalStatus } from "../OptionSet"

export class VfTService {
  common: CommonLib

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
  }

  public setFieldsAvailabilityOnLoad() {
    var loanApplication = this.common.getLookupValue('invln_loanapplication')
    if (loanApplication != null) {
      Xrm.WebApi.retrieveRecord('invln_loanapplication', loanApplication.id).then(result => {
        if (result.invln_externalstatus == ExternalStatus.sentForApproval) {
          this.common.disableAllFields()
        }
      })
    }
  }

}
