import { RetrieveRequestType } from "./OptionSet"

export interface EntityReferenceRibbon {
  Id: string
  Name: string
  TypeName: string
  TypeCode: number
}

export class CommonLib {
  public executionContext: Xrm.Events.EventContext | undefined
  public formContext: Xrm.FormContext

  constructor(eCtx: any) {
    if (eCtx !== null && eCtx.getFormContext) {
      this.executionContext = eCtx
      this.formContext = eCtx.getFormContext()
    } else {
      this.formContext = eCtx // for ribbon primary controls
    }
  }

  public getResourceString(key: string, webResourceName?: string) {
    if (!webResourceName) {
      webResourceName = 'ap_/resx/common'
    }

    return Xrm.Utility.getResourceString(webResourceName, key)
  }

  public getWebApiconfig() {
    return { webApiVersion: '9.1', includeAnnotations: '*' }
  }

  public getFormType() {
    return this.formContext.ui.getFormType()
  }

  public refresh(save: boolean) {
    if (
      this.formContext !== undefined &&
      this.formContext !== null &&
      this.formContext.data !== undefined &&
      this.formContext.data !== null
    ) {
      return this.formContext.data.refresh(save)
    }

  }

  public refreshForm(save: boolean) {
    if (
      this.formContext !== undefined &&
      this.formContext !== null &&
      this.formContext.data !== undefined &&
      this.formContext.data !== null
    ) {
      this.formContext.data.save().then(() => {
        Xrm.Navigation.openForm({
          entityId: this.formContext.data.entity.getId(),
          entityName: this.formContext.data.entity.getEntityName()
        })
      })
      return this.formContext.data.refresh(save)
    }
  }

  public refreshRibbon(refreshAll) {
    if (
      this.formContext !== undefined &&
      this.formContext !== null &&
      this.formContext.ui !== undefined &&
      this.formContext.ui !== null
    ) {
      return this.formContext.ui.refreshRibbon(refreshAll)
    }
  }

  public parseLookup(lookupName: string, webApiEntity: object): Array<Xrm.LookupValue> | null {
    if (webApiEntity == null || lookupName == null) {
      return null
    }

    if (webApiEntity[lookupName] == undefined || webApiEntity[lookupName] == null) {
      return null
    }

    var lookup: Xrm.LookupValue = {
      id: '{' + webApiEntity[lookupName].toUpperCase() + '}',
      entityType: webApiEntity[lookupName + '@Microsoft.Dynamics.CRM.lookuplogicalname'],
      name: webApiEntity[lookupName + '@OData.Community.Display.V1.FormattedValue']
    }

    var lookupValue = []
      ; (lookupValue[0] as any) = lookup

    return lookupValue
  }

  public convertOdataToLookup(odataEntity, fieldName: string): Array<Xrm.LookupValue> | null {
    if (odataEntity['_' + fieldName + '_value']) {
      return [
        {
          id: '{' + odataEntity['_' + fieldName + '_value'].toUpperCase() + '}',
          name: odataEntity['_' + fieldName + '_value@OData.Community.Display.V1.FormattedValue'],
          entityType:
            odataEntity['_' + fieldName + '_value@Microsoft.Dynamics.CRM.lookuplogicalname']
        }
      ]
    } else {
      return null
    }
  }

  public cloneFromOneFieldToAnother(from: string, to: string) {
    var _fromAttr = this.formContext.getAttribute(from)
    if (_fromAttr != null) {
      var _fromVal = _fromAttr.getValue()
      this.formContext.getAttribute(to).setValue(_fromVal)
      this.formContext.getAttribute(to).setSubmitMode('always')
    }
  }

  public getLookupValue(attributeName: string): Xrm.LookupValue | null {
    const attribute = this.formContext.getAttribute<Xrm.Attributes.LookupAttribute>(attributeName)

    if (attribute) {
      const lookupValue = attribute.getValue()

      if (lookupValue && lookupValue[0]) {
        return lookupValue[0]
      }
    }

    return null
  }

