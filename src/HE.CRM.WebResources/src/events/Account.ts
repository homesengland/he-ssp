import { AccountService } from '../services/AccountService'
import { CommonLib } from '../Common'

export class Account {
  private common: CommonLib
  private accountService: AccountService

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
    this.accountService = new AccountService(eCtx)
  }

  public static onLoad(eCtx) {
    const eventLogic = new Account(eCtx)
    eventLogic.registerEvents()
    eventLogic.accountService.setFieldsAvailabilityOnLoad()
  }

  public static onExternalCreditTratingChange(eCtx) {
    const eventLogic = new Account(eCtx)
    eventLogic.accountService.setFieldsAvailabilityBasedOnExternalCreditTrating()
  }
  public static onCreditTratingAgencyChange(eCtx) {
    const eventLogic = new Account(eCtx)
    eventLogic.accountService.setFieldsAvailabilityBasedOnCreditRatingAgency()
  }

  public registerEvents() {
    if (this.common.getAttribute('invln_externalcreditrating')) {
      this.common.getAttribute('invln_externalcreditrating').removeOnChange(Account.onExternalCreditTratingChange)
      this.common.getAttribute('invln_externalcreditrating').addOnChange(Account.onExternalCreditTratingChange)
    }
    if (this.common.getAttribute('invln_creditratingagency')) {
      this.common.getAttribute('invln_creditratingagency').removeOnChange(Account.onCreditTratingAgencyChange)
      this.common.getAttribute('invln_creditratingagency').addOnChange(Account.onCreditTratingAgencyChange)
    }
  }
}
