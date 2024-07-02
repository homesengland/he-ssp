import { HomeTypeService } from '../services/HomeTypeService'
import { CommonLib } from '../Common'

export class HomeType {
  private common: CommonLib
  private homeTypeService: HomeTypeService

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
    this.homeTypeService = new HomeTypeService(eCtx)
  }

  public static onLoad(eCtx) {
    const eventLogic = new HomeType(eCtx)
    eventLogic.registerEvents();
    eventLogic.homeTypeService.ndssCalculationError();
    eventLogic.homeTypeService.showHideSection();
  }

  public static onSave(eCtx) {
    const eventLogic = new HomeType(eCtx)
    eventLogic.homeTypeService.removeNotification();
  }

  static nDSSNotification(eCtx) {
    const eventLogic = new HomeType(eCtx);
    eventLogic.homeTypeService.ndssCalculationError();
  }

  public registerEvents() {
    if (this.common.getAttribute('invln_numberofbedrooms')) {
      this.common.getAttribute('invln_numberofbedrooms').removeOnChange(HomeType.nDSSNotification);
      this.common.getAttribute('invln_numberofbedrooms').addOnChange(HomeType.nDSSNotification);
    }

    if (this.common.getAttribute('invln_maxoccupancy')) {
      this.common.getAttribute('invln_maxoccupancy').removeOnChange(HomeType.nDSSNotification);
      this.common.getAttribute('invln_maxoccupancy').addOnChange(HomeType.nDSSNotification);
    }

    if (this.common.getAttribute('invln_numberofstoreys')) {
      this.common.getAttribute('invln_numberofstoreys').removeOnChange(HomeType.nDSSNotification);
      this.common.getAttribute('invln_numberofstoreys').addOnChange(HomeType.nDSSNotification);
    }
  }
}