  public setLookUpValue(LookUpAttributeName: string, Type: string, Id: string, Name: string) {
    if (LookUpAttributeName != null && Type != null && Id != null && Name != null) {
      var lookupReference: any = [];
      lookupReference[0] = {};
      lookupReference[0].id = Id;
      lookupReference[0].entityType = Type;
      lookupReference[0].name = Name;
      this.formContext.getAttribute(LookUpAttributeName).setValue(lookupReference);
    }
  }

  public trimBraces(value: string): string {
    return value.replace('{', '').replace('}', '')
  }

  public calculateNextDate(startDate: Date, numberofDay: number, onlyBusiness): Date {
    var _day = startDate.getDay()
    var daysToAdd = numberofDay
    if (_day == 0) daysToAdd++
    if (_day + daysToAdd >= 6) {
      var remainingWorkDays = daysToAdd - (5 - _day)
      daysToAdd += 2
      if (remainingWorkDays > 5) {
        daysToAdd += 2 * Math.floor(remainingWorkDays / 5)
        if (remainingWorkDays % 5 == 0) daysToAdd -= 2
      }
    }
    startDate.setDate(startDate.getDate() + daysToAdd)
    return startDate
  }

  public setAttributeValue<T extends Xrm.Attributes.Attribute>(attributeName: string, value) {
    var attribute = this.formContext.getAttribute<T>(attributeName)
    if (attribute !== null) {
      return attribute.setValue(value)
    }
  }

  public setSubmitMode(attributeName: string, submitMode: XrmEnum.SubmitMode) {
    var attribute = this.formContext.getAttribute(attributeName)
    if (attribute !== null) {
      return attribute.setSubmitMode(submitMode)
    }
  }

  public setAttributeDateValue(attributeName: string, value: string) {
    if (value) {
      const date = new Date(value)
      this.setAttributeValue(attributeName, date)
    }
  }

  public lockTheForm(isLocked: boolean) {
    this.formContext.ui.controls.forEach((control, index) => {
      (<Xrm.Controls.StandardControl>control).setDisabled(isLocked)
    })
  }

  public lockControl(name: string, isDisabled: boolean) {
    var _control = this.formContext.getControl<Xrm.Controls.StandardControl>(name)
    if (_control != null) _control.setDisabled(isDisabled)
  }

  public hideControl(name: string, isHidden: boolean) {
    var _control = this.formContext.getControl<Xrm.Controls.StandardControl>(name)
    if (_control != null) _control.setVisible(!isHidden)
  }

  public setControlRequired(name: string, level: Xrm.Attributes.RequirementLevel) {
    const _attr = this.formContext.getAttribute(name)
    if (_attr != null) {
      _attr.setRequiredLevel(level)
    }
  }

  public setControlRequiredV2(name: string, isRequired: boolean) {
    if (isRequired === false) {
      this.setControlRequired(name, 'none')
    } else {
      this.setControlRequired(name, 'required')
    }
  }

  public showTab(tabName: string) {
    var tab = this.formContext.ui.tabs.get(tabName)

    if (tab != null) {
      tab.setVisible(true)
    }
  }

  public hideTab(tabName: string) {
    var tab = this.formContext.ui.tabs.get(tabName)

    if (tab != null) {
      tab.setVisible(false)
    }
  }

  public showSection(tabName: string, sectionName: string) {
    var tab = this.formContext.ui.tabs.get(tabName)
    if (tab != null) {
      var _section = tab.sections.get(sectionName)

      if (_section != null) _section.setVisible(true)
    }
  }

  public hideSection(tabName: string, sectionName: string) {
    const tab = this.formContext.ui.tabs.get(tabName)

    if (tab != null) {
      const section = tab.sections.get(sectionName)

      if (section != null) section.setVisible(false)
    }
  }

  public areGuidsEqual(g1: string, g2: string): boolean {
    return g1.replace(/[{}]/g, '').toLowerCase() === g2.replace(/[{}]/g, '').toLowerCase()
  }

