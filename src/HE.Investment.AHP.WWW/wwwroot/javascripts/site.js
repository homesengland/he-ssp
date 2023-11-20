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
(() => {
  const fileInputSelector = `.govuk-file-upload[type="file"]`;
  const fileInputFormGroupId = 'file-input-form-group';
  const validationSummaryId = 'validation-summary';
  const validationSummaryListId = 'validation-error-list';
  const continueButtonSelector = `.govuk-button[type="submit"]`;
  const maxFileSizeId = 'MaxFileSizeInMegabytes';

  const uploadControlId = document.querySelectorAll(fileInputSelector)[0].id;

  const inputFieldError = (message) => `<span id="${uploadControlId}-error" class="govuk-error-message field-validation-error" data-valmsg-for="${uploadControlId}" data-valmsg-replace="true"><span class="govuk-visually-hidden">Error:</span>${message}</span>`;
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
  const inputFiledErrorSummaryMessage = (message) => `<li><a href="#${uploadControlId}">${message}</a></li>`;

  const fileInputChanged = () => {
    const maxFileSizeInMegabytes = document.getElementById(maxFileSizeId).value;
    const maxFileSize = maxFileSizeInMegabytes * 1024 * 1024;
    const uploadControl = document.querySelectorAll(fileInputSelector)[0];
    const continueButton = document.querySelectorAll(continueButtonSelector)[0];

    continueButton.disabled = false;
    clearInputFieldError();
    clearInputFieldErrorSummary();

    for (let i = 0; i < uploadControl.files.length; i++) {
      if (uploadControl.files[i].size > maxFileSize) {
        const errorMessage = `The selected file must be smaller than ${maxFileSizeInMegabytes}MB`;

        continueButton.disabled = true;
        addInputFieldError(errorMessage);
        addInputFieldErrorSummary(errorMessage);
      }
    }
  }

  const addInputFieldError = (message) => {
    const formGroup = document.getElementById(fileInputFormGroupId);

    document.querySelectorAll(fileInputSelector)[0].insertAdjacentHTML('beforebegin', inputFieldError(message));
    formGroup.classList.add('govuk-form-group--error');
  }

  const clearInputFieldError = () => {
    const formGroup = document.getElementById(fileInputFormGroupId);
    const errorSpan = document.getElementById(`${uploadControlId}-error`);
    if (errorSpan){
      errorSpan.remove();
    }

    formGroup.classList.remove('govuk-form-group--error');
  }

  const addInputFieldErrorSummary = (message) => {
    let summaryValidationList = document.getElementById(validationSummaryListId);
    if (!summaryValidationList) {
      const forms = document.getElementsByTagName('form');
      forms[0].children[0].insertAdjacentHTML('afterend', emptyErrorSummary());

      summaryValidationList = document.getElementById(validationSummaryListId);
    }

    summaryValidationList.getElementsByTagName('ul')[0].innerHTML += inputFiledErrorSummaryMessage(message);
  }

  const removeInputFieldErrorSummaryMessages = (summaryValidationList) => {
    const errors = summaryValidationList.getElementsByTagName('a');
    for (let i = 0; i < errors.length; i++) {
      if (errors[i].href.endsWith(`#${uploadControlId}`)) {
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

  document.addEventListener("DOMContentLoaded", function() {
    const fileInput = document.querySelectorAll(fileInputSelector)[0];
    if (fileInput !== null) {
     fileInput.addEventListener("change", fileInputChanged);
    }
  });
})();
