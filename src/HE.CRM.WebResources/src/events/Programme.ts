import { ProgrammeService } from '../services/ProgrammeService'
import { CommonLib } from '../Common'

export class Programme {
  private common: CommonLib
  private programmeService: ProgrammeService

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
    this.programmeService = new ProgrammeService(eCtx);
  }

  public static onLoad(eCtx) {
    const eventLogic = new Programme(eCtx);
    eventLogic.programmeService.ShowFormNotification();
  }
}
