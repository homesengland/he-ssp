(() => {
  const setUpBannerButtons = () => {
    var banners = document.getElementsByClassName("govuk-cookie-banner");
    if(banners.length !== 1){
      return;
    }

    var policyAcknowledged = getCookie('accept_additional_cookies');
    if(policyAcknowledged !== undefined){
      banners[0].remove();
      return;
    }

    banners[0].classList.remove('govuk-!-display-none');
    var buttons = banners[0].getElementsByTagName("button");

    for (let button of buttons) {
      button.onclick = () => {
        setCookie(button.value);
        banners[0].remove();}
    }
  }

  const setCookie =(value) => {
    const data = new Date();
    data.setTime(data.getTime() + (365 * 24*60*60*1000));
    document.cookie = "accept_additional_cookies=".concat(value, '; expires=', data.toUTCString(), '; path=/; secure');
  }

  const getCookie = (name) => {
    if (document.cookie !== "") {
      const cookies = document.cookie.split(/; */);

      for (let cookie of cookies) {
        const [ cookieName, cookieVal ] = cookie.split("=");
        if (cookieName === decodeURIComponent(name)) {
          return decodeURIComponent(cookieVal);
        }
      }
    }

    return undefined;
  }

  document.addEventListener("DOMContentLoaded", function(event) {
    setUpBannerButtons();
  });

})();

(() => {
  const exclusiveCheckboxGroupClassName = "exclusive-checkbox-group";
  const exclusiveCheckboxGroupExclusiveClassName = `${exclusiveCheckboxGroupClassName}--exclusive-option`;

  const setUpExclusiveCheckbox = () => {
    const checkboxGroup = document.getElementsByClassName(exclusiveCheckboxGroupClassName);
    const exclusiveCheckboxGroup = document.getElementsByClassName(exclusiveCheckboxGroupExclusiveClassName);
    if (checkboxGroup.length !== 1 || exclusiveCheckboxGroup.length !== 1) {
      return;
    }

    addCheckboxChangeEventHandlers(checkboxGroup[0].getElementsByTagName("input"), exclusiveCheckboxGroupExclusiveClassName);
    addCheckboxChangeEventHandlers(exclusiveCheckboxGroup[0].getElementsByTagName("input"), exclusiveCheckboxGroupClassName);
  }

  const addCheckboxChangeEventHandlers = (checkboxes, checkboxesToDeselect) =>{
    for (let i = 0; i < checkboxes.length; i++) {
      checkboxes[i].addEventListener('change', (event) => {
        if (event.currentTarget.value) {
          deselectAllByClassName(checkboxesToDeselect);
        }
      });
    }
  }

  const deselectAllByClassName = (className) => {
    const checkboxes = document.getElementsByClassName(className).item(0).getElementsByTagName("input");
    for (let i = 0; i < checkboxes.length; i++) {
      checkboxes[i].checked = false;
    }
  }

  document.addEventListener("DOMContentLoaded", function() {
    setUpExclusiveCheckbox();
  });
})();

