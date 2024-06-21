import { CommonLib } from '../Common'

export class ContactService {
  common: CommonLib

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
  }

  public openCustomPageSendInvite() {
    var userSettings = Xrm.Utility.getGlobalContext().userSettings;
    var userId = this.common.trimBraces(userSettings.userId);
    var recordId = this.common.trimBraces(this.common.getCurrentEntityId());

    var pageInput: any = {
      pageType: "custom",
      name: "invln_contactsendinvitation_faf14",
      entityName: userId,
      recordId: recordId,
    };
    var navigationOptions: any = {
      target: 2,
      position: 1,
      width: { value: 550, unit: "px" },
      height: { value: 320, unit: "px" },
      title: "Send invitation"
    };
    Xrm.Navigation.navigateTo(pageInput, navigationOptions).then(() => {
      this.common.refresh(false)
    })
  }
}
