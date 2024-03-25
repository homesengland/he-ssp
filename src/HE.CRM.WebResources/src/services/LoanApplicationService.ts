import { CommonLib } from '../Common'
import { Securities } from "../OptionSet"

export class LoanApplicationService {
  common: CommonLib

  private static readonly finalConclusionAssetDoesNotMeet: string = "The Asset does not meet the criteria to be assessed as SPPI and therefore will be classified as a Fair Value Through Profit or Loss (FVTPL) asset. Please submit assessment to IAT@homesengland.gov.uk"
  private static readonly finalConclusionContactFinancial: string = "Contact Financial Accounts Team. Submit assessment to IAT@homesengland.gov.uk"
  private static readonly finalConclusionAssetDoesMeet: string = "The Asset does meet the criteria to be assessed as SPPI and therefore will be classified as an Amortised Cost asset"

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
  }

  public setFieldsVisibilityBasedOnSecurities() {
    var securities: any = this.common.getAttributeValue('invln_securities')
    if (securities != null) {
      if (securities.includes(Securities.other)) {
        this.common.hideControl('invln_customsecurity', false)
        this.common.setControlRequiredV2('invln_customsecurity', true)
      } else {
        this.common.hideControl('invln_customsecurity', true)
        this.common.setControlRequiredV2('invln_customsecurity', false)
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
