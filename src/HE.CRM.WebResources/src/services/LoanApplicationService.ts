import { CommonLib } from '../Common'
import { Securities } from "../OptionSet"
import { LoanAppInternalStatus } from "../OptionSet"

export class LoanApplicationService {
  common: CommonLib

  private static readonly finalConclusionAssetDoesNotMeet: string = "The Asset does not meet the criteria to be assessed as SPPI and therefore will be classified as a Fair Value Through Profit or Loss (FVTPL) asset. Please submit assessment to IAT@homesengland.gov.uk"
  private static readonly finalConclusionContactFinancial: string = "Contact Financial Accounts Team. Submit assessment to IAT@homesengland.gov.uk"
  private static readonly finalConclusionAssetDoesMeet: string = "The Asset does meet the criteria to be assessed as SPPI and therefore will be classified as an Amortised Cost asset"

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
  }

  public takeActionsOnTabSecurities() {
    let status = this.common.getAttribute("statuscode").getValue();

    let requireSectionForStatuses = [
      LoanAppInternalStatus.ReferredBacktoApplicant,
      LoanAppInternalStatus.SentforApproval,
      LoanAppInternalStatus.UnderReview,
      LoanAppInternalStatus.NotApproved,
      LoanAppInternalStatus.ApprovedSubjecttoDueDiligence,
      LoanAppInternalStatus.ApplicationDeclined,
      LoanAppInternalStatus.InDueDiligence,
      LoanAppInternalStatus.SentforPreCompleteApproval,
      LoanAppInternalStatus.ApprovedSubjectToContract,
      LoanAppInternalStatus.AwaitingCPSatisfaction,
      LoanAppInternalStatus.CPsSatisfied,
      LoanAppInternalStatus.LoanAvailable
    ]

    if (status == LoanAppInternalStatus.Draft
      || status == LoanAppInternalStatus.ApplicationSubmitted) {
      this.common.setControlRequiredV2('invln_securities', false);
    } else {
      this.common.setControlRequiredV2('invln_securities', true);
    }

    let requireSection = requireSectionForStatuses.includes(status);
    var securities: any = this.common.getAttributeValue('invln_securities');

    const securitiesArray = Object
      .keys(Securities)
      .filter((v) => !isNaN(Number(v)))
      .map((x) => parseInt(x));

    if (securities == null) {
      this.hideSecuritySections(securitiesArray);
      return;
    }
    
    for (let i = 0; i < securitiesArray.length; i++) {
      let securityItem = securitiesArray[i];
      if (securities.includes(securityItem)) {
        this.showSecuritySection(securityItem.toString(), requireSection);
      }
      else {
        this.hideSecuritySection(securityItem.toString());
      }
    }
  }

  private showSecuritySection(sectionName: string, required: boolean) {
    var tab = this.common.getTab("tab_9");
    if (tab == null) {
      console.warn("Tab 'tab_9' not found");
      return;
    }
    var section = tab.sections.get(sectionName)
    if (section == null) {
      console.warn(`Section ${sectionName} not found in tab 'tab_9'`);
      return;
    }
    
    section.setVisible(true);
    
    for (let i = 0; i < section.controls.getLength(); i++) {
      let controlName = section.controls.get(i).getName();
      this.common.setControlRequiredV2(controlName, required);
    }
  }

  private hideSecuritySection(sectionName: string) {
    var tab = this.common.getTab("tab_9");
    if (tab == null) {
      console.warn("Tab 'tab_9' not found");
      return;
    }
    var section = tab.sections.get(sectionName)
    if (section == null) {
      console.warn(`Section ${sectionName} not found in tab 'tab_9'`);
      return;
    }
    
    section.setVisible(false);

    for (let i = 0; i < section.controls.getLength(); i++) {
      let controlName = section.controls.get(i).getName();
      this.common.setControlRequiredV2(controlName, false);
    }
  }

  private hideSecuritySections(sectionNames: number[]) {
    var tab = this.common.getTab("tab_9");
    if (tab == null) {
      console.warn("Tab 'tab_9' not found");
      return;
    }

    for (let i = 0; i < sectionNames.length; i++) {
      let sectionName = sectionNames[i].toString();
      let section = tab.sections.get(sectionName);
      
      section.setVisible(false);

      for (let i = 0; i < section.controls.getLength(); i++) {
        let controlName = section.controls.get(i).getName();
        this.common.setControlRequiredV2(controlName, false);
      }
    }
  }
  
  public populateFields() {
    var additionalReturns = this.common.getAttributeValue('invln_additionalreturns')
    if (additionalReturns) {
      this.common.setAttributeValue('invln_assessedassppi', false)
      this.common.setAttributeValue('invln_finalconclusion', LoanApplicationService.finalConclusionAssetDoesNotMeet)
    } else if (additionalReturns == false) {
      var rateOfInterest = this.common.getAttributeValue('invln_rateofinterest')
      var specialPurposeVehicleProvided = this.common.getAttributeValue('invln_specialpurposevehicleprovided')
      var LTGDVOver80 = this.common.getAttributeValue('invln_ltgdv')
      var lessThan10investedByBorrower = this.common.getAttributeValue('invln_investedbyborrower')
      var projectedProfitMarginLowerThan10 = this.common.getAttributeValue('invln_projectedprofitmargin')
      if (rateOfInterest) {
        this.common.setAttributeValue('invln_assessedassppi', false)
        this.common.setAttributeValue('invln_finalconclusion', LoanApplicationService.finalConclusionAssetDoesNotMeet)
      } else if (rateOfInterest == false) {
        if (specialPurposeVehicleProvided) {
          if (LTGDVOver80 || lessThan10investedByBorrower || projectedProfitMarginLowerThan10) {
            this.common.setAttributeValue('invln_finalconclusion', LoanApplicationService.finalConclusionContactFinancial)
            this.common.setAttributeValue('invln_assessedassppi', null)
          } else if (LTGDVOver80 == false && lessThan10investedByBorrower == false && projectedProfitMarginLowerThan10 == false) {
            this.common.setAttributeValue('invln_assessedassppi', true)
            this.common.setAttributeValue('invln_finalconclusion', LoanApplicationService.finalConclusionAssetDoesMeet)
          }
        } else if (specialPurposeVehicleProvided == false) {
          this.common.setAttributeValue('invln_assessedassppi', true)
          this.common.setAttributeValue('invln_finalconclusion', LoanApplicationService.finalConclusionAssetDoesMeet)
        }
      }
    }
  }

  public setFieldsVisibilityBasedOnAssessedAsSppi() {
    console.log("setFieldsVisibilityBasedOnAssessedAsSppi");
    let assesedAsSppi = this.common.getAttributeValue('invln_additionalreturns');
    let rateofinterest = this.common.getAttributeValue('invln_rateofinterest');
    this.populateFields()
    if (assesedAsSppi == null) {
      this.common.hideControl('invln_rateofinterest', false);
      this.common.hideControl('invln_specialpurposevehicleprovided', false);
      this.common.hideControl('invln_ltgdv', false);
      this.common.hideControl('invln_projectedprofitmargin', false);
      this.common.hideControl('invln_investedbyborrower', false);
      this.common.hideControl('invln_assessedassppi', false);
      this.common.hideControl('invln_finalconclusion', false);
    }

    if (assesedAsSppi) {
      this.common.hideControl('invln_rateofinterest', true);
      this.common.hideControl('invln_specialpurposevehicleprovided', true);
      this.common.hideControl('invln_ltgdv', true);
      this.common.hideControl('invln_projectedprofitmargin', true);
      this.common.hideControl('invln_investedbyborrower', true);
      this.common.hideControl('invln_assessedassppi', false);
      this.common.hideControl('invln_finalconclusion', false);
      this.common.setAttributeValue("invln_rateofinterest", false);
      this.common.setAttributeValue('invln_specialpurposevehicleprovided', false);
      this.common.setAttributeValue('invln_ltgdv', false);
      this.common.setAttributeValue('invln_projectedprofitmargin', false);
      this.common.setAttributeValue('invln_investedbyborrower', false);
    }

    if (assesedAsSppi == false && (rateofinterest == null || rateofinterest == false)) {
      this.common.hideControl('invln_rateofinterest', false);
      this.common.hideControl('invln_specialpurposevehicleprovided', false);
      this.common.hideControl('invln_ltgdv', false);
      this.common.hideControl('invln_projectedprofitmargin', false);
      this.common.hideControl('invln_investedbyborrower', false);
      this.common.hideControl('invln_assessedassppi', false);
      this.common.hideControl('invln_finalconclusion', false);
    }

    if (!assesedAsSppi && rateofinterest) {
      this.common.hideControl('invln_rateofinterest', false);
      this.common.hideControl('invln_specialpurposevehicleprovided', true);
      this.common.hideControl('invln_ltgdv', true);
      this.common.hideControl('invln_projectedprofitmargin', true);
      this.common.hideControl('invln_investedbyborrower', true);
      this.common.hideControl('invln_assessedassppi', false);
      this.common.hideControl('invln_finalconclusion', false);

      this.common.setAttributeValue('invln_specialpurposevehicleprovided', false);
      this.common.setAttributeValue('invln_ltgdv', false);
      this.common.setAttributeValue('invln_projectedprofitmargin', false);
      this.common.setAttributeValue('invln_investedbyborrower', false);
    }
  }

  public openCustomPage() {
    var recordId = this.common.trimBraces(this.common.getCurrentEntityId())
    var pageInput: any = {
      pageType: "custom",
      name: "invln_changeloanapplicationstatus_2a09b",
      recordLogicalName: "invln_loanapplication",
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
