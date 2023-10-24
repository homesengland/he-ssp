function onFileChange() {
  var maxFileSizeInMegabytes = document.getElementById('MaxFileSizeInMegabytes').value;
  var maxFileSize = maxFileSizeInMegabytes * 1024 * 1024;
  var uploadControl = document.getElementById('File');
  var errorForm = document.getElementById('file-error-form');
  var errorMessages = document.getElementById('error-messages');
  var continueButton = document.getElementById('continue-button');

  if (!errorMessages) {
    return;
  }
  else {
    errorMessages.innerHTML = "";
    errorForm.classList.remove("govuk-form-group--error");
    continueButton.disabled = false;
    clearValidationSummary();
  }

  if (uploadControl.files.length > 0) {
    for (let i = 0; i < uploadControl.files.length; i++) {
      if (uploadControl.files[i].size > maxFileSize) {
        var errorMessage = `The selected file ${uploadControl.files[i].name} must be smaller than or equal to ${maxFileSizeInMegabytes}MB`
        errorMessages.innerHTML += `<div>${errorMessage}</div>`;
        errorForm.classList.add("govuk-form-group--error");
        continueButton.disabled = true;
        addValidationSummaryMessage(errorMessage);
      }
    }
  }
}

function clearValidationSummary() {
  var el = document.getElementById('validation-summary');
  el.insertAdjacentHTML('afterend', '<div id="validation-summary"></div>')
  el.remove();
}

function addValidationSummaryMessage(message) {
  var el = document.getElementById('validation-summary');
  if (!el.innerHTML) {
    el.innerHTML = `<div aria-labelledby="error-summary-title" role="alert" tabindex="-1" data-module="govuk-error-summary" id="validation-summary" class="govuk-error-summary">
                      <h2 id="error-summary-title" class="govuk-error-summary__title">
                        There is a problem
                      </h2>
                      <div class="govuk-error-summary__body">
                        <div data-valmsg-summary="true" id="validation-error-list" class="govuk-error-message">
                          <ul class="govuk-list govuk-error-summary__list"></ul>
                        </div>
                      </div>
                    </div>`;
  }
  el.querySelector(".govuk-list.govuk-error-summary__list")
    .insertAdjacentHTML('beforeend', `<li><a href="#OrganisationMoreInformationFile">${message}</a></li>`)
}

document.getElementById("File").addEventListener("change", onFileChange);
