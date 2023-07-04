function validateFileSize() {

    const maxFileSize = 20 * 1024 * 1024;
    var uploadControl = document.getElementById('File');
    var errorForm = document.getElementById('file-error-form');
    var errorMessageControl = document.getElementById('client-error-message');
    var continueButton = document.getElementById('continue-button');

    if (!errorMessageControl) {
        return;
    }

    if (uploadControl.files.length > 0 && uploadControl.files[0].size > maxFileSize) {
        errorMessageControl.innerText = "The selected file must be smaller than or equal to 20MB";
        errorForm.classList.add("govuk-form-group--error");
        continueButton.disabled = true;
    }
    else {
        errorMessageControl.innerText = "";
        errorForm.classList.remove("govuk-form-group--error");
        continueButton.disabled = false;
    }
}

document.getElementById("File").addEventListener("change", validateFileSize);