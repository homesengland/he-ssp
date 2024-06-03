import { CommonLib } from '../Common'
import { Buildactivitytype, RehabActivityType, Tenure } from '../OptionSet';

export class DeliveryPhaseService {
  common: CommonLib

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
  }

  public ShowMessageOnSave(eCtx, application) {
    console.log("ShowMessageOnSave");

    let buildActivityType = this.common.getAttribute("invln_buildactivitytype").getValue();
    console.log("buildActivityType: " + buildActivityType);
    let rehabActivityType = this.common.getAttribute("invln_rehabactivitytype").getValue();
    console.log("rehabActivityType: " + rehabActivityType);
    let status = this.common.getAttribute("statuscode").getValue();
    console.log("status: " + status);
    if (status != 858110002)
      return;
    let tenure = application.invln_tenure;
    var eventArgs = eCtx.getEventArgs();
    if (tenure == Tenure.HOLD && buildActivityType != Buildactivitytype.OffTheShelf && rehabActivityType != RehabActivityType.ExistingSatisfactory) {
      console.log("Show message: This tenure does not support flexible payments");
      eventArgs.preventDefault();
      this.common.openConfirmDialog("This build activity type does not support flexible payments", "Alert");

    } else if (tenure == Tenure.HOLD && (buildActivityType == Buildactivitytype.OffTheShelf || rehabActivityType == RehabActivityType.ExistingSatisfactory)) {
      console.log("Show message: This build activity type does not support flexible payments");
      eventArgs.preventDefault();
      this.common.openConfirmDialog("This build activity type does not support flexible payments", "Alert");

    }
  }
  public async GetApplication() {
    let applicationId = this.common.getLookupValue('invln_application');
    if (applicationId != null)
      return await Xrm.WebApi.retrieveRecord('invln_scheme', applicationId.id);
  }

}
