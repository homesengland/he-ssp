import { CommonLib } from '../Common'
import { StatusReviewApproval } from "../OptionSet"

export class ReviewApprovalService {
  common: CommonLib

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
  }

  public setNewReviewApprovalButtonVisibility() {
    return new Promise((resolve, reject) => {
      var entityId = this.common.getCurrentEntityId();
      Xrm.WebApi.retrieveMultipleRecords("invln_reviewapproval", "?$select=invln_reviewerapprover,invln_status&$filter=_invln_ispid_value eq '" + this.common.trimBraces(entityId) + "' and (invln_reviewerapprover eq 858110000 or invln_reviewerapprover eq 858110001)").then(result => {
        for (var i = 0; i < result.entities.length; i++) {
          var element = result.entities[0];
          if (element.invln_status == StatusReviewApproval.Pending || element.invln_status == StatusReviewApproval.Sent_Back || element.invln_status == StatusReviewApproval.Rejected) {
            resolve(false);
          }
        }

        resolve(true);
      })
    })
  }

  public onStateChange() {
    var currentUserId = this.common.getUserId() as any;
    var currentUserName = this.common.getUserName() as any;
    this.common.setLookUpValue("invln_reviewedapprovedbyid", "systemuser", currentUserId, currentUserName);
    this.common.setAttributeValue("invln_reviewapprovaldate", new Date(Date.now()));
    this.setStatusRequired();
  }

  public setStatusRequired() {
    var status = this.common.getAttributeValue("invln_status");
    if (status == StatusReviewApproval.Rejected) {
      this.common.setControlRequiredV2("invln_reviewerapprovercomments", true);
    }
    else {
      this.common.setControlRequiredV2("invln_reviewerapprovercomments", false);
    }
  }
}
