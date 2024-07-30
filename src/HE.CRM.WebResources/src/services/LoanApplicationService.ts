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

    if (status == LoanAppInternalStatus.Draft
      || status == LoanAppInternalStatus.ApplicationSubmitted) {
      this.common.setControlRequiredV2('invln_securities', false);
    } else {
      this.common.setControlRequiredV2('invln_securities', true);
    }

    this.common.setControlRequiredV2('invln_customsecurity', false)
    this.common.setControlRequiredV2('invln_debenturedescription', false);
    this.common.setControlRequiredV2('invln_debenturevalue', false);
    this.common.setControlRequiredV2('invln_debenturemarginedsecurityvalue', false);
    this.common.setControlRequiredV2('invln_firstlegalchargedescription', false);
    this.common.setControlRequiredV2('invln_firstlegalchargevalue', false);
    this.common.setControlRequiredV2('invln_firstlegalchargemarginedsecurityvalue', false);
    this.common.setControlRequiredV2('invln_subsequentchargedescription', false);
    this.common.setControlRequiredV2('invln_subsequentchargevalue', false);
    this.common.setControlRequiredV2('invln_subsequentchargemarginedsecurityvalue', false);
    this.common.setControlRequiredV2('invln_personalguaranteedescription', false);
    this.common.setControlRequiredV2('invln_personalguaranteevalue', false);
    this.common.setControlRequiredV2('invln_personalguaranteemarginedsecurityvalue', false);
    this.common.setControlRequiredV2('invln_parentcompanyguaranteedescription', false);
    this.common.setControlRequiredV2('invln_parentcompanyguaranteevalue', false);
    this.common.setControlRequiredV2('invln_parentcompanyguaranteemarginedsecurityvalue', false);
    this.common.setControlRequiredV2('invln_subordinateddeeddescription', false);
    this.common.setControlRequiredV2('invln_subordinateddeedvalue', false);
    this.common.setControlRequiredV2('invln_subordinateddeedmarginedsecurityvalue', false);
    this.common.setControlRequiredV2('invln_costoverrunguarantee', false);
    this.common.setControlRequiredV2('invln_costoverrunvalue', false);
    this.common.setControlRequiredV2('invln_costoverrunmarginedsecurityvalue', false);
    this.common.setControlRequiredV2('invln_completionguaranteedescription', false);
    this.common.setControlRequiredV2('invln_completionguaranteevalue', false);
    this.common.setControlRequiredV2('invln_completionguaranteemarginedsecurityvalue', false);
    this.common.setControlRequiredV2('invln_interestshortfalldescription', false);
    this.common.setControlRequiredV2('invln_interestshortfallvalue', false);
    this.common.setControlRequiredV2('invln_interestshortfallmarginedsecurityvalue', false);
    this.common.setControlRequiredV2('invln_otherdescription', false);
    this.common.setControlRequiredV2('invln_othervalue', false);
    this.common.setControlRequiredV2('invln_othermarginedsecurityvalue', false);
    this.common.setControlRequiredV2('invln_collateralwarrantydescription', false);
    this.common.setControlRequiredV2('invln_collateralwarrantyvalue', false);
    this.common.setControlRequiredV2('invln_collateralwarrantymarginatedsecurityvalue', false);

    this.common.hideControl('invln_customsecurity', true)
    this.common.hideControl('invln_debenturedescription', true);
    this.common.hideControl('invln_debenturevalue', true);
    this.common.hideControl('invln_debenturemarginedsecurityvalue', true);
    this.common.hideControl('invln_firstlegalchargedescription', true);
    this.common.hideControl('invln_firstlegalchargevalue', true);
    this.common.hideControl('invln_firstlegalchargemarginedsecurityvalue', true);
    this.common.hideControl('invln_subsequentchargedescription', true);
    this.common.hideControl('invln_subsequentchargevalue', true);
    this.common.hideControl('invln_subsequentchargemarginedsecurityvalue', true);
    this.common.hideControl('invln_personalguaranteedescription', true);
    this.common.hideControl('invln_personalguaranteevalue', true);
    this.common.hideControl('invln_personalguaranteemarginedsecurityvalue', true);
    this.common.hideControl('invln_parentcompanyguaranteedescription', true);
    this.common.hideControl('invln_parentcompanyguaranteevalue', true);
    this.common.hideControl('invln_parentcompanyguaranteemarginedsecurityvalue', true);
    this.common.hideControl('invln_subordinateddeeddescription', true);
    this.common.hideControl('invln_subordinateddeedvalue', true);
    this.common.hideControl('invln_subordinateddeedmarginedsecurityvalue', true);
    this.common.hideControl('invln_costoverrunguarantee', true);
    this.common.hideControl('invln_costoverrunvalue', true);
    this.common.hideControl('invln_costoverrunmarginedsecurityvalue', true);
    this.common.hideControl('invln_completionguaranteedescription', true);
    this.common.hideControl('invln_completionguaranteevalue', true);
    this.common.hideControl('invln_completionguaranteemarginedsecurityvalue', true);
    this.common.hideControl('invln_interestshortfalldescription', true);
    this.common.hideControl('invln_interestshortfallvalue', true);
    this.common.hideControl('invln_interestshortfallmarginedsecurityvalue', true);
    this.common.hideControl('invln_otherdescription', true);
    this.common.hideControl('invln_othervalue', true);
    this.common.hideControl('invln_othermarginedsecurityvalue', true);
    this.common.hideControl('invln_collateralwarrantydescription', true);
    this.common.hideControl('invln_collateralwarrantyvalue', true);
    this.common.hideControl('invln_collateralwarrantymarginatedsecurityvalue', true);

    if (status == LoanAppInternalStatus.ReferredBacktoApplicant
      || status == LoanAppInternalStatus.UnderReview
      || status == LoanAppInternalStatus.SentforApproval
      || status == LoanAppInternalStatus.NotApproved
      || status == LoanAppInternalStatus.ApprovedSubjecttoDueDiligence
      || status == LoanAppInternalStatus.ApplicationDeclined
      || status == LoanAppInternalStatus.InDueDiligence
      || status == LoanAppInternalStatus.SentforPreCompleteApproval
      || status == LoanAppInternalStatus.ApprovedSubjectToContract
      || status == LoanAppInternalStatus.AwaitingCPSatisfaction
      || status == LoanAppInternalStatus.CPsSatisfied
      || status == LoanAppInternalStatus.LoanAvailable
      || status == LoanAppInternalStatus.CashflowRequested
      || status == LoanAppInternalStatus.CashflowUnderReview) {
      var securities: any = this.common.getAttributeValue('invln_securities')
      if (securities != null) {
        if (securities.includes(Securities.debenture)) {
          this.common.hideControl('invln_debenturedescription', false)
          this.common.hideControl('invln_debenturevalue', false)
          this.common.hideControl('invln_debenturemarginedsecurityvalue', false)

          this.common.setControlRequiredV2('invln_debenturedescription', true);
          this.common.setControlRequiredV2('invln_debenturevalue', true);
          this.common.setControlRequiredV2('invln_debenturemarginedsecurityvalue', true);
        }
        if (securities.includes(Securities.firstLegalCharge)) {
          this.common.hideControl('invln_firstlegalchargedescription', false)
          this.common.hideControl('invln_firstlegalchargevalue', false)
          this.common.hideControl('invln_firstlegalchargemarginedsecurityvalue', false)

          this.common.setControlRequiredV2('invln_firstlegalchargedescription', true);
          this.common.setControlRequiredV2('invln_firstlegalchargevalue', true);
          this.common.setControlRequiredV2('invln_firstlegalchargemarginedsecurityvalue', true);
        }
        if (securities.includes(Securities.subsequentCharge)) {
          this.common.hideControl('invln_subsequentchargedescription', false)
          this.common.hideControl('invln_subsequentchargevalue', false)
          this.common.hideControl('invln_subsequentchargemarginedsecurityvalue', false)

          this.common.setControlRequiredV2('invln_subsequentchargedescription', true);
          this.common.setControlRequiredV2('invln_subsequentchargevalue', true);
          this.common.setControlRequiredV2('invln_subsequentchargemarginedsecurityvalue', true);
        }
        if (securities.includes(Securities.personalGuarantee)) {
          this.common.hideControl('invln_personalguaranteedescription', false)
          this.common.hideControl('invln_personalguaranteevalue', false)
          this.common.hideControl('invln_personalguaranteemarginedsecurityvalue', false)

          this.common.setControlRequiredV2('invln_personalguaranteedescription', true);
          this.common.setControlRequiredV2('invln_personalguaranteevalue', true);
          this.common.setControlRequiredV2('invln_personalguaranteemarginedsecurityvalue', true);
        }
        if (securities.includes(Securities.parentCompanyGuarantee)) {
          this.common.hideControl('invln_parentcompanyguaranteedescription', false)
          this.common.hideControl('invln_parentcompanyguaranteevalue', false)
          this.common.hideControl('invln_parentcompanyguaranteemarginedsecurityvalue', false)

          this.common.setControlRequiredV2('invln_parentcompanyguaranteedescription', true);
          this.common.setControlRequiredV2('invln_parentcompanyguaranteevalue', true);
          this.common.setControlRequiredV2('invln_parentcompanyguaranteemarginedsecurityvalue', true);
        }
        if (securities.includes(Securities.subordinatedDeed)) {
          this.common.hideControl('invln_subordinateddeeddescription', false)
          this.common.hideControl('invln_subordinateddeedvalue', false)
          this.common.hideControl('invln_subordinateddeedmarginedsecurityvalue', false)

          this.common.setControlRequiredV2('invln_subordinateddeeddescription', true);
          this.common.setControlRequiredV2('invln_subordinateddeedvalue', true);
          this.common.setControlRequiredV2('invln_subordinateddeedmarginedsecurityvalue', true);
        }
        if (securities.includes(Securities.costOverrunGuarantee)) {
          this.common.hideControl('invln_costoverrunguarantee', false)
          this.common.hideControl('invln_costoverrunvalue', false)
          this.common.hideControl('invln_costoverrunmarginedsecurityvalue', false)

          this.common.setControlRequiredV2('invln_costoverrunguarantee', true);
          this.common.setControlRequiredV2('invln_costoverrunvalue', true);
          this.common.setControlRequiredV2('invln_costoverrunmarginedsecurityvalue', true);
        }
        if (securities.includes(Securities.completionGuarantee)) {
          this.common.hideControl('invln_completionguaranteedescription', false)
          this.common.hideControl('invln_completionguaranteevalue', false)
          this.common.hideControl('invln_completionguaranteemarginedsecurityvalue', false)

          this.common.setControlRequiredV2('invln_completionguaranteedescription', true);
          this.common.setControlRequiredV2('invln_completionguaranteevalue', true);
          this.common.setControlRequiredV2('invln_completionguaranteemarginedsecurityvalue', true);
        }
        if (securities.includes(Securities.interestShortfall)) {
          this.common.hideControl('invln_interestshortfalldescription', false)
          this.common.hideControl('invln_interestshortfallvalue', false)
          this.common.hideControl('invln_interestshortfallmarginedsecurityvalue', false)

          this.common.setControlRequiredV2('invln_interestshortfalldescription', true);
          this.common.setControlRequiredV2('invln_interestshortfallvalue', true);
          this.common.setControlRequiredV2('invln_interestshortfallmarginedsecurityvalue', true);
        }
        if (securities.includes(Securities.other)) {
          this.common.hideControl('invln_customsecurity', false)
          this.common.hideControl('invln_otherdescription', false)
          this.common.hideControl('invln_othervalue', false)
          this.common.hideControl('invln_othermarginedsecurityvalue', false)

          this.common.setControlRequiredV2('invln_customsecurity', true)
          this.common.setControlRequiredV2('invln_otherdescription', true);
          this.common.setControlRequiredV2('invln_othervalue', true);
          this.common.setControlRequiredV2('invln_othermarginedsecurityvalue', true);
        }
        if (securities.includes(Securities.collateralWarranty)) {
          this.common.hideControl('invln_collateralwarrantydescription', false)
          this.common.hideControl('invln_collateralwarrantyvalue', false)
          this.common.hideControl('invln_collateralwarrantymarginatedsecurityvalue', false)

          this.common.setControlRequiredV2('invln_collateralwarrantydescription', true);
          this.common.setControlRequiredV2('invln_collateralwarrantyvalue', true);
          this.common.setControlRequiredV2('invln_collateralwarrantymarginatedsecurityvalue', true);
        }
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
