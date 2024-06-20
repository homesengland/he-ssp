import { ContactService } from '../services/ContactService'
import { CommonLib } from '../Common'

export class Contact {
  private common: CommonLib
  private contactService: ContactService

  constructor(eCtx) {
    this.common = new CommonLib(eCtx)
    this.contactService = new ContactService(eCtx)
  }

  public static SendInviteButtonClick(eCtx) {
    const eventLogic = new Contact(eCtx)
    eventLogic.contactService.openCustomPageSendInvite();
  }
}
