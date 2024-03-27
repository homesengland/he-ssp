import { CommonLib } from '../Common'

export class ProjectDetailsService {
  common: CommonLib
  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
  };

  public async populateRegion() {
    console.log("Execute Populate Region function");
    let localAuthorityId = this.common.getAttributeValue('invln_helocalauthorityid');
    let localAuthority = await Xrm.WebApi.retrieveRecord('he_localauthority', localAuthorityId![0]['id']);
    this.common.setAttributeValue('invln_heregion', localAuthority.he_laregion)

  } 
}