  public getCurrentEntityId() {
    return this.formContext.data.entity.getId()
  }

  public getCurrentEntityName() {
    return this.formContext.data.entity.getEntityName()
  }

  private getFormContext(eCtx: any) {
    if (eCtx && eCtx.getFormContext && eCtx.getFormContext()) {
      return eCtx.getFormContext()
    } else {
      return console.log('Could not load Form Context!')
    }
  }

  public getAttribute<T extends Xrm.Attributes.Attribute>(attributeName: string): T {
    return this.formContext.getAttribute<T>(attributeName)
  }

  public getAttributeValue(attrName: string) {
    var attrValue = null
    if (attrName != null) {
      attrValue =
        this.formContext.getAttribute(attrName) == null
          ? null
          : this.formContext.getAttribute(attrName).getValue()
    }

    return attrValue
  }

  public getSelectedOption(attrName: string) {
    var selectedOption
    if (attrName != null) {
      selectedOption =
        this.formContext.getAttribute(attrName) == null
          ? null
          : this.formContext
            .getAttribute<Xrm.Attributes.OptionSetAttribute>(attrName)
            .getSelectedOption()
    }

    return selectedOption
  }

  public getAllOptions(attrName: string) {
    var allOptions
    if (attrName != null) {
      allOptions =
        this.formContext.getAttribute(attrName) == null
          ? null
          : this.formContext.getAttribute<Xrm.Attributes.OptionSetAttribute>(attrName).getOptions()
    }

    return allOptions
  }

  public clearOptions(attrName: string) {
    var optionSet = this.formContext.getControl<Xrm.Controls.OptionSetControl>(attrName)
    if (optionSet != null) {
      optionSet.clearOptions()
    }
  }

  public addOption(attrName: string, optionToAdd: Xrm.OptionSetValue) {
    var optionSet = this.formContext.getControl<Xrm.Controls.OptionSetControl>(attrName)
    optionSet.addOption(optionToAdd)
  }

  public setValue(attrName: string, attrValue: any) {
    if (attrName != null) {
      this.formContext.getAttribute(attrName).setValue(attrValue)
      this.formContext.getAttribute(attrName).setSubmitMode('always')
    }
  }

  public getTab(tabName: string) {
    return this.formContext.ui.tabs.get(tabName)
  }

  public getSection(sectionName: string, tabName: string) {
    return this.getTab(tabName).sections.get(sectionName)
  }

  public getControl<T extends Xrm.Controls.Control>(controlName: string): T {
    return this.formContext.getControl<T>(controlName)
  }

  public validateDates(startDate: Date, endDate: Date) {
    var isValid = true
    if (startDate != null && endDate != null) {
      var tmpStartDate = startDate.setHours(0, 0, 0, 0)
      var tmpEndDate = endDate.setHours(0, 0, 0, 0)

      if (tmpStartDate > tmpEndDate) {
        isValid = false
      }
    }

    return isValid
  }

  public setFieldNotification(fieldname: string, message: string, uniqueid: string) {
    var control = this.getControl<Xrm.Controls.StandardControl>(fieldname)
    if (control) {
      control.setNotification(message, uniqueid)
    }
  }

  public clearFieldNotification(fieldname: string, uniqueid: string) {
    var control = this.getControl<Xrm.Controls.StandardControl>(fieldname)
    if (control) {
      control.clearNotification(uniqueid)
    }
  }

  public setFormNotification(
    message: string,
    level: XrmEnum.FormNotificationLevel,
    uniqueId: string
  ) {
    return this.formContext.ui.setFormNotification(message, level, uniqueId)
  }

  public clearFormNotification(uniqueId?: string) {
    if (uniqueId == null) return false

    return this.formContext.ui.clearFormNotification(uniqueId)
  }

  public saveForm() {
    return this.formContext.data.save()
  }

