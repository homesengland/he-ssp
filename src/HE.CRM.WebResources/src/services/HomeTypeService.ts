import { CommonLib } from '../Common'

export class HomeTypeService {
  common: CommonLib

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
  }

  public async ndssCalculationError() {
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
    let ndss = await Xrm.WebApi.retrieveMultipleRecords("invln_ndss", "?$select=invln_standardnumber&$filter=(invln_standardnumber eq " + concatenatevalue + ")&$top=50");
    if (ndss.entities.length == 0) {
      this.common.openConfirmDialog("Home type not covered by NDSS.", " NDSS Alert.");
    }
  }
}
