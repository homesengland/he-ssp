import { CommonLib } from '../Common'

export class ClaimService {
  common: CommonLib

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
  }


  public openCustomPage() {
    var recordId = this.common.trimBraces(this.common.getCurrentEntityId())
    var pageInput: any = {
      pageType: "custom",
      name: "invln_changeclaimstatus_5c854",
      recordLogicalName: "invln_claim",
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
