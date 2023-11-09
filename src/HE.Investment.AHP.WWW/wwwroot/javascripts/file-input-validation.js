// This script performs validation for maximum file size for upload file input element.
// It displays dynamic error message just above file input field and in error summary.
// It is compatible with server side errors, so it appends new messages and removes only messages that applies to file input.
// For more details about usage look at HomeTypes/DesignPlans.cshtml file.
//
// Requirements:
// - file input with Id 'File'
// - file input should be placed inside 'div' with Id 'file-input-form-group'
// - file input error message is placed in 'span' with Id 'File-error'
// - on the top of the page there is placed '_ErrorSummaryPartial.cshtml' partial view
// - page has submit button with Id 'continue-button'
// - maximum size of the file is in value of element with Id 'MaxFileSizeInMegabytes'
(() => {
  const uploadControlId = 'File';
  const fileInputFormGroupId = 'file-input-form-group';
  const validationSummaryId = 'validation-summary';
  const validationSummaryListId = 'validation-error-list';
  const continueButtonId = 'continue-button';
  const maxFileSizeId = 'MaxFileSizeInMegabytes';

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
    const uploadControl = document.getElementById(uploadControlId);
    const continueButton = document.getElementById(continueButtonId);

    continueButton.disabled = false;
    clearInputFieldError();
    clearInputFieldErrorSummary();

    for (let i = 0; i < uploadControl.files.length; i++) {
      if (uploadControl.files[i].size > maxFileSize) {
        const errorMessage = `The selected file ${uploadControl.files[0].name} must be smaller than ${maxFileSizeInMegabytes}MB`;

        continueButton.disabled = true;
        addInputFieldError(errorMessage);
        addInputFieldErrorSummary(errorMessage);
      }
    }
  }

  const addInputFieldError = (message) => {
    const formGroup = document.getElementById(fileInputFormGroupId);

    document.getElementById(uploadControlId).insertAdjacentHTML('beforebegin', inputFieldError(message));
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
    const fileInput = document.getElementById(uploadControlId);
    if (fileInput !== null) {
     fileInput.addEventListener("change", fileInputChanged);
    }
  });
})();