// This script performs validation for maximum file size for upload file input element.
// It displays dynamic error message just above file input field and in error summary.
// It is compatible with server side errors, so it appends new messages and removes only messages that applies to file input.
// For more details about usage look at HomeTypes/DesignPlans.cshtml file.
//
// Requirements:
// - file input '.govuk-file-upload[type="file"]'
// - file input should be placed inside 'div' with Id 'file-input-form-group'
// - file input error message is placed in 'span' with Id 'File-error'
// - on the top of the page there is placed '_ErrorSummaryPartial.cshtml' partial view
// - page has submit button '.govuk-button[type="submit"]'
// - maximum size of the file is in value of element with Id 'MaxFileSizeInMegabytes'
// - URL to file upload is in value of element with id 'upload-file-url'
// - URL to remove file is in value of element with id 'remove-file-url-template', URL contains {:fileId} template parameter
// - URL to download file is in value of element with id 'download-file-url-template', URL contains {:fileId} template parameter
(() => {
  const fileInputSelector = `.govuk-file-upload[type="file"]`;
  const fileInputFormGroupId = 'file-input-form-group';
  const validationSummaryId = 'validation-summary';
  const validationSummaryListId = 'validation-error-list';
  const continueButtonSelector = `.govuk-button[type="submit"]`;
  const maxFileSizeId = 'MaxFileSizeInMegabytes';
  const uploadFileUrlId = 'upload-file-url';
  const removeFileUrlId = 'remove-file-url-template';
  const downloadFileUrlId = 'download-file-url-template';

  const getUploadControlId = () => document.querySelectorAll(fileInputSelector)[0].id;
  const inputFieldError = (message) => `<span id="${getUploadControlId()}-error" class="govuk-error-message field-validation-error" data-valmsg-for="${getUploadControlId()}" data-valmsg-replace="true"><span class="govuk-visually-hidden">Error:</span>${message}</span>`;
  const emptyErrorSummary = () =>
    `<div aria-labelledby="error-summary-title" role="alert" tabindex="-1" data-module="govuk-error-summary" id="${validationSummaryId}" class="govuk-error-summary">
      <h2 id="error-summary-title" class="govuk-error-summary__title">
        There is a problem
      </h2>
      <div class="govuk-error-summary__body">
        <div data-valmsg-summary="true" id="${validationSummaryListId}" class="govuk-error-message">
          <ul class="govuk-list govuk-error-summary__list"></ul>
        </div>
      </div>
    </div>`;
  const inputFiledErrorSummaryMessage = (message) => `<li><a href="#${getUploadControlId()}">${message}</a></li>`;

  const fileTableRow = (fileId, fileName) => `<tr class="govuk-table__row" id="file-${fileId}">
      <td class="govuk-table__cell govuk-!-font-weight-bold">${sanitize(fileName)}</td>
      <td class="govuk-table__cell"></td>
      <td class="govuk-table__cell govuk-!-text-align-right">Queued</td>
    </tr>`;

  const removeFileLink = (removeFileUrl) => `<a class="govuk-link--no-visited-state govuk-!-margin-left-4 govuk-link" href="${removeFileUrl}">Remove</a>`;

  const downloadFileLink = (fileName, downloadFileUrl) => `<a class="govuk-link" href="${downloadFileUrl}">${sanitize(fileName)}</a>`;

  const fileInputChanged = () => {
    const maxFileSizeInMegabytes = document.getElementById(maxFileSizeId).value;
    const maxFileSize = maxFileSizeInMegabytes * 1024 * 1024;
    const allowedExtensions = document.getElementById('AllowedExtensions').value;
    const allowedExtensionsArray = allowedExtensions.split(', ').map(x => `.${x.toUpperCase()}`);
    const uploadControl = document.querySelectorAll(fileInputSelector)[0];
    const continueButton = document.querySelectorAll(continueButtonSelector)[0];
    const uploadFileUrl = document.getElementById(uploadFileUrlId);
    const token = document.querySelectorAll('input[name="__RequestVerificationToken"]')[0].value;
    const invalidFiles = new DataTransfer();
    const filesToUpload = {};
    const shouldUpload = uploadFileUrl !== null && uploadFileUrl !== undefined;

    for (let i = 0; i < uploadControl.files.length; i++) {
      invalidFiles.items.add(uploadControl.files[i]);
    }

    continueButton.disabled = true;
    uploadControl.disabled = shouldUpload;
    let hasErrors = false;
    clearInputFieldError();
    clearInputFieldErrorSummary();

    for (let i = invalidFiles.files.length - 1; i >= 0; i--) {
      const file = invalidFiles.files[i];
      if (file.size > maxFileSize) {
        const errorMessage = `The selected file \"${sanitize(file.name)}\" must be smaller than or equal to ${maxFileSizeInMegabytes}MB`;

        hasErrors = true;
        addInputFieldError(errorMessage);
        addInputFieldErrorSummary(errorMessage);
      }
      if (!allowedExtensionsArray.includes(getFileExtension(file.name))) {
        const errorMessage = `The selected file \"${sanitize(file.name)}\" must be a ${allowedExtensions}`;

        hasErrors = true;
        addInputFieldError(errorMessage);
        addInputFieldErrorSummary(errorMessage);
      } else if (shouldUpload) {
        const fileId = (Math.random() + 1).toString(36).substring(7);
        fileUploadInitialized(fileId, file);
        filesToUpload[fileId] = file;
        invalidFiles.items.remove(i);
      }
    }

    if (shouldUpload) {
      uploadControl.files = invalidFiles.files;
      uploadFiles(filesToUpload, uploadFileUrl, token)
        .finally(() => {
          if (!hasErrors) {
            continueButton.disabled = false;
          }

          uploadControl.disabled = false;
        });
    }
    else {
      if (!hasErrors) {
        continueButton.disabled = false;
      }

      uploadControl.disabled = false;
    }
  }

  const uploadFiles = (filesToUpload, uploadFileUrl, token) => {
    let promise = Promise.resolve();

    for (let fileId in filesToUpload) {
      const url = uploadFileUrl.value;
      const file = filesToUpload[fileId];
      const formData = new FormData();
      formData.append("__RequestVerificationToken", token);
      formData.append("file", file);

      promise = promise.then(() => fileUploadStarted(fileId))
        .then(() => fetch(url, {method: "POST", body: formData}))
        .then(response => response.ok ? fileUploadFinished(fileId, file.name, response) : fileUploadFailed(fileId, response))
        .catch(() => fileUploadFailed(fileId, undefined));
    }

    return promise;
  }

  const fileUploadInitialized = (fileId, file) => {
    const table = document.getElementsByClassName("govuk-table__body")[0];
    table.innerHTML += fileTableRow(fileId, file.name);
  }

  const fileUploadStarted = (fileId) => {
    const actionColumn = document.querySelector(`#file-${fileId} :nth-child(3)`);
    actionColumn.innerHTML = '<div class="loader-line"></div>';
  }

  const fileUploadFinished = (fileId, fileName, response) => {
    return response.json()
      .then(uploadedFile => {
        const fileNameColumn = document.querySelector(`#file-${fileId} :nth-child(1)`);
        const uploadedColumn = document.querySelector(`#file-${fileId} :nth-child(2)`);
        const actionColumn = document.querySelector(`#file-${fileId} :nth-child(3)`);

        uploadedColumn.innerText = uploadedFile.uploadDetails;

        const removeFileUrlElement = document.getElementById(removeFileUrlId);
        if (uploadedFile.canBeRemoved && removeFileUrlElement) {
          const removeUrl = removeFileUrlElement.value.replace('%3AfileId', uploadedFile.fileId);
          actionColumn.innerHTML = removeFileLink(removeUrl);
        } else {
          actionColumn.innerHTML = "";
        }

        const downloadFileUrlElement = document.getElementById(downloadFileUrlId);
        if (downloadFileUrlElement) {
          const downloadUrl = downloadFileUrlElement.value.replace('%3AfileId', uploadedFile.fileId);
          fileNameColumn.innerHTML = downloadFileLink(fileName, downloadUrl);
        }
      });
  }

  const fileUploadFailed = (fileId, response) => {
    const uploadedColumn = document.querySelector(`#file-${fileId} :nth-child(2)`);
    const actionColumn = document.querySelector(`#file-${fileId} :nth-child(3)`);

    uploadedColumn.innerText = "Upload failed";
    actionColumn.innerHTML = "";

    if (response !== undefined && response.status === 400) {
      return response.json()
        .then(errors => {
          if (Array.isArray(errors) && errors.length > 0 && errors[0].errorMessage){
            const errorMessage = errors[0].errorMessage;

            uploadedColumn.innerText = errorMessage;
            addInputFieldError(errorMessage, true);
            addInputFieldErrorSummary(errorMessage, true);
          }
        })
        .catch(error => console.log(error));
    }

    return Promise.resolve();
  }

  const addInputFieldError = (message) => {
    const formGroup = document.getElementById(fileInputFormGroupId);
    const inputControl = document.querySelectorAll(fileInputSelector)[0];
    const error = inputFieldError(message);

    formGroup.classList.add('govuk-form-group--error');

    for (const node in inputControl.parentNode.childNodes.values()) {
      if (node.outerHTML === error) {
        return;
      }
    }

    inputControl.insertAdjacentHTML('beforebegin', error);
  }

  const clearInputFieldError = () => {
    const formGroup = document.getElementById(fileInputFormGroupId);
    const errorSpans = formGroup.getElementsByClassName('govuk-error-message');
    for (let i = errorSpans.length - 1; i >= 0; i--) {
      errorSpans[i].remove();
    }

    formGroup.classList.remove('govuk-form-group--error');
  }

  const addInputFieldErrorSummary = (message) => {
    let summaryValidationList = document.getElementById(validationSummaryListId);
    if (!summaryValidationList) {
      const form = document.querySelectorAll(fileInputSelector)[0].closest('form');
      form.children[0].insertAdjacentHTML('afterend', emptyErrorSummary());

      summaryValidationList = document.getElementById(validationSummaryListId);
    }

    const list = summaryValidationList.getElementsByTagName('ul')[0];
    const error = inputFiledErrorSummaryMessage(message);

    for (const node in list.childNodes.values()) {
      if (node.outerHTML === error) {
        return;
      }
    }

    list.innerHTML += inputFiledErrorSummaryMessage(message);
  }

  const removeInputFieldErrorSummaryMessages = (summaryValidationList) => {
    const errors = summaryValidationList.getElementsByTagName('a');
    for (let i = errors.length - 1; i >= 0; i--) {
      if (errors[i].href.endsWith(`#${getUploadControlId()}`)) {
        errors[i].parentNode.remove();
      }
    }
  }

  const clearInputFieldErrorSummary = () => {
    const summaryValidationList = document.getElementById(validationSummaryListId);
    if (!summaryValidationList) {
      return;
    }

    removeInputFieldErrorSummaryMessages(summaryValidationList);

    if (summaryValidationList.getElementsByTagName('a').length === 0) {
      document.getElementById(validationSummaryId).remove();
    }
  }

  const sanitize = string => {
    const map = {
      '&': '&amp;',
      '<': '&lt;',
      '>': '&gt;',
      '"': '&quot;',
      "'": '&#x27;',
      "/": '&#x2F;',
    };
    const reg = /[&<>"'/]/ig;
    return string.replace(reg, (match)=>(map[match]));
  }

  const getFileExtension = fileName =>
  {
    return fileName.substring(fileName.lastIndexOf('.')).toUpperCase();
  }

  document.addEventListener("DOMContentLoaded", function() {
    const fileInput = document.querySelectorAll(fileInputSelector)[0];
    if (fileInput !== null && fileInput !== undefined) {
      fileInput.addEventListener("change", fileInputChanged);
    }
  });
})();

(() => {
  const setUpBackLink = () => {
    const backLink = document.getElementById("js-back-link");
    if (backLink) {
      backLink.onclick = () => window.history.back();
    }
  }

  document.addEventListener("DOMContentLoaded", function () {
    setUpBackLink();
  });
})();

(() => {
    const initModule = (Module, selector) => {
        var components = document.querySelectorAll(`[data-module="${selector}"]`)
        components.forEach(item => new Module(item).init())
    }

  initModule(window.GOVUKFrontend.CharacterCount, "govuk-character-count");
  initModule(window.GOVUKFrontend.Radios, "govuk-radios");
  initModule(window.GOVUKFrontend.Button, "govuk-button");
})();

MOJFrontend.initAll = function (options) {
  options = typeof options !== 'undefined' ? options : {};
  let scope = typeof options.scope !== 'undefined' ? options.scope : document;

  let $sortableTables = scope.querySelectorAll('[data-module="moj-sortable-table"]');
  MOJFrontend.nodeListForEach($sortableTables, function ($table) {
    new MOJFrontend.SortableTable({
      table: $table
    });
  });
}

MOJFrontend.initAll();
