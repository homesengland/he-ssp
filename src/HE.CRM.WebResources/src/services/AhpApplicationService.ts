import { CommonLib } from '../Common'

export class AhpApplicationService {
  common: CommonLib

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
  }

  public nDSSNotification() {
    this.common.clearFormNotification("NDSSNotification");
    this.common.clearFormNotification("NDSSNotCoveredNotification");
    let ndssmax = this.common.getAttribute("invln_maximumm2asofndssofthehometypesonthis").getValue();
    let ndssmin = this.common.getAttribute("invln_minimumm2asofndssofthehometypesonthis").getValue();
    let tenure = this.common.getAttribute("invln_tenure").getValue();
    let status = this.common.getAttribute("invln_externalstatus").getValue();
    if (status == 858110001) //draft
      return;

    if (!(ndssmax == null && ndssmin == null))
      return;

    this.common.setFormNotification("Home type not covered by NDSS.", XrmEnum.FormNotificationLevel.Info, "NDSSNotCoveredNotification");
  }

  public openCustomPage() {
    var recordId = this.common.trimBraces(this.common.getCurrentEntityId())
    var pageInput: any = {
      pageType: "custom",
      name: "invln_changeahpapplicationstatus_d1179",
      recordLogicalName: "invln_scheme",
      recordId: recordId,
    };
    var navigationOptions: any = {
      target: 2,
      position: 1,
      width: { value: 900, unit: "px" },
      height: { value: 600, unit: "px" },
      title: "Change Status"
    };
    Xrm.Navigation.navigateTo(pageInput, navigationOptions).then(() => {
      this.common.refresh(false)
    })
  }

}
