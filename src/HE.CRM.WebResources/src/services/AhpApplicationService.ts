import { CommonLib } from '../Common'

export class AhpApplicationService {
  common: CommonLib

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
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
