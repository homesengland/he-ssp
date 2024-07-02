import { CommonLib } from '../Common'

export class AhpApplicationService {
  common: CommonLib

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
  }

  public async nDSSNotification() {

    this.common.clearFormNotification("FormNotification1");

    let hometypes = await Xrm.WebApi.retrieveMultipleRecords("invln_hometype", "?$select=invln_hometypename&$filter=(_invln_application_value eq '8c981e55-a437-ef11-a317-002248c5d15f' and invln_percentagevalueofndssstandard eq null)&$top=50");

    let names = "";
    if (hometypes.entities.length > 0) {
      for (var i = 0; i < hometypes.entities.length; i++) {
        names = names + ", " + hometypes.entities[i].invln_hometypename
      }
      this.common.setFormNotification("Home type: " + names + " not covered by NDSS.", XrmEnum.FormNotificationLevel.Info, "FormNotification1")
    }

    this.common.clearFormNotification("NDSSNotification");
    this.common.clearFormNotification("NDSSNotCoveredNotification");
    let ndssmax = this.common.getAttribute("invln_maximumm2asofndssofthehometypesonthis").getValue();
    let ndssmin = this.common.getAttribute("invln_minimumm2asofndssofthehometypesonthis").getValue();
    let status = this.common.getAttribute("invln_externalstatus").getValue();
    if (status == 858110001) //draft
      return;

    if (!(ndssmax == null && ndssmin == null))
      return;

    var ndssmaxc = <any>this.common.getControl('invln_maximumm2asofndssofthehometypesonthis');

    ndssmaxc.addNotification({
      messages: ["Home type not covered by NDSS."],
      notificationLevel: 'RECOMMENDATION',
      uniqueId: 'ndssmaxc',
      actions: null
    });

    var ndssminc = <any>this.common.getControl('invln_minimumm2asofndssofthehometypesonthis');

    ndssminc.addNotification({
      messages: ["Home type not covered by NDSS."],
      notificationLevel: 'RECOMMENDATION',
      uniqueId: 'ndssminc',
      actions: null
    });

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
