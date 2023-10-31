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
    var securities :any = this.common.getAttributeValue('invln_securities')
    debugger;
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

    } else if(additionalReturns == false){
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
    var assesedAsSppi = this.common.getAttributeValue('invln_assessedassppi')
    this.common.hideControl('invln_additionalreturns', false)
    this.common.hideControl('invln_rateofinterest', false)
    this.common.hideControl('invln_specialpurposevehicleprovided', false)
    if (assesedAsSppi == false) {
      this.common.hideControl('invln_additionalreturns', true)
      this.common.hideControl('invln_rateofinterest', true)
      this.common.hideControl('invln_specialpurposevehicleprovided', true)
    }
  }

}
