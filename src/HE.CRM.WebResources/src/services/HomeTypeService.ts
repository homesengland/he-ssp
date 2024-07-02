import { CommonLib } from '../Common'
import { Tenure } from '../OptionSet';

export class HomeTypeService {
  common: CommonLib

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
  }

  public removeNotification() {
    console.log("Remove notification")
    this.common.clearFieldNotification("invln_percentagevalueofndssstandard", "NDSS Alert");
  }

  public async ndssCalculationError() {
    this.common.clearFieldNotification("invln_percentagevalueofndssstandard", "NDSS Alert");

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
      this.common.setFieldNotification("invln_percentagevalueofndssstandard", "Home type not covered by NDSS.", "NDSS Alert")
    }
  }

  public async showHideSection() {
    console.log("Execute showHideSection function");
    let applicationId = this.common.getAttributeValue("invln_application");
    let application = await Xrm.WebApi.retrieveRecord('invln_scheme', applicationId![0]['id']);
    let tenur = application.invln_tenure;

    if (tenur == Tenure.SharedOwnership || tenur == Tenure.OPSO || tenur == Tenure.HOLD) {
      console.log("show : Overview", "Shared Ownership, OPSO and HOLD");
      this.common.showSection("Overview", "Shared Ownership, OPSO and HOLD");
    }
    if (tenur == Tenure.RentRoBuy) {
      console.log("show : Overview", "Rent to Buy Details");
      this.common.showSection("Overview", "Rent to Buy Details");
    }
    if (tenur == Tenure.SocialRent) {
      console.log("show : Overview", "Social Rent Details");
      this.common.showSection("Overview", "Social Rent Details");
    }
    if (tenur == Tenure.AffordableRent) {
      console.log("show : Overview", "Affordable Rent Details");
      this.common.showSection("Overview", "Affordable Rent Details");
    }
  }
}
