import { LoanApplicationService } from '../services/LoanApplicationService'
import { CommonLib } from '../Common'

export class LoanApplication {
  private common: CommonLib
  private loanApplicationService: LoanApplicationService

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
    this.loanApplicationService = new LoanApplicationService(eCtx)
  }

  public static onLoad(eCtx) {
    const eventLogic = new LoanApplication(eCtx)
    eventLogic.registerEvents();
    eventLogic.loanApplicationService.setFieldsVisibilityBasedOnSecurities();
    eventLogic.loanApplicationService.populateFields();
    eventLogic.loanApplicationService.setFieldsVisibilityBasedOnAssessedAsSppi();
  }

  public static onSecuritiesChange(eCtx) {
    const eventLogic = new LoanApplication(eCtx)
    eventLogic.loanApplicationService.setFieldsVisibilityBasedOnSecurities()
  }

  public static onAdditionalReturnsChange(eCtx) {
    const eventLogic = new LoanApplication(eCtx)
    eventLogic.loanApplicationService.populateFields()
  }

  public static onRateOfInterestChange(eCtx) {
    const eventLogic = new LoanApplication(eCtx)
    eventLogic.loanApplicationService.populateFields()
  }

  public static onSpecialPuproseVehicleProvidedChange(eCtx) {
    const eventLogic = new LoanApplication(eCtx)
    eventLogic.loanApplicationService.populateFields()
  }

  public static onLtgdvChange(eCtx) {
    const eventLogic = new LoanApplication(eCtx)
    eventLogic.loanApplicationService.populateFields()
  }

  public static onInvestedByBorrowerChange(eCtx) {
    const eventLogic = new LoanApplication(eCtx)
    eventLogic.loanApplicationService.populateFields()
  }

  public static onProjectedProfitMarginChange(eCtx) {
    const eventLogic = new LoanApplication(eCtx)
    eventLogic.loanApplicationService.populateFields()
  }

  public static onAssessedAsSppiChange(eCtx) {
    const eventLogic = new LoanApplication(eCtx)
    eventLogic.loanApplicationService.setFieldsVisibilityBasedOnAssessedAsSppi()
  }

  public registerEvents() {
    if (this.common.getAttribute('invln_securities')) {
      this.common.getAttribute('invln_securities').removeOnChange(LoanApplication.onSecuritiesChange)
      this.common.getAttribute('invln_securities').addOnChange(LoanApplication.onSecuritiesChange)
    }
    if (this.common.getAttribute('invln_additionalreturns')) {
      this.common.getAttribute('invln_additionalreturns').removeOnChange(LoanApplication.onAdditionalReturnsChange)
      this.common.getAttribute('invln_additionalreturns').addOnChange(LoanApplication.onAdditionalReturnsChange)
    }
    if (this.common.getAttribute('invln_rateofinterest')) {
      this.common.getAttribute('invln_rateofinterest').removeOnChange(LoanApplication.onRateOfInterestChange)
      this.common.getAttribute('invln_rateofinterest').addOnChange(LoanApplication.onRateOfInterestChange)
    }
    if (this.common.getAttribute('invln_specialpurposevehicleprovided')) {
      this.common.getAttribute('invln_specialpurposevehicleprovided').removeOnChange(LoanApplication.onSpecialPuproseVehicleProvidedChange)
      this.common.getAttribute('invln_specialpurposevehicleprovided').addOnChange(LoanApplication.onSpecialPuproseVehicleProvidedChange)
    }
    if (this.common.getAttribute('invln_ltgdv')) {
      this.common.getAttribute('invln_ltgdv').removeOnChange(LoanApplication.onLtgdvChange)
      this.common.getAttribute('invln_ltgdv').addOnChange(LoanApplication.onLtgdvChange)
    }
    if (this.common.getAttribute('invln_investedbyborrower')) {
      this.common.getAttribute('invln_investedbyborrower').removeOnChange(LoanApplication.onInvestedByBorrowerChange)
      this.common.getAttribute('invln_investedbyborrower').addOnChange(LoanApplication.onInvestedByBorrowerChange)
    }
    if (this.common.getAttribute('invln_projectedprofitmargin')) {
      this.common.getAttribute('invln_projectedprofitmargin').removeOnChange(LoanApplication.onProjectedProfitMarginChange)
      this.common.getAttribute('invln_projectedprofitmargin').addOnChange(LoanApplication.onProjectedProfitMarginChange)
    }
    if (this.common.getAttribute('invln_assessedassppi')) {
      this.common.getAttribute('invln_assessedassppi').removeOnChange(LoanApplication.onAssessedAsSppiChange)
      this.common.getAttribute('invln_assessedassppi').addOnChange(LoanApplication.onAssessedAsSppiChange)
    }
  }
}
