import { IspService } from '../services/IspService'
import { CommonLib } from '../Common'
import { event } from 'jquery'

export class Isp {
  private common: CommonLib
  private ispService: IspService

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
    this.ispService = new IspService(eCtx)
  }

  public static onLoad(eCtx) {
    const eventLogic = new Isp(eCtx)
    eventLogic.ispService.mapFieldOnLoad();
    eventLogic.registerEvents()
    eventLogic.ispService.setStaticFieldsOnLoad()
    eventLogic.ispService.setFieldsRequirementBasedOnSendOnApproval()
    eventLogic.ispService.setFieldsVisibilityBasedOnSecurity()
    eventLogic.ispService.blockFieldsForLoansReviewer()
  }

  public static onSendOnApprovalChange(eCtx) {
    const eventLogic = new Isp(eCtx)
    eventLogic.ispService.setFieldsRequirementBasedOnSendOnApproval()
  }

  public static onTabChange(eCtx) {
    const eventLogic = new Isp(eCtx)
    eventLogic.ispService.showNotificationOnApprovalTab()
  }

  public registerEvents() {
    if (this.common.getAttribute('invln_sendforapproval')) {
      this.common.getAttribute('invln_sendforapproval').removeOnChange(Isp.onSendOnApprovalChange)
      this.common.getAttribute('invln_sendforapproval').addOnChange(Isp.onSendOnApprovalChange)
      this.common.getTab('Approval').addTabStateChange(Isp.onTabChange)
    }
  }
}