  public saveEntityAsync(saveOptions?: any) {
    return this.formContext.data.save(saveOptions)
  }

  public getUserId() {
    var userSettings = Xrm.Utility.getGlobalContext().userSettings
    if (userSettings != null) {
      return userSettings.userId
    } else {
      return null
    }
  }

  public getUserName() {
    var userSettings = Xrm.Utility.getGlobalContext().userSettings
    if (userSettings != null) {
      return userSettings.userName
    } else {
      return null
    }
  }

  public getUserSecurityRoles() {
    var userSettings = Xrm.Utility.getGlobalContext().userSettings
    if (userSettings != null) {
      return userSettings.roles
    } else {
      return null
    }
  }

  public log(obj: any) {
    console.log(obj)
  }

  public copyFieldsValues(map: any, predicate: boolean) {
    if (predicate) {
      for (var field in map) {
        this.copyFieldValue(map, predicate, field)
      }
    }
  }

  public copyFieldValue(map: any, predicate: boolean, sourceFieldName: string) {
    if (predicate) {
      var destinationFieldName = map[sourceFieldName]
      var value = this.getAttributeValue(sourceFieldName)
      this.setAttributeValue(destinationFieldName, value)
    }
  }

  public mapDictionaryToFields(map: any, dictionary: any) {
    for (var key in map) {
      var destinationFieldName = map[key]
      var value = dictionary[key]
      this.setAttributeValue(destinationFieldName, value)
    }
  }

  public removeFirstPropertyByValue(map: any, value: any) {
    if (value === undefined || value === null) return
    var keyToDelete

    for (var key in map) {
      if (map[key] === value) {
        keyToDelete = key
        break
      }
    }

    if (keyToDelete) delete map[keyToDelete]
  }

  public addDays(date: Date, days: number) {
    var dateTimezoneOffset = date.getTimezoneOffset() * 60 * 1000
    var ticks = date.getTime()
    var calculatedDate = new Date()
    var calculatedDateTimezoneOffset

    ticks += 1000 * 60 * 60 * 24 * days
    calculatedDate.setTime(ticks)

    calculatedDateTimezoneOffset = calculatedDate.getTimezoneOffset() * 60 * 1000

    if (dateTimezoneOffset !== calculatedDateTimezoneOffset) {
      var diff = calculatedDateTimezoneOffset - dateTimezoneOffset
      ticks += diff
      calculatedDate.setTime(ticks)
    }

    return calculatedDate
  }

  public diffDays(end: Date, start: Date) {
    if (!start || !end) return undefined

    return (end.getTime() - start.getTime()) / (1000 * 3600 * 24)
  }

  public nipValidator(fieldName: string) {
    if (this.formContext.getAttribute(fieldName).getValue() != null) {
      let oldNip = this.formContext.getAttribute(fieldName).getValue()
      let newNip = oldNip.replace(/\D/g, '')

      if (newNip.length == 10) {
        let a, b, c, d, NIP
        a = newNip.slice(0, 3)
        b = newNip.slice(3, 6)
        c = newNip.slice(6, 8)
        d = newNip.slice(8, 10)
        NIP = ''
        NIP = NIP.concat(a, b, c, d)

        let factor: Array<number>
        factor = [6, 5, 7, 2, 3, 4, 5, 6, 7]

        let sum = 0

        for (let i = 8; i >= 0; i--) {
          sum += factor[i] * parseInt(NIP.charAt(i))
        }

        if (sum % 11 == 10 ? false : sum % 11 == parseInt(NIP.charAt(9))) {
          this.formContext.getAttribute(fieldName).setValue(NIP)
        } else {
          return false
        }
      } else {
        return false
      }
    }
    return true
  }

