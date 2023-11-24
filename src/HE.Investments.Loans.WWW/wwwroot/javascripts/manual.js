function handleSummaryHighlight() {
    const isCheckDetails = getParameterByName("IsCheckDetails");
    const changedField = getParameterByName("ChangedField");

    if (changedField !== null) {
        document.getElementById(changedField).focus();
    } else if (isCheckDetails != null && isCheckDetails === "True") {
        let form = document.querySelector("form");
        let input = form.querySelector("input:not([type='hidden'])");

        if (input) {
            input.focus();
        }
    }
}

function getParameterByName(name, url) {
    url = url || window.location.href;
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}

document.addEventListener("DOMContentLoaded", function(event) {
    handleSummaryHighlight();
});

document.getElementById("skipQuestionLink").addEventListener("click", function(event) {
    document.forms[0].submit();
});

function clearClasses(className) {
    const errorGroup = document.getElementsByClassName(className);

    for (let field in errorGroup) {
        if (errorGroup[field].classList)
            errorGroup[field].classList.remove(className);
    }
}

function clearErrors() {
    const summary = document.getElementById("validation-summary");
    if (summary) {
        summary.style.display = "none";
    }

    clearClasses("govuk-form-group--error");
    clearClasses("govuk-input--error");

    const errorFields = document.getElementsByClassName("govuk-error-message");
    for (let field in errorFields) {
        const errorField = errorFields[field];
        if (errorField && errorField.style && errorField.style.display)
            errorFields[field].style.display = "none";
    }
};
