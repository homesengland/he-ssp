import { CommonLib } from '../Common'
import { Buildactivitytype, RehabActivityType, Tenure } from '../OptionSet';

export class DeliveryPhaseService {
  common: CommonLib

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
  }

  public async ShowMessageOnSave() {
    console.log("ShowMessageOnSave");

    let buildActivityType = this.common.getAttribute("invln_buildactivitytype").getValue();
    console.log("buildActivityType: " + buildActivityType);
    let rehabActivityType = this.common.getAttribute("invln_rehabactivitytype").getValue();
    console.log("rehabActivityType: " + rehabActivityType);
    let status = this.common.getAttribute("statuscode").getValue();
    console.log("status: " + status);
    let applicationId = this.common.getLookupValue('invln_application');
    console.log("applicationId: " + applicationId);
    if (status != 858110002)
      return;

    if (applicationId != null) {
      let application = await Xrm.WebApi.retrieveRecord('invln_scheme', applicationId.id);
      let tenure = application.invln_tenure;
      console.log("tenure: " + tenure);
      if (tenure == Tenure.HOLD) {
        console.log("Show message: This tenure does not support flexible payments");
        this.common.preventSave();
        this.common.openConfirmDialog("This build activity type does not support flexible payments", "Alert");
        this.common.preventSave();
      } else if (tenure == Tenure.HOLD && (buildActivityType == Buildactivitytype.OffTheShelf || rehabActivityType == RehabActivityType.ExistingSatisfactory)) {
        console.log("Show message: This build activity type does not support flexible payments");
        this.common.preventSave();
        this.common.openConfirmDialog("This build activity type does not support flexible payments", "Alert");

      }

    }

  }

}
