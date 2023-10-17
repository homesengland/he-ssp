import { CommonLib } from '../Common'
import { ExternalStatus } from "../OptionSet"

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
  private static financialAnalysisValue: string = "Please provide a financial summary for the Borrower / contractor."
  private static staticAnalysisValue: string = "Please provide rationale for the proposed CRR."
  private static underSensitivityAnalisysValue: string = "Please provide commentary on sensitivity analyses conducted and highlight any likely stressed scenarios."
  private static summaryValue: string = "Please summarise the output of your Homes England policy assessment exercise."
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

    this.common.setAttributeValue('invln_staticfinancialanalysis', IspService.financialAnalysisValue)
    this.common.setAttributeValue('invln_staticcrranalysis', IspService.staticAnalysisValue)
    this.common.setAttributeValue('invln_undersensitivityanalysissection', IspService.underSensitivityAnalisysValue)

    this.common.setAttributeValue('invln_staticsummary', IspService.summaryValue)

    this.common.setAttributeValue('invln_staticrecommendation', IspService.recommendationValue)
    this.common.setAttributeValue('invln_staticstandardconditionstobewaived', IspService.standardConditionsToBeWaivedValue)
  }
}