  public peselValidator(fieldName: string) {
    if (this.formContext.getAttribute(fieldName).getValue() != null) {
      let oldPesel = this.formContext.getAttribute(fieldName).getValue()
      let newPesel = oldPesel.replace(/[^0-9]/g, '')
      let weight = [1, 3, 7, 9, 1, 3, 7, 9, 1, 3];
      let sum = 0;
      let controlNumber = parseInt(newPesel.substring(10, 11));

      for (let i = 0; i < weight.length; i++) {
        sum += (parseInt(newPesel.substring(i, i + 1)) * weight[i]);
      }
      sum = sum % 10;
      if ((10 - sum) % 10 === controlNumber) {
        this.formContext.getAttribute(fieldName).setValue(newPesel)
      }
      else {
        return false
      }
    }
    return true
  }

  public toogleNavigationItem(itemName: string, isVisible: boolean) {
    const navItem = this.formContext.ui.navigation.items.get(itemName)

    if (navItem) {
      navItem.setVisible(isVisible)
    }
  }

  public addCustomFilter(
    lookupCtrlName: string,
    filter: string,
    entityLogicalName?: string | undefined
  ) {
    let ctrl = this.getControl<Xrm.Controls.LookupControl>(lookupCtrlName)

    if (ctrl) {
      ctrl.addCustomFilter(filter, entityLogicalName)
    }
  }

  public addPreSearch(lookupCtrlName: string, handler: Xrm.Events.ContextSensitiveHandler) {
    let ctrl = this.getControl<Xrm.Controls.LookupControl>(lookupCtrlName)

    if (ctrl) {
      ctrl.addPreSearch(handler)
    }
  }

  public preventSave(): void {
    let saveCtx = <Xrm.Events.SaveEventContext>this.executionContext

    if (saveCtx && saveCtx.getEventArgs()) {
      saveCtx.getEventArgs().preventDefault()
    }
  }

  public getCurrentEntityPrimaryAttributeValue() {
    return this.formContext.data.entity.getPrimaryAttributeValue()
  }

  public disableField(fieldName, disable) {
    var _control = this.formContext.getControl<Xrm.Controls.StandardControl>(fieldName)
    if (_control != null) _control.setDisabled(disable)
  }

  public disableFields(fieldNames: string[], disable) {
    for (var i = 0; i < fieldNames.length; i++) {
      if (this.getAttribute(fieldNames[i])) {
        this.formContext.getControl<Xrm.Controls.StandardControl>(fieldNames[i]).setDisabled(disable)
      }
    }
  }

  public disableFieldsWithSameName(fieldName, disable) {
    var attribute = this.getAttribute<Xrm.Attributes.Attribute>(fieldName);
    if (attribute) {
      attribute.controls.forEach((control: any) => {
        control.setDisabled(disable);
      });
    }
    var control = this.getControl<Xrm.Controls.StandardControl>(fieldName);
    if (control) {
      control.setDisabled(disable);
    }
  }

  public hideFieldsWithSameName(fieldName, hide) {
    var attribute = this.getAttribute<Xrm.Attributes.Attribute>(fieldName);
    if (attribute) {
      attribute.controls.forEach((control: any) => {
        control.setVisible(!hide);
      });
    }
    var control = this.getControl<Xrm.Controls.StandardControl>(fieldName);
    if (control) {
      control.setVisible(!hide);
    }
  }

  public isDisabled(fieldName): boolean {
    return this.formContext.getControl<Xrm.Controls.StandardControl>(fieldName).getDisabled()
  }

  public disableAllFields() {
    this.formContext.ui.controls.forEach((control, i) => {
      if (control && (control as Xrm.Controls.StandardControl).getDisabled && !(control as Xrm.Controls.StandardControl).getDisabled()) {
        (control as Xrm.Controls.StandardControl).setDisabled(true)
      }
    });
  }

