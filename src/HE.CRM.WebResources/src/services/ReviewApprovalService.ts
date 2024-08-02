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
          var element = result.entities[i];
          if (element.invln_status == StatusReviewApproval.Pending || element.invln_status == StatusReviewApproval.Sent_Back || element.invln_status == StatusReviewApproval.Rejected) {
            resolve(false);
          }
        }

        resolve(true);
      })
    })
  }

  public onStateChange() {
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

  public addFilterToHoFIndividualField() {

    var fetchXml =
      "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>" +
      "  <entity name='systemuser'>" +
      "    <link-entity name='teammembership' from='systemuserid' to='systemuserid' intersect='true'>" +
      "      <filter>" +
      "       <condition attribute='teamid' operator='eq' value='009544c2-477a-ee11-8179-002248004a06' />" +
      "      </filter>" +
      "    </link-entity>" +
      "  </entity>" +
      "</fetch>";

    var layoutXml = "<grid name='resultset' object='1' jump='systemuserid' select='1' icon='1' preview='1'>" +
      "<row name='result' id='systemuserid'>" +
      "<cell name='fullname' width='150' />" +
      "</row></grid>";

    var viewId = "{00000000-0000-0000-0000-000000000003}";
    var viewDisplayName = "Hof Team Users";

    this.common.addCustomViewToLookup("invln_hofindividual", viewId, "systemuser", viewDisplayName, fetchXml, layoutXml);
  };

  public addAccessToTransactionManager() {
    let isTrasactionManager = false;
    let transactionMenagerRoleName = "[Loans] Transaction manager";

    let securityRoles = this.common.getUserSecurityRoles();
    securityRoles?.forEach(function (value) {
      if (value.name == transactionMenagerRoleName) isTrasactionManager = true;
      console.log(value.name);
    });

    let approvalType = this.common.getAttribute("invln_reviewerapprover").getValue();
    let status = this.common.getAttribute("invln_status").getValue();
    if (!(isTrasactionManager && status == 858110000 && (approvalType == 858110001 || approvalType == 858110002))) {
      this.common.hideControl("invln_hofindividual", true)
    }
  }

}
