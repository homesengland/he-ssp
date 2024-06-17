import { CommonLib } from '../Common'

export class AhpApplicationService {
  common: CommonLib

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
  }

  public ndssCalculationError() {
    let numberofbedrooms = this.common.getAttribute("invln_numberofbedrooms").getValue();
    console.log(numberofbedrooms);
    let maxoccupancy = this.common.getAttribute("invln_maxoccupancy").getValue();
    console.log(maxoccupancy);
    let numberofstoreys = this.common.getAttribute("invln_numberofstoreys").getValue();
    console.log(numberofstoreys);
    if (numberofbedrooms == null || maxoccupancy == null || numberofstoreys == null) {
      console.log("return");
      return;
    }
    let concatenatevalue = numberofbedrooms.toString() + maxoccupancy.toString() + numberofstoreys.toString();

    https://investmentsdev.crm11.dynamics.com/api/data/v9.2/invln_ndsses?$select=invln_standardnumber&$filter=(invln_standardnumber eq 111)&$top=50

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
