import { CommonLib } from '../Common'
import { ExternalStatus, Securities } from "../OptionSet"

export class IspService {
  common: CommonLib

  private static briefOutlineValue: string = "Please provide a high-level overview of the project and the proposed funding package."
  private static keyRisksSummaryValue: string = "Please highlight any key risks and/or significant challenges in relation to the project or Borrower."
  private static complianceValue: string = "Please outline whether the proposal is compliant with fund criteria and relevant policies. Also indicate whether the Simplified Process is to be utilised."
  private static strategicRationaleValue: string = "Please outline the strategic rationale for the proposal."
  private static borrowingEntityAndRelationshipValue: string = "Please provide details about the borrowing entity, management team and group structure (where applicable). Please ensure commentary covers experience, track record of Borrower/contractor and key concerns/strengths."
  private static underPreviousSchemesTableValue: string = "Please provide commentary on the borrower's previous schemes."
  private static locationAndDevelopmentValue: string = "Please provide a detailed overview of the project including commentary on: location, building programme/phrasing, designs & specifications, underlying assumptions, construction challenges, environmental factors, home types, sales values and local market dynamics."
  private static projectFundamentalsValue: string = "Please provide an overview of the project's fundamentals, including commentary on: Project costs, assessment of contractor & professional team, type of build contracts, planning details, any CIL/S.106/S.278 requirements. Also highlight any key delivery risks and any extraneous due diligence or conditions of support."
  private static overviewValue: string = "Please detail the proposed facility structure, including commentary on: project funding structure, Homes England facility structure (incl. facility term, repayment, any non-standard proposals), facility documentation type. Please also comment on: repayment and exit strategy, other debt/interests, security. Also use this text box to justify any policy exceptions and detail any non-standard assumptions for land value."
  private static monitoringAndControlsValue: string = "*The Borrower is required to submit a monitoring and progress report on a periodic basis to Homes England and Monitoring Surveyor providing, but not limited to, the following information: milestone progress (including delays and proposed mitigation steps), disposal information, costs forecasting and a cashflow.\n" +
  "*Unaudited accounts must be provided within 180 days of the end of each financial year and a compliance certificate (confirming compliance with, inter alia, the financial covenants) and accounts on a periodic basis.\n"+
  "*Homes England will receive progress reports from the monitoring surveyor on a periodic basis and the monitoring surveyor will verify the costs to be funded and all claims for funding (including recycled receipts requests).\n"
  private static financialAnalysisValue: string = "Please provide a financial summary for the Borrower / contractor."
  private static staticAnalysisValue: string = "Please provide rationale for the proposed CRR."
  private static underSensitivityAnalisysValue: string = "Please provide commentary on sensitivity analyses conducted and highlight any likely stressed scenarios."
  private static summaryValue: string = "Please summarise the output of your Homes England policy assessment exercise."
  private static strategicValue: string = "The Strategic Case considers the fit with fund objectives and capital allocation approach set out in the Fund Business Case."
  private static economicValue: string = "The Economic Case considers Value for Money (VfM) and consists of three tests: economy, efficiency and effectiveness."
  private static deliverabilityValue: string = "Deliverability considers how projects perform against commercial, financial and management arrangements with a focus on achievement of outputs delivered."
  private static recommendationValue: string = "Please summarise your recommendation to the Approver."
  private static standardConditionsToBeWaivedValue: string = "Please highlight any Standard Conditions that do not apply to this proposal and the rationale for those exclusion(s)."

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
  }

  public setFieldsAvailabilityOnLoad() {
    var loanApplication = this.common.getLookupValue('invln_loanapplication')
    if (loanApplication != null) {
      Xrm.WebApi.retrieveRecord('invln_loanapplication', loanApplication.id).then(result => {
        if (result.invln_externalstatus == ExternalStatus.sentForApproval) {
          this.common.disableAllFields()
        }
      })
    }
  }

  public setFieldsVisibilityBasedOnSecurity() {
    var loanApplication = this.common.getLookupValue('invln_loanapplication')
    this.hideFirstLegalChargeFields(true);
    this.hideSubsequentChargeFields(true);
    this.hideDebentureFields(true);
    this.hidePersonalGuaranteeFields(true);
    this.hideCostOverrunGuaranteeFields(true);
    this.hideInterestShortfallFields(true);
    this.hideOtherFields(true);
    this.hideParentCompanyGuaranteeFields(true);
    this.hideSubordinatedDeedFields(true);
    this.hideCompletionGuaranteeFields(true);
    if (loanApplication != null) {
      Xrm.WebApi.retrieveRecord('invln_loanapplication', loanApplication.id).then(result => {
        if (result != null && result.invln_securities != null) {
          var securitiesArray : string[] = result.invln_securities.split(",");
          securitiesArray.forEach(element => {
            debugger;
            switch (element) {
              case Securities.debenture.toString():
                this.hideDebentureFields(false);
                break;
              case Securities.firstLegalCharge.toString():
                this.hideFirstLegalChargeFields(false);
                break;
              case Securities.subsequentCharge.toString():
                this.hideSubsequentChargeFields(false);
                break;
              case Securities.personalGuarantee.toString():
                this.hidePersonalGuaranteeFields(false);
                break;
              case Securities.parentCompanyGuarantee.toString():
                this.hideParentCompanyGuaranteeFields(false);
                break;
              case Securities.subordinatedDeed.toString():
                this.hideSubordinatedDeedFields(false);
                break;
              case Securities.costOverrunGuarantee.toString():
                this.hideCostOverrunGuaranteeFields(false);
                break;
              case Securities.completionGuarantee.toString():
                this.hideCompletionGuaranteeFields(false);
                break;
              case Securities.interestShortfall.toString():
                this.hideInterestShortfallFields(false);
                break;
              case Securities.other.toString():
                this.hideOtherFields(false);
                break;
            }
          })
        }
      })
    }
  }

  public setStaticFieldsOnLoad() {
    this.common.setAttributeValue('invln_staticbriefoutline', IspService.briefOutlineValue)
    this.common.setAttributeValue('invln_statickeyriskssummary', IspService.keyRisksSummaryValue)
    this.common.setAttributeValue('invln_staticcompliance', IspService.complianceValue)

    this.common.setAttributeValue('invln_staticstrategicrationale', IspService.strategicRationaleValue)
    this.common.setAttributeValue('invln_staticborrowingentityrelationship', IspService.borrowingEntityAndRelationshipValue)
    this.common.setAttributeValue('invln_underpreviousschemstable', IspService.underPreviousSchemesTableValue)

    this.common.setAttributeValue('invln_staticlocationadndevelopment', IspService.locationAndDevelopmentValue)
    this.common.setAttributeValue('invln_staticprojectfundamentals', IspService.projectFundamentalsValue)

    this.common.setAttributeValue('invln_staticoverview', IspService.overviewValue)
    this.common.setAttributeValue('invln_staticmonitoringcontrols', IspService.monitoringAndControlsValue)

    this.common.setAttributeValue('invln_staticfinancialanalysis', IspService.financialAnalysisValue)
    this.common.setAttributeValue('invln_staticcrranalysis', IspService.staticAnalysisValue)
    this.common.setAttributeValue('invln_undersensitivityanalysissection', IspService.underSensitivityAnalisysValue)

    this.common.setAttributeValue('invln_staticsummary', IspService.summaryValue)
    this.common.setAttributeValue('invln_staticeconomic', IspService.economicValue)
    this.common.setAttributeValue('invln_staticdeliverability', IspService.deliverabilityValue)
    this.common.setAttributeValue('invln_staticstrategic', IspService.strategicValue)

    this.common.setAttributeValue('invln_staticrecommendation', IspService.recommendationValue)
    this.common.setAttributeValue('invln_staticstandardconditionstobewaived', IspService.standardConditionsToBeWaivedValue)
  }

  public setFieldsRequirementBasedOnSendOnApproval() {
    var sendForApproval = this.common.getAttributeValue('invln_sendforapproval') ?? false;
    this.common.setControlRequiredV2('invln_briefoutline', sendForApproval)
    this.common.setControlRequiredV2('invln_datesubmitted', sendForApproval)
    this.common.setControlRequiredV2('invln_kycnarrative', sendForApproval)
    this.common.setControlRequiredV2('invln_fundrecoveryrate', sendForApproval)
    this.common.setControlRequiredV2('invln_approvallevel', sendForApproval)
    this.common.setControlRequiredV2('invln_interestmargin', sendForApproval)
    this.common.setControlRequiredV2('invln_baserate', sendForApproval)
    this.common.setControlRequiredV2('invln_baseratepercent', sendForApproval)
    this.common.setControlRequiredV2('invln_aggregatelimitproposed', sendForApproval)
    this.common.setControlRequiredV2('invln_sppimet', sendForApproval)
    this.common.setControlRequiredV2('invln_expirydate', sendForApproval)
    this.common.setControlRequiredV2('invln_producttype', sendForApproval)
    this.common.setControlRequiredV2('invln_loanprincipalpercent', sendForApproval)
    this.common.setControlRequiredV2('invln_loanprincipalk', sendForApproval)
    this.common.setControlRequiredV2('invln_equitypercent', sendForApproval)
    this.common.setControlRequiredV2('invln_equityk', sendForApproval)
    this.common.setControlRequiredV2('invln_totalinvestmentpercent', sendForApproval)
    this.common.setControlRequiredV2('invln_totalinvestmentk', sendForApproval)
    this.common.setControlRequiredV2('invln_interestandrechargedfeespercent', sendForApproval)
    this.common.setControlRequiredV2('invln_interestandrechargedfeesk', sendForApproval)
    this.common.setControlRequiredV2('invln_totalexposurepercent', sendForApproval)
    this.common.setControlRequiredV2('invln_totalexposurek', sendForApproval)
    this.common.setControlRequiredV2('invln_borrowerequitycashinfrapercent', sendForApproval)
    this.common.setControlRequiredV2('invln_borrowerequitycashinfrak', sendForApproval)
    this.common.setControlRequiredV2('invln_borrowerequitycashconpercent', sendForApproval)
    this.common.setControlRequiredV2('invln_borrowerequitycashconk', sendForApproval)
    this.common.setControlRequiredV2('invln_borrowerequitylandpercent', sendForApproval)
    this.common.setControlRequiredV2('invln_borrowerequitycashinfrak', sendForApproval)
    this.common.setControlRequiredV2('invln_borrowerequitycashconpercent', sendForApproval)
    this.common.setControlRequiredV2('invln_borrowerequitycashconk', sendForApproval)
    this.common.setControlRequiredV2('invln_borrowerequitylandpercent', sendForApproval)
    this.common.setControlRequiredV2('invln_borrowerequitylandk', sendForApproval)
    this.common.setControlRequiredV2('invln_recycledincomepercent', sendForApproval)
    this.common.setControlRequiredV2('invln_recycledincomek', sendForApproval)
    this.common.setControlRequiredV2('invln_otherincomepercent', sendForApproval)
    this.common.setControlRequiredV2('invln_otherincomek', sendForApproval)
    this.common.setControlRequiredV2('invln_otherdebtpercent', sendForApproval)
    this.common.setControlRequiredV2('invln_otherdebtk', sendForApproval)
    this.common.setControlRequiredV2('invln_totalfundspercent', sendForApproval)
    this.common.setControlRequiredV2('invln_totalfundsk', sendForApproval)
    this.common.setControlRequiredV2('invln_landcostpaidpercent', sendForApproval)
    this.common.setControlRequiredV2('invln_landcostpaidk', sendForApproval)
    this.common.setControlRequiredV2('invln_landcostdeferredpercent', sendForApproval)
    this.common.setControlRequiredV2('invln_landcostdeferredk', sendForApproval)
    this.common.setControlRequiredV2('invln_infrastructurecostspercent', sendForApproval)
    this.common.setControlRequiredV2('invln_infrastructurecostsk', sendForApproval)
    this.common.setControlRequiredV2('invln_constructioncostspercent', sendForApproval)
    this.common.setControlRequiredV2('invln_constructioncostsk', sendForApproval)
    this.common.setControlRequiredV2('invln_s106costspercent', sendForApproval)
    this.common.setControlRequiredV2('invln_s106costsk', sendForApproval)
    this.common.setControlRequiredV2('invln_abnormalspercent', sendForApproval)
    this.common.setControlRequiredV2('invln_abnormalsk', sendForApproval)
    this.common.setControlRequiredV2('invln_salesprofessionalpercent', sendForApproval)
    this.common.setControlRequiredV2('invln_salesprofessionalk', sendForApproval)
    this.common.setControlRequiredV2('invln_financepercent', sendForApproval)
    this.common.setControlRequiredV2('invln_financek', sendForApproval)
    this.common.setControlRequiredV2('invln_contingencypercent', sendForApproval)
    this.common.setControlRequiredV2('invln_contingencyk', sendForApproval)
    this.common.setControlRequiredV2('invln_otherpercent', sendForApproval)
    this.common.setControlRequiredV2('invln_otherk', sendForApproval)
    this.common.setControlRequiredV2('invln_totalcostspercent', sendForApproval)
    this.common.setControlRequiredV2('invln_totalcostsk', sendForApproval)
    this.common.setControlRequiredV2('invln_totalnounitstobedevelopedunlocked', sendForApproval)
    this.common.setControlRequiredV2('invln_nounitsdirectlyfundedbyinvestment', sendForApproval)
    this.common.setControlRequiredV2('invln_grossdevelopmentvaluegdv', sendForApproval)
    this.common.setControlRequiredV2('invln_developersoverallprofit', sendForApproval)
    this.common.setControlRequiredV2('invln_returnonequityroe', sendForApproval)
    this.common.setControlRequiredV2('invln_peakfunding', sendForApproval)
    this.common.setControlRequiredV2('invln_loanrecycledincomeotherdebttocostspeak', sendForApproval)
    this.common.setControlRequiredV2('invln_loanrecycledincomeotherdebttogdvpeak', sendForApproval)
    this.common.setControlRequiredV2('invln_loanrecycledincomeotherdebttocostscoven', sendForApproval)
    this.common.setControlRequiredV2('invln_loanrecycledincomeotherdebttogdvcovenan', sendForApproval)
    this.common.setControlRequiredV2('invln_keyriskssummary', sendForApproval)
    this.common.setControlRequiredV2('invln_compliance', sendForApproval)
    this.common.setControlRequiredV2('invln_strategicrationale', sendForApproval)
    this.common.setControlRequiredV2('invln_borrowingentityandrelationship', sendForApproval)
    this.common.setControlRequiredV2('invln_borrowingentityandrelationshipcomments', sendForApproval)
    this.common.setControlRequiredV2('invln_locationanddevelopment', sendForApproval)
    this.common.setControlRequiredV2('invln_projectfundamentals', sendForApproval)
    this.common.setControlRequiredV2('invln_margin', sendForApproval)
    this.common.setControlRequiredV2('invln_securitycover', sendForApproval)
    this.common.setControlRequiredV2('invln_arrangementfee', sendForApproval)
    this.common.setControlRequiredV2('invln_renumeration', sendForApproval)
    this.common.setControlRequiredV2('invln_firstlegalchargedescription', sendForApproval)
    this.common.setControlRequiredV2('invln_subsequentchargedescription', sendForApproval)
    this.common.setControlRequiredV2('invln_debenturedescription', sendForApproval)
    this.common.setControlRequiredV2('invln_personalguaranteedescription', sendForApproval)
    this.common.setControlRequiredV2('invln_parentcompanyguaranteedescription', sendForApproval)
    this.common.setControlRequiredV2('invln_subordinateddeeddescription', sendForApproval)
    this.common.setControlRequiredV2('invln_costoverrunguarantee', sendForApproval)
    this.common.setControlRequiredV2('invln_completionguaranteedescription', sendForApproval)
    this.common.setControlRequiredV2('invln_interestshortfalldescription', sendForApproval)
    this.common.setControlRequiredV2('invln_otherdescription', sendForApproval)
    this.common.setControlRequiredV2('invln_reportfrequency', sendForApproval)
    this.common.setControlRequiredV2('invln_financialsummary', sendForApproval)
    this.common.setControlRequiredV2('invln_crranalysis', sendForApproval)
    this.common.setControlRequiredV2('invln_sensitivityanalysiscomments', sendForApproval)
    this.common.setControlRequiredV2('invln_summary', sendForApproval)
    this.common.setControlRequiredV2('invln_strategicassessmentbanding', sendForApproval)
    this.common.setControlRequiredV2('invln_newhomesbyscaleandtypemeasure', sendForApproval)
    this.common.setControlRequiredV2('invln_encouragingdiversityandinnovation', sendForApproval)
    this.common.setControlRequiredV2('invln_achievementofwiderhousingobjectivesmeasure', sendForApproval)
    this.common.setControlRequiredV2('invln_achievementofwidereconomicobjectivesmeasure', sendForApproval)
    this.common.setControlRequiredV2('invln_vfmassessment', sendForApproval)
    this.common.setControlRequiredV2('invln_economymeasure', sendForApproval)
    this.common.setControlRequiredV2('invln_efficiencymeasure', sendForApproval)
    this.common.setControlRequiredV2('invln_effectivenessmeasure', sendForApproval)
    this.common.setControlRequiredV2('invln_additionalitymeasure', sendForApproval)
    this.common.setControlRequiredV2('invln_totalpublicsectorexpendituremeasure', sendForApproval)
    this.common.setControlRequiredV2('invln_deliverabilityassessment', sendForApproval)
    this.common.setControlRequiredV2('invln_commercialmeasure', sendForApproval)
    this.common.setControlRequiredV2('invln_financialmeasure', sendForApproval)
    this.common.setControlRequiredV2('invln_managementmeasure', sendForApproval)
    this.common.setControlRequiredV2('invln_legalrechargedfees', sendForApproval)
    this.common.setControlRequiredV2('invln_legalnonrechargedfees', sendForApproval)
    this.common.setControlRequiredV2('invln_propertyrechargedfees', sendForApproval)
    this.common.setControlRequiredV2('invln_propertynonrechargedfees', sendForApproval)
    this.common.setControlRequiredV2('invln_monitoringrechargedfees', sendForApproval)
    this.common.setControlRequiredV2('invln_monitoringnonrechargedfees', sendForApproval)
    this.common.setControlRequiredV2('invln_otherrechargedfees', sendForApproval)
    this.common.setControlRequiredV2('invln_othernonrechargedfees', sendForApproval)
    this.common.setControlRequiredV2('invln_totalrechargedfees', sendForApproval)
    this.common.setControlRequiredV2('invln_totalnonrechargedfees', sendForApproval)
    this.common.setControlRequiredV2('invln_recommendation', sendForApproval)
    this.common.setControlRequiredV2('invln_tmname', sendForApproval)
    this.common.setControlRequiredV2('invln_datesentforapproval', sendForApproval)
  }

  private hideFirstLegalChargeFields(isDisabled: boolean) {
    this.common.hideControl('invln_firstlegalchargedescription', isDisabled);
    this.common.hideControl('invln_firstlegalchargemarginedsecurityvalue', isDisabled);
    this.common.hideControl('invln_firstlegalchargevaluek', isDisabled);
  }

  private hideSubsequentChargeFields(isDisabled: boolean) {
    this.common.hideControl('invln_subsequentchargedescription', isDisabled);
    this.common.hideControl('invln_subsequentchargemarginedsecurityvalue', isDisabled);
    this.common.hideControl('invln_subsequentchargevaluek', isDisabled);
  }

  private hideDebentureFields(isDisabled: boolean) {
    this.common.hideControl('invln_debenturedescription', isDisabled);
    this.common.hideControl('invln_debenturemarginedsecurityk', isDisabled);
    this.common.hideControl('invln_debenturevaluek', isDisabled);
  }

  private hidePersonalGuaranteeFields(isDisabled: boolean) {
    this.common.hideControl('invln_personalguaranteedescription', isDisabled);
    this.common.hideControl('invln_personalguaranteemarginedsecurityk', isDisabled);
    this.common.hideControl('invln_personalguaranteevaluek', isDisabled);
  }

  private hideCostOverrunGuaranteeFields(isDisabled: boolean) {
    this.common.hideControl('invln_costoverrunguarantee', isDisabled);
    this.common.hideControl('invln_costoverrunmarginedsecurityk', isDisabled);
    this.common.hideControl('invln_costoverrunvaluek', isDisabled);
  }

  private hideInterestShortfallFields(isDisabled: boolean) {
    this.common.hideControl('invln_interestshortfalldescription', isDisabled);
    this.common.hideControl('invln_interestshortfallmarginedsecurityk', isDisabled);
    this.common.hideControl('invln_interestshortfallvaluek', isDisabled);
  }

  private hideOtherFields(isDisabled: boolean) {
    this.common.hideControl('invln_otherdescription', isDisabled);
    this.common.hideControl('invln_othervaluek', isDisabled);
    this.common.hideControl('invln_othermarginedsecurityk', isDisabled);
  }

  private hideParentCompanyGuaranteeFields(isDisabled: boolean) {
    this.common.hideControl('invln_parentcompanyguaranteedescription', isDisabled);
    this.common.hideControl('invln_parentcompanyguaranteevaluek', isDisabled);
    this.common.hideControl('invln_parentcompanyguaranteemarginedsecurityk', isDisabled);
  }

  private hideSubordinatedDeedFields(isDisabled: boolean) {
    this.common.hideControl('invln_subordinateddeeddescription', isDisabled);
    this.common.hideControl('invln_subordinateddeedvaluek', isDisabled);
    this.common.hideControl('invln_subordinateddeedmarginedsecurityk', isDisabled);
  }

  private hideCompletionGuaranteeFields(isDisabled: boolean) {
    this.common.hideControl('invln_completionguaranteedescription', isDisabled);
    this.common.hideControl('invln_completionguaranteevaluek', isDisabled);
    this.common.hideControl('invln_completionguaranteemarginedsecurityk', isDisabled);
  }

}
