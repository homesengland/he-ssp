function onFileChange() {
  const maxFileSizeInMegabytes = document.getElementById('MaxFileSizeInMegabytes').value;
  const allowedExtensions = document.getElementById('AllowedExtensions').value.split(';');

  const maxFileSize = maxFileSizeInMegabytes * 1024 * 1024;
  const uploadControl = document.getElementById('File');
  const errorForm = document.getElementById('file-error-form');
  const errorMessages = document.getElementById('error-messages');
  const continueButton = document.getElementById('continue-button');

  console.log(allowedExtensions)

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
      let file = uploadControl.files[i];
      console.log(file);
      if (file.size > maxFileSize) {
        let errorMessage = `The selected file ${file.name} must be smaller than or equal to ${maxFileSizeInMegabytes}MB`
        errorMessages.innerHTML += `<div>${errorMessage}</div>`;
        errorForm.classList.add("govuk-form-group--error");
        continueButton.disabled = true;
        addValidationSummaryMessage(errorMessage);
      }

      if (!allowedExtensions.includes(getFileExtension(file.name)))
      {
        let errorMessage = `The selected file ${file.name} must be a PDF, Word Doc, JPEG or RTF`
        errorMessages.innerHTML += `<div>${errorMessage}</div>`;
        errorForm.classList.add("govuk-form-group--error");
        continueButton.disabled = true;
        addValidationSummaryMessage(errorMessage);
      }
    }
  }
}

function clearValidationSummary() {
  let el = document.getElementById('validation-summary');
  el.insertAdjacentHTML('afterend', '<div id="validation-summary"></div>')
  el.remove();
}

function addValidationSummaryMessage(message) {
  let el = document.getElementById('validation-summary');
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

function getFileExtension(fileName)
{
  return fileName.substring(fileName.lastIndexOf('.'));
}

document.getElementById("File").addEventListener("change", onFileChange);
