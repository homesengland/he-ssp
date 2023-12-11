import { AhpApplicationService } from '../services/AhpApplicationService'
import { CommonLib } from '../Common'

export class AhpApplication {
  private common: CommonLib
  private ahpApplicationService: AhpApplicationService

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
    this.ahpApplicationService = new AhpApplicationService(eCtx)
  }

  public static onLoad(eCtx) {
    const eventLogic = new AhpApplication(eCtx)
    eventLogic.registerEvents();
  }

  public static onChangeAhpApplicationStatusButtonClick(eCtx) {
    const eventLogic = new AhpApplication(eCtx)
    eventLogic.ahpApplicationService.openCustomPage();
  }

  public registerEvents() {
    
  }
}
