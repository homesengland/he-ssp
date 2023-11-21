import { CommonLib } from '../Common'
import { ReviewApprovalService } from '../services/ReviewApprovalService'

export class ReviewApproval {
  private common: CommonLib
  private reviewApprovalService: ReviewApprovalService

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
    this.reviewApprovalService = new ReviewApprovalService(eCtx)
  }

  public static setNewReviewApprovalButtonVisibility(eCtx) {
    const eventLogic = new ReviewApproval(eCtx)
    return eventLogic.reviewApprovalService.setNewReviewApprovalButtonVisibility()
  }

  public static onLoad(eCtx) {
    const eventLogic = new ReviewApproval(eCtx)
    eventLogic.registerEvents()
    eventLogic.reviewApprovalService.onStateChange()
  }

  public static onStateChange(eCtx) {
    const eventLogic = new ReviewApproval(eCtx)
    eventLogic.reviewApprovalService.onStateChange()
  }

  public registerEvents() {
    if (this.common.getAttribute('invln_status')) {
      this.common.getAttribute('invln_status').removeOnChange(ReviewApproval.onStateChange)
      this.common.getAttribute('invln_status').addOnChange(ReviewApproval.onStateChange)
    }
  }
}