  public setMainAccount(parentCustomerFieldName, mainAccountName) {
    var parentId = this.getAttribute(parentCustomerFieldName).getValue()[0].id
    this.getAttribute(mainAccountName).setValue(null)

    var Entity: any = {}
    Entity.EntityType = this.getAttribute(parentCustomerFieldName).getValue()[0].entityType
    Entity.EntityID = parentId

    var req: any = {}
    req.Entity = JSON.stringify(Entity)
    req.getMetadata = function () {
      return {
        boundParameter: null,
        parameterTypes: {
          Entity: {
            typeName: 'Edm.String',
            structuralProperty: 5
          }
        },
        operationType: 0,
        operationName: 'odx_GetParent'
      }
    }

    var self = this

    Xrm.WebApi.online
      .execute(req)
      .then(
        function (data) {
          return data.json()
        },
        function (error) {
          console.log(error.message)
        }
      )
      .then(function (responseBody: any) {
        var parsed = JSON.parse(responseBody.EntityRefResponse)
        var lookup: Xrm.LookupValue = {
          id: '{' + parsed.EntityID.toUpperCase() + '}',
          entityType: parsed.EntityType
        }

        var lookupValue = []
          ; (lookupValue[0] as any) = lookup

        self.getAttribute(mainAccountName).setValue(lookupValue)

        console.log('The response is: %s', responseBody)
      })
  }

  public hasWriteAccess(entityLogicalName: string, id: string) {
    id = this.trimBraces(id)
    var request: any = {}
    request.Target = {
      '@odata.type': `Microsoft.Dynamics.CRM.${entityLogicalName}`
    }

    request.Target[`${entityLogicalName}id`] = id

    request.getMetadata = () => {
      return {
        boundParameter: null,
        parameterTypes: {
          Target: {
            typeName: 'mscrm.crmbaseentity',
            structuralProperty: 5
          }
        },
        operationType: 0,
        operationName: 'odx_haswriteaccess'
      }
    }

    return Xrm.WebApi.online.execute(request).then(result => {
      return result.json()
    })
  }

  public formatPhoneNumber(prefix: any, phoneNumber: any) {
    let result = `${prefix}${phoneNumber}`

    if (prefix && phoneNumber) {
      if (phoneNumber.length == 9) {
        let phoneOnlyNumbers = phoneNumber.replace(/\D/g, '')
        let match = phoneOnlyNumbers.match(/^(\d{3})(\d{3})(\d{3})$/)

        if (match) {
          result = `${prefix} ${match[1]} ${match[2]} ${match[3]}`
        }
      }
    }
    return result
  }

  public addMonthsToDate(startDate: Date, numberofMonth: number): Date {
    let newDate: Date = startDate

    if (startDate && numberofMonth) {
      newDate = new Date(startDate.setMonth(startDate.getMonth() + numberofMonth))
    }
    return newDate
  }

  public openConfirmDialog(text: string, title: string) {
    return Xrm.Navigation.openConfirmDialog(
      {
        text: text,
        title: title
      },
      { height: 200, width: 400 }
    )
  }

  public clearTab(tabName, sectionName, shallNotClean) {
    var sections = this.getTab(tabName).sections
    var section = sections.get<Xrm.Controls.Section>(sectionName)
    var self = this

    section.controls.forEach(function (value) {
      var attrName = value.getName()
      if (attrName !== shallNotClean) {
        self.setAttributeValue(attrName, null)
      }
    })
  }

  public retrieveMultipleByFetchXml(entityLogicalName: string, options: string, maxPageSize?: number): Xrm.Async.PromiseLike<Xrm.RetrieveMultipleResult> {
    let fetchXml = "?fetchXml=" + encodeURIComponent(options);
    return Xrm.WebApi.retrieveMultipleRecords(entityLogicalName, fetchXml, maxPageSize);
  }


