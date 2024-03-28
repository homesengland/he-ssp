import { ProjectDetailsService } from '../services/ProjectDetailsService'
import { CommonLib } from '../Common'

export class ProjectDetails {
  private common: CommonLib
  private projectDetailsService: ProjectDetailsService

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
    this.projectDetailsService = new ProjectDetailsService(eCtx)
  }

  public static onLoad(eCtx) {
    const eventLogic = new ProjectDetails(eCtx)
    eventLogic.registerEvents()
  }

  public static populateRegion(eCtx) {
    const eventLogic = new ProjectDetails(eCtx)
    eventLogic.projectDetailsService.populateRegion()
  }


  public registerEvents() {
    if (this.common.getAttribute('invln_helocalauthorityid')) {
      this.common.getAttribute('invln_helocalauthorityid').removeOnChange(ProjectDetails.populateRegion)
      this.common.getAttribute('invln_helocalauthorityid').addOnChange(ProjectDetails.populateRegion)
    }
  }

}
