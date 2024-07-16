import { CommonLib } from '../Common'
import { ExternalStatus, Securities } from "../OptionSet"
import * as Collections from 'typescript-collections';
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

  public async mapFieldOnLoad() {
    if (this.common.getFormType() != XrmEnum.FormType.Create)
      return;

    const dictionaryLoans = new Map<string, string>([
      ['invln_projectname', 'invln_projectname'],
      ['invln_submitternew', '_ownerid'],
      ['invln_heregion', 'invln_laregion'],
      ['invln_programmenew', 'invln_programme'],
      ['invln_sppimet', 'invln_assessedassppi'],//  SPPI Met ?
      ['invln_firstlegalchargedescription', 'invln_firstlegalchargedescription'],//First Legal Charge Description
      ['invln_firstlegalchargevaluek', 'invln_firstlegalchargevalue'],//First Legal Charge Value £
      ['invln_firstlegalchargemarginedsecurityvalue', 'invln_firstlegalchargemarginedsecurityvalue'],//  First Legal Charge Margined Security Value
      ['invln_subsequentchargedescription', 'invln_subsequentchargedescription'],//Subsequent Charge Description
      ['invln_subsequentchargevaluek', 'invln_subsequentchargevalue'],//Subsequent Charge Value £
      ['invln_subsequentchargemarginedsecurityvalue', 'invln_subsequentchargemarginedsecurityvalue'],//  Subsequent Charge Margined Security Value
      ['invln_debenturedescription', 'invln_debenturedescription'],//Debenture Description
      ['invln_debenturevaluek', 'invln_debenturevalue'],//Debenture Value £
      ['invln_debenturemarginedsecurityk', 'invln_debenturemarginedsecurityvalue'],//  Debenture Margined Security Value
      ['invln_personalguaranteedescription', 'invln_personalguaranteedescription'],//Personal Guarantee Description
      ['invln_personalguaranteevaluek', 'invln_personalguaranteevalue'],//Personal Guarantee Value £
      ['invln_personalguaranteemarginedsecurityk', 'invln_personalguaranteemarginedsecurityvalue'],//  Personal Guarantee Margined Security Value
      ['invln_parentcompanyguaranteedescription', 'invln_parentcompanyguaranteedescription'],//Parent Company Guarantee Description
      ['invln_parentcompanyguaranteevaluek', 'invln_parentcompanyguaranteevalue'],//Parent Company Guarantee Value £
      ['invln_parentcompanyguaranteemarginedsecurityk', 'invln_parentcompanyguaranteemarginedsecurityvalue'],//  Parent Company Guarantee Margined Security Value
      ['invln_subordinateddeeddescription', 'invln_subordinateddeeddescription'],//Subordinated Deed Description
      ['invln_subordinateddeedvaluek', 'invln_subordinateddeedvalue'],//Subordinated Deed Value £
      ['invln_subordinateddeedmarginedsecurityk', 'invln_subordinateddeedmarginedsecurityvalue'],//  Subordinated Deed Margined Security Value
      ['invln_costoverrunguarantee', 'invln_costoverrunguarantee'],//Cost Overrun Guarantee
      ['invln_costoverrunvaluek', 'invln_costoverrunvalue'],//Cost Overrun Value £
      ['invln_costoverrunmarginedsecurityk', 'invln_costoverrunmarginedsecurityvalue'],//  Cost Overrun Margined Security Value
      ['invln_completionguaranteedescription', 'invln_completionguaranteedescription'],//Completion Guarantee Description
      ['invln_completionguaranteevaluek', 'invln_completionguaranteevalue'],//Completion Guarantee Value £
      ['invln_completionguaranteemarginedsecurityk', 'invln_completionguaranteemarginedsecurityvalue'],//  Completion Guarantee Margined Security Value
      ['invln_interestshortfalldescription', 'invln_interestshortfalldescription'],//Interest Shortfall Description
      ['invln_interestshortfallvaluek', 'invln_interestshortfallvalue'],//Interest Shortfall Value £
      ['invln_interestshortfallmarginedsecurityk', 'invln_interestshortfallmarginedsecurityvalue'],//  Interest Shortfall Margined Security Value
      ['invln_collateralwarrantydescription', 'invln_collateralwarrantydescription'],//Collateral Warranty Description
      ['invln_collateralwarrantyvalue', 'invln_collateralwarrantyvalue'],//Collateral Warranty Value £
      ['invln_collateralwarrantymarginatedsecurityvalue', 'invln_collateralwarrantymarginatedsecurityvalue'],//  Collateral Warranty Marginated Security Value
      ['invln_otherdescription', 'invln_otherdescription'],//Other Description
      ['invln_othervaluek', 'invln_othervalue'],//Other Value £
      ['invln_othermarginedsecurityk', 'invln_othermarginedsecurityvalue'],//  Other Margined Security Value
      ['ownerid', '_ownerid'],
      ['invln_organisation', '_invln_account'],
      ['invln_borrowername', '_invln_contact'],
    ]);

    const dictionaryAccount = new Map<string, string>([
      ['invln_crr', 'invln_currentcrr'],
      ['invln_kycrating', 'invln_rating']
    ]);

    const dictionaryCashFlow = new Map<string, string>([
      ['invln_loanprincipal', 'invln_loanprincipal'],//Loan(Principal)
      ['invln_interestmargin', 'invln_interestmargin'],//Interest Margin Number
      ['invln_interestmarginpercent', 'invln_interestmarginpercent'],//Interest Margin %
      ['invln_interestmarginvalue', 'invln_interestvalue'], //Interest Value
      ['invln_baseratenumber', 'invln_baseratenumber'],//Base Rate Number
      ['invln_baseratepercent', 'invln_baseratepercent'],//Base Rate %
      ['invln_loanprincipalpercent', 'invln_loanprincipalpercent'],//Loan(Principal) %
      ['invln_interestandfeespercentage', 'invln_interestandfeespercent'],//Interest and Fees %
      ['invln_interestandfees', 'invln_interestandfees'],//Interest and Fees £
      ['invln_nonrechargedfeespercentage', 'invln_nonrechargedfeespercent'],//Non Recharged Fees %
      ['invln_nonrechargedfees', 'invln_nonrechargedfees'],//Non Recharged Fees £
      ['invln_totalexposurepercent', 'invln_totalexposurepercent'],//Total Exposure %
      ['invln_totalexposurek', 'invln_totalexposure'],//Total Exposure £
      ['invln_borrowerequitycashpercentage', 'invln_borrowerequitycashpercent'],//Borrower Equity Cash %
      ['invln_borrowerequitycash', 'invln_borrowerequitycash'],//Borrower Equity Cash £
      ['invln_borrowerequitylandpercent', 'invln_borrowerequitylandpercent'],//Borrower Equity Land %
      ['invln_borrowerequitylandk', 'invln_borrowerequityland'],//Borrower Equity Land £
      ['invln_recycledincomepercent', 'invln_recycledincomepercent'],//Recycled Income %
      ['invln_recycledincomek', 'invln_recycledincome'],//Recycled Income £
      ['invln_otherdebtpercent', 'invln_otherdebtpercent'],//      Other Debt %
      ['invln_otherdebtk', 'invln_otherdebt'],//    Other Debt £
      ['invln_totalfundspercent', 'invln_totalfundspercent'],//      Total Funds %
      ['invln_totalfundsk', 'invln_totalfunds'],//Total Funds £
      ['invln_landcostpaidpercent', 'invln_landcostpaidpercent'],//      Land Cost - Paid %
      ['invln_landcostpaidk', 'invln_landcostpaid'],//      Land Cost - Paid £
      ['invln_landcostdeferredpercent', 'invln_landcostdeferredpercent'],//      Land Cost - Deferred %
      ['invln_landcostdeferredk', 'invln_landcostdeferred'],//      Land Cost - Deferred £
      ['invln_landvaluationupliftpercentage', 'invln_landvaluationupliftpercent'],//     Land Valuation Uplift %
      ['invln_landvaluationuplift', 'invln_landvaluationuplift'],//    Land Valuation Uplift £
      ['invln_infrastructurecostspercent', 'invln_infrastructurecostspercent'],//      Infrastructure Costs %
      ['invln_infrastructurecostsk', 'invln_infrastructurecosts'],//    Infrastructure Costs £
      ['invln_constructioncostsk', 'invln_constructioncosts'],//   Construction Costs £
      ['invln_constructioncostspercent', 'invln_constructioncostspercent'],//    Construction Costs %
      ['invln_prelimcostspercentage', 'invln_prelimcostspercent'],//      Prelim Costs %
      ['invln_prelimcosts', 'invln_prelimcosts'],//    Prelim Costs £
      ['invln_s106costspercent', 'invln_s106costspercent'],//      S106 Costs %
      ['invln_s106costsk', 'invln_s106costs'],//    S106 Costs £
      ['invln_abnormalspercent', 'invln_abnormalspercent'],//      Abnormals %
      ['invln_abnormalsk', 'invln_abnormals'],//      Abnormals £
      ['invln_salescostspercentage', 'invln_salescostspercent'],//      Sales Costs %
      ['invln_salescosts', 'invln_salescosts'],//    Sales Costs £
      ['invln_professionalfeespercentage', 'invln_professionalfeespercent'],//      Professional Fees %
      ['invln_professionalfees', 'invln_professionalfees'],//    Professional Fees £
      ['invln_financepercent', 'invln_financepercent'],//      Finance %
      ['invln_financek', 'invln_finance'],//      Finance £
      ['invln_contingencypercent', 'invln_contingencypercent'],//      Contingency %
      ['invln_contingencyk', 'invln_contingency'],//      Contingency £
      ['invln_otherpercent', 'invln_othercostspercent'],//      Other Costs %
      ['invln_otherk', 'invln_othercosts'],//    Other Costs £
      ['invln_totalcostspercent', 'invln_totalcostspercent'],//      Total Costs %
      ['invln_totalcostsk', 'invln_totalcosts'],//    Total Costs £
      ['invln_totalnounitstobedevelopedunlocked', 'invln_totalnumberofhomes'],//      Total No.Units to be Developed / Unlocked
      ['invln_grossdevelopmentvaluegdv', 'invln_grossdevelopmentvalue'],//Gross Development Value(GDV)
      ['invln_developersoverallprofit', 'invln_developersoverallprofitvalue'],//Developer's Overall Profit
      ['invln_returnonequityroe', 'invln_returnonequityroe'],//Return on Equity(ROE)
      ['invln_peakfunding', 'invln_peakfunding'],//Peak Funding
      ['invln_peakfundingdate', 'invln_peakfundingdate'],//Peak Funding Date
      ['invln_loanrecycledincomeotherdebttocostspeak', 'invln_loanrecycledincomeotherdebttocostspeak'],//Loan + Recycled Income + Other Debt to Costs Peak
      ['invln_loanrecycledincomeotherdebttocostsdate', 'invln_loanrecycledincomeotherdebttocostdate'],//Loan + Recycled Income + Other Debt to Cost Date
      ['invln_loanrecycledincomeotherdebttogdvpeak', 'invln_loanotherdebttogdvpeak'],//Loan + Other Debt to GDV Peak
      ['invln_loanotherdebttogdvpeakdate', 'invln_loanotherdebttogdvdate'],//Loan + Other Debt to GDV Date
      ['invln_arrangementfee', 'invln_arrangementfee'],//Arrangement Fee
      ['invln_arrangementfeevalue', 'invln_arrangementfeevalue'], //Arrangement Fee Value
    ])

    let loanApplication = this.common.getLookupValue('invln_loanapplication')
    let accountId = null;
    if (loanApplication != null) {
      Xrm.WebApi.retrieveRecord('invln_loanapplication', loanApplication.id).then(result => {
        for (const [key, value] of dictionaryLoans) {
          try {
            if (value.startsWith('_')) {
              this.common.setLookUpValue(key,
                result[value + "_value@Microsoft.Dynamics.CRM.lookuplogicalname"],
                result[value + "_value"],
                result[value + "_value@OData.Community.Display.V1.FormattedValue"]
              )
            } else {
              this.common.setAttributeValue(key, result[value]);
            }
          } catch (e) {
            console.log(e);
          }
        }

        console.log(result['invln_securities']);
        let values = result['invln_securities'].split(',')
        let osv = values.map(function (x) {
          return parseInt(x);
        })
        this.common.setAttributeValue('invln_securities', osv);
      })
    };
    if (loanApplication != null) {
      console.log(1);
      let loans = await Xrm.WebApi.retrieveRecord('invln_loanapplication', loanApplication.id)
      console.log(loans['_invln_account_value']);
      if (loans['_invln_account_value'] != null) {
        console.log(3);
        Xrm.WebApi.retrieveRecord('account', loans['_invln_account_value']).then(result => {
          for (const [key, value] of dictionaryAccount) {
            try {
              this.common.setAttributeValue(key, result[value]);
            } catch (e) {
              console.log(e);
            }
          }
        })
      }
      let cashflowSubmition = await Xrm.WebApi.retrieveMultipleRecords("invln_cashflowsubmission",
        "?$filter=(_invln_loanapplication_value eq " + loanApplication.id + ")&$orderby=invln_dateagreed desc&$top=1")
      if (cashflowSubmition != null) {
        for (const [key, value] of dictionaryCashFlow) {
          try {

            this.common.setAttributeValue(key, cashflowSubmition[value]);
          } catch (e) {
            console.log(e);
          }
        }
      }
    }
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
