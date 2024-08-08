import { ClaimService } from '../services/ClaimService'
import { CommonLib } from '../Common'

export class Claim {
  private common: CommonLib
  private claimService: ClaimService

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
    this.claimService = new ClaimService(eCtx)
  }

  public static onChangeClaimStatusButtonClick(eCtx) {
    const eventLogic = new Claim(eCtx)
    eventLogic.claimService.openCustomPage();
  }
}


