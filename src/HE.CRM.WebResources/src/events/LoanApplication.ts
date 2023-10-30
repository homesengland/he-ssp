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
  }

  public static onSecuritiesChange(eCtx) {
    const eventLogic = new LoanApplication(eCtx)
    eventLogic.loanApplicationService.setFieldsVisibilityBasedOnSecurities()
  }

  public registerEvents() {
    if (this.common.getAttribute('invln_securities')) {
      this.common.getAttribute('invln_securities').removeOnChange(LoanApplication.onSecuritiesChange)
      this.common.getAttribute('invln_securities').addOnChange(LoanApplication.onSecuritiesChange)
    }
  }
}
