function onFileChange() {
  var maxFileSizeInMegabytes = document.getElementById('MaxFileSizeInMegabytes').value;
  var maxFileSize = maxFileSizeInMegabytes * 1024 * 1024;
  var uploadControl = document.getElementById('File');
  var errorForm = document.getElementById('file-error-form');
  var errorMessageControl = document.getElementById('client-error-message');
  var continueButton = document.getElementById('continue-button');

  if (!errorMessageControl) {
    return;
  }
  else {
    errorMessageControl.innerHTML = "";
    errorForm.classList.remove("govuk-form-group--error");
    continueButton.disabled = false;
  }

  if (uploadControl.files.length > 0) {
    for (let i = 0; i < uploadControl.files.length; i++) {
      if (uploadControl.files[i].size > maxFileSize) {
        errorMessageControl.innerHTML += `<div>The selected file ${uploadControl.files[i].name} must be smaller than or equal to ${maxFileSizeInMegabytes}MB</div>`;
        errorForm.classList.add("govuk-form-group--error");
        continueButton.disabled = true;
      }
    }
  }
}

document.getElementById("File").addEventListener("change", onFileChange);