  public retrieveRecord(retrieveRequestType: RetrieveRequestType, logicalName: string, collectionName: string, id: string, options?: any, headers?: {}): Promise<any> {
    var baseHeaders = {
      "OData-MaxVersion": "4.0", "OData-Version": "4.0",
      "Content-Type": "application/json; charset=utf-8",
      "Accept": "application/json",
      "Prefer": "odata.include-annotations=*"
    }

    switch (retrieveRequestType) {

      case RetrieveRequestType.WebAPI:
        return new Promise((resolve, reject) => {
          Xrm.WebApi.retrieveRecord(logicalName, id, options).then(
            (response) => {
              resolve(response);
            },
            (error) => {
              reject(error.message || error)
            }
          )
        })

      case RetrieveRequestType.Fetch:
        var url = Xrm.Utility.getGlobalContext().getClientUrl() + "/api/data/v9.2/" + collectionName + "(" + this.trimBraces(id) + ")" + options
        return new Promise((resolve, reject) => {
          fetch(
            url,
            {
              method: "GET",
              headers: {
                ...baseHeaders, ...headers
              },
            }).then(async (response) => {
              const data = await response.json();
              if (response.ok) {
                resolve(response.json());
              }
              else {
                reject((data.error && data.error.message) || response.status);
              }
            })
        })

      case RetrieveRequestType.XHR:
        var url = Xrm.Utility.getGlobalContext().getClientUrl() + "/api/data/v9.2/" + collectionName + "(" + this.trimBraces(id) + ")" + options
        return new Promise((resolve, reject) => {
          let xhr = new XMLHttpRequest();
          xhr.open("GET", url);
          baseHeaders = { ...baseHeaders, ...headers }
          Object.keys(baseHeaders).forEach(key => {
            xhr.setRequestHeader(key, baseHeaders[key]);
          });
          xhr.onload = () => {
            const data = JSON.parse(xhr.response);
            if (xhr.status >= 200 && xhr.status < 300) {
              resolve(data);
            }
            else {
              reject(data.error && data.error.message);
            }
          };
          xhr.onerror = () => reject(xhr.statusText);
          xhr.send();
        })

      default:
        return new Promise((resolve) => {
          throw new Error('Wrong parameter!');
        })
    }
  }

  public retrieveRecords(retrieveRequestType: RetrieveRequestType, logicalName: string, collectionName: string, options: any, headers?: {}): Promise<any> {
    var baseHeaders = {
      "OData-MaxVersion": "4.0", "OData-Version": "4.0",
      "Content-Type": "application/json; charset=utf-8",
      "Accept": "application/json",
      "Prefer": "odata.include-annotations=*"
    }

    switch (retrieveRequestType) {

      case RetrieveRequestType.WebAPI:
        return new Promise((resolve, reject) => {
          return Xrm.WebApi.retrieveMultipleRecords(logicalName, options).then(
            (response) => {
              resolve(response);
            },
            (error) => {
              reject(error.message || error)
            }
          )
        })

      case RetrieveRequestType.Fetch:
        var url = Xrm.Utility.getGlobalContext().getClientUrl() + "/api/data/v9.2/" + collectionName + options
        return new Promise((resolve, reject) => {
          fetch(
            url,
            {
              method: "GET",
              headers: {
                ...baseHeaders, ...headers
              },
            }).then(async (response) => {
              const data = await response.json();
              if (response.ok) {
                resolve(response.json());
              }
              else {
                reject((data.error && data.error.message) || response.status);
              }
            })
        })

      case RetrieveRequestType.XHR:
        var url = Xrm.Utility.getGlobalContext().getClientUrl() + "/api/data/v9.2/" + collectionName + options
        return new Promise((resolve, reject) => {
          let xhr = new XMLHttpRequest();
          xhr.open("GET", url);
          baseHeaders = { ...baseHeaders, ...headers }
          Object.keys(baseHeaders).forEach(key => {
            xhr.setRequestHeader(key, baseHeaders[key]);
          });
          xhr.onload = () => {
            const data = JSON.parse(xhr.response);
            if (xhr.status >= 200 && xhr.status < 300) {
              resolve(data);
            }
            else {
              reject(data.error && data.error.message);
            }
          };
          xhr.onerror = () => reject(xhr.statusText);
          xhr.send();
        })
      default:
        return new Promise((resolve) => {
          throw new Error('Wrong parameter!');
        })
    }
  }
}
