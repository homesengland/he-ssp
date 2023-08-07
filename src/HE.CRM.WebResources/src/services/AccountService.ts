import { CommonLib } from '../Common'
import { CreditRatingAgency } from "../OptionSet"

export class AccountService {
  common: CommonLib

  static readonly DateApprovedInFutureMsg: string = "Date approved cannot be in future and must be in previous 3 years"
  static readonly DateApprovedInFutureMsgId: string = "DateApprovedInFutureMsgId"
  static readonly DateDuteForRenewalInPastMsg: string = "Date due for renewal cannot be in past and must be in next 3 years"
  static readonly DateDuteForRenewalInPastMsgId: string = "DateDuteForRenewalInPastMsgId"

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
      this.setFieldsAvailabilityBasedOnCreditRatingAgency()
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

  public checkIfDateApprovedIsInPast() {
    this.common.clearFieldNotification('invln_dateapproved', AccountService.DateApprovedInFutureMsgId)
    var dateApproved : any = this.common.getAttributeValue('invln_dateapproved')
    if (dateApproved != null) {
      dateApproved = dateApproved.setHours(0, 0, 0, 0)
      var today = new Date().setHours(0, 0, 0, 0)
      var threeYearsAgo = new Date().setMonth(new Date().getMonth() - 36)
      if (dateApproved > today || threeYearsAgo > dateApproved) {
        this.common.setFieldNotification('invln_dateapproved', AccountService.DateApprovedInFutureMsg, AccountService.DateApprovedInFutureMsgId);
      }
    }
  }
  public checkIfDateDueForRenewalIsInFuture() {
    this.common.clearFieldNotification('invln_datedueforrenewal', AccountService.DateDuteForRenewalInPastMsgId)
    var dateApproved: any = this.common.getAttributeValue('invln_datedueforrenewal')
    if (dateApproved != null) {
      dateApproved = dateApproved.setHours(0, 0, 0, 0)
      var today = new Date().setHours(0, 0, 0, 0)
      var dateInThreeYears = new Date().setMonth(new Date().getMonth() + 36)
      if (dateApproved < today || dateApproved > dateInThreeYears) {
        this.common.setFieldNotification('invln_datedueforrenewal', AccountService.DateDuteForRenewalInPastMsg, AccountService.DateDuteForRenewalInPastMsgId);
      } 
    }
  }
  

}
