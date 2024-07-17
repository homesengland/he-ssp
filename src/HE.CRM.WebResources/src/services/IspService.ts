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
  private static monitoringAndControlsValue: string = "The Borrower is required to submit a monitoring and progress report on a periodic basis to Homes England and Monitoring Surveyor providing, but not limited to, the following information: milestone progress (including delays and proposed mitigation steps), disposal information, costs forecasting and a cashflow.\n" +
    "Unaudited accounts must be provided within 180 days of the end of each financial year and a compliance certificate (confirming compliance with, inter alia, the financial covenants) and accounts on a periodic basis.\n" +
    "Homes England will receive progress reports from the monitoring surveyor on a periodic basis and the monitoring surveyor will verify the costs to be funded and all claims for funding (including recycled receipts requests).\n"
  private static financialAnalysisValue: string = "Please provide a financial summary for the Borrower / contractor."
  private static staticAnalysisValue: string = "Please provide rationale for the proposed CRR."
  private static underSensitivityAnalisysValue: string = "Please provide commentary on sensitivity analyses conducted and highlight any likely stressed scenarios."
  private static summaryValue: string = "Please summarise the output of your Homes England policy assessment exercise."
  private static strategicValue: string = "The Strategic Case considers the fit with fund objectives and capital allocation approach set out in the Fund Business Case."
  private static economicValue: string = "The Economic Case considers Value for Money (VfM) and consists of three tests: economy, efficiency and effectiveness."
  private static deliverabilityValue: string = "Deliverability considers how projects perform against commercial, financial and management arrangements with a focus on achievement of outputs delivered."
  private static recommendationValue: string = "Please summarise your recommendation to the Approver."
  private static standardConditionsToBeWaivedValue: string = "Please highlight any Standard Conditions that do not apply to this proposal and the rationale for those exclusion(s)."
  private static lessThan10MInformationValue: string = "You will not be able to send this ISP for approval until the DES and HoF reviews have taken place. It is policy that the following review / approve this ISP: Risk, CRO Delegated Authority";
  private static moreThan10MLessThan50InformationValue: string = "You will not be able to send this ISP for approval until the DES and HoF reviews have taken place. It is policy that the following review / approve this ISP: Risk, CRO, IPE";
  private static statictextaboveplots = "Please use this section to give an overview of the homes being delivered."
  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
  }

  public showNotificationOnApprovalTab() {
    var displayState = this.common.getTab('Approval').getDisplayState();
    if (displayState == "expanded") {
      var ispValue = this.common.getAttributeValue("invln_loanprincipal");
      if (ispValue != null && ispValue <= 10000000) {
        this.common.setFormNotification(IspService.lessThan10MInformationValue, XrmEnum.FormNotificationLevel.Info, "lessThan10MInformation");
      }
      else if (ispValue != null && ispValue <= 50000000) {
        this.common.setFormNotification(IspService.moreThan10MLessThan50InformationValue, XrmEnum.FormNotificationLevel.Info, "moreThan10MLessThan50Information");
      }
    }
    else {
      this.common.clearFormNotification("lessThan10MInformation");
      this.common.clearFormNotification("moreThan10MLessThan50Information");
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
          var securitiesArray: string[] = result.invln_securities.split(",");
          securitiesArray.forEach(element => {
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
    this.common.setAttributeValue('invln_statictextaboveplots', IspService.statictextaboveplots)
  }

  public setFieldsRequirementBasedOnSendOnApproval() {
    var sendForApproval = this.common.getAttributeValue('invln_sendforapproval') ?? false;
    let list: Array<string>;
    list = ['invln_briefoutline', 'invln_kycnarrative', 'invln_fundrecoveryrate', 'invln_approvallevelnew', 'invln_baserate', 'invln_aggregatelimitproposed',
      'invln_expirydate', 'invln_nounitsdirectlyfundedbyinvestment', 'invln_loanrecycledincomeotherdebttocostscoven', 'invln_keyriskssummary',
      'invln_compliance', 'invln_strategicrationale', 'invln_borrowingentityandrelationship', 'invln_borrowingentityandrelationshipcomments',
      'invln_locationanddevelopment', 'invln_projectfundamentals', 'invln_securitycover', 'invln_renumeration', 'invln_reportfrequency',
      'invln_financialsummary', 'invln_crranalysis', 'invln_sensitivityanalysiscomments', 'invln_summary', 'invln_strategicassessmentbanding',
      'invln_newhomesbyscaleandtypemeasure', 'invln_achievementofwiderhousingobjectivesmeasure', 'invln_achievementofwidereconomicobjectivesmeasure',
      'invln_vfmassessment', 'invln_economymeasure', 'invln_efficiencymeasure', 'invln_effectivenessmeasure', 'invln_additionalitymeasure', 'invln_totalpublicsectorexpendituremeasure',
      'invln_deliverabilityassessment', 'invln_commercialmeasure', 'invln_financialmeasure', 'invln_managementmeasure', 'invln_legalrechargedfees',
      'invln_legalnonrechargedfees', 'invln_propertyrechargedfees', 'invln_propertynonrechargedfees', 'invln_monitoringrechargedfees', 'invln_otherrechargedfees',
      'invln_othernonrechargedfees', 'invln_totalrechargedfees', 'invln_totalnonrechargedfees', 'invln_recommendation', 'invln_tmname',
      'invln_sensitivityanalysistable', 'invln_encouragingdiversityandinnovation'
    ];

    list.forEach(x => { this.common.setControlRequiredV2(x, sendForApproval) })

    this.common.setControlRequiredV2('invln_datesubmitted', sendForApproval)
    this.common.setControlRequiredV2('invln_interestmargin', sendForApproval)
    this.common.setControlRequiredV2('invln_baseratepercent', sendForApproval)
    this.common.setControlRequiredV2('invln_sppimet', sendForApproval)
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
    this.common.setControlRequiredV2('invln_grossdevelopmentvaluegdv', sendForApproval)
    this.common.setControlRequiredV2('invln_developersoverallprofit', sendForApproval)
    this.common.setControlRequiredV2('invln_returnonequityroe', sendForApproval)
    this.common.setControlRequiredV2('invln_peakfunding', sendForApproval)
    this.common.setControlRequiredV2('invln_loanrecycledincomeotherdebttocostspeak', sendForApproval)
    this.common.setControlRequiredV2('invln_loanrecycledincomeotherdebttogdvpeak', sendForApproval)
    this.common.setControlRequiredV2('invln_loanrecycledincomeotherdebttogdvcovenan', sendForApproval)
    this.common.setControlRequiredV2('invln_margin', sendForApproval)
    this.common.setControlRequiredV2('invln_arrangementfee', sendForApproval)
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
    this.common.setControlRequiredV2('invln_encouragingdiversityandinnovation', sendForApproval)
    this.common.setControlRequiredV2('invln_monitoringnonrechargedfees', sendForApproval)
    this.common.setControlRequiredV2('invln_datesentforapproval', sendForApproval)
  }

  public async blockFieldsForLoansReviewer() {
    let isReviewer = false;
    let isTrasactionMenager = false;
    let reviewerRoleName = "[Loans] Reviewer";
    let transactionMenagerRoleName = "[Loans] Transaction manager";

    let securityRoles = this.common.getUserSecurityRoles();
    securityRoles?.forEach(function (value) {
      if (value.name == reviewerRoleName) isReviewer = true;
      if (value.name == transactionMenagerRoleName) isTrasactionMenager = true
      console.log(value.name);
    });

    let appLookup = <any>this.common.getAttributeValue("invln_loanapplication");
    let application = await Xrm.WebApi.retrieveRecord("invln_loanapplication", appLookup[0].id);
    console.log(application.invln_externalstatus)

    if (isReviewer && application.invln_externalstatus != 858110010) { //send for aproval
      this.common.disableAllFields();
    };

    var ispStatus = this.common.getAttributeValue("invln_approvalstatus");

    if ((isReviewer || isTrasactionMenager) && application.invln_externalstatus != 858110010 && ispStatus == 858110001) {
      this.common.disableAllFields();
    };
    console.log(ispStatus);
  };

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
