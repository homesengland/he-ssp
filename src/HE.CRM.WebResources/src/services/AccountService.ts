import { CommonLib } from '../Common'
import { CreditRatingAgency } from "../OptionSet"

export class AccountService {
  common: CommonLib

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
  }

  public setFieldsAvailabilityOnLoad() {
    this.setFieldsAvailabilityBasedOnExternalCreditTrating()
    this.setFieldsAvailabilityBasedOnCreditRatingAgency()
  }

  public setFieldsAvailabilityBasedOnExternalCreditTrating() {
    var doesBorrowerHaveExternalCreditRating = this.common.getAttributeValue('invln_externalcreditrating')
    if (doesBorrowerHaveExternalCreditRating) {
      this.common.hideControl('invln_creditratingagency', false)
    } else {
      this.common.hideControl('invln_creditratingagency', true)
      this.common.setAttributeValue('invln_creditratingagency', null)
    }
  }

  public setFieldsAvailabilityBasedOnCreditRatingAgency() {
    var creditRatingAgency = this.common.getAttributeValue('invln_creditratingagency')
    if (creditRatingAgency == CreditRatingAgency.other) {
      this.common.hideControl('invln_othercreditratingagency', false)
    } else {
      this.common.hideControl('invln_othercreditratingagency', true)
      this.common.setAttributeValue('invln_othercreditratingagency', null)
    }
  }

}
