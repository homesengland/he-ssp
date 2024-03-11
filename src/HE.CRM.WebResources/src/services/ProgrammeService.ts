import { CommonLib } from '../Common'

export class ProgrammeService {
  common: CommonLib;

  constructor(eCtx) {
    this.common = new CommonLib(eCtx);
  }

  public async ShowFormNotification() {
    let milestone = await Xrm.WebApi.retrieveMultipleRecords("invln_programme", "?$expand=invln_milestoneframeworkitem_programmeId($select=invln_percentagepaidonmilestone)&$filter=(invln_programmeid eq '" + this.common.getCurrentEntityId() + "') and (invln_milestoneframeworkitem_programmeId/any(o1:(o1/invln_milestoneframeworkitemid ne null)))");
    let sum = 0;
    let milstones = milestone.entities[0]["invln_milestoneframeworkitem_programmeId"];
    for (var i = 0; i < milstones.length; i++) {
      sum = sum + milstones[i]["invln_percentagepaidonmilestone"]
      console.log(sum);
    }

    if (sum == 100) {
      this.common.clearFormNotification("mileStoneNotification");
    } else {
      this.common.setFormNotification("There is a problem Tranche proportions for this delivery phase must add to 100 % ",
        XrmEnum.FormNotificationLevel.Error,
        "mileStoneNotification");
    }
  }
}
