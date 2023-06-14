var activeConditionalInput = null;

const onChange = (checkbox, thisInput) => () => {
    if (checkbox.checked) {
        if (thisInput !== null) {
            thisInput.classList.remove("govuk-checkboxes__conditional--hidden");
        }
    }
    else {
        if (thisInput !== null) {
            thisInput.classList.add("govuk-checkboxes__conditional--hidden");
        }
    }
}

const checkboxesTuples = Array.from(document.getElementsByName('checkbox')).map(v => JSON.parse(v.value))

const checkboxes = checkboxesTuples.map(
    ({ checkboxId, conditionalId }) =>
    ({
        checkbox: document.getElementById(checkboxId),
        conditionalInput: document.getElementById(conditionalId)
    }))

checkboxes.forEach(
    ({ checkbox, conditionalInput }) => {
        checkbox.addEventListener('change', onChange(checkbox, conditionalInput));

        onChange(checkbox, conditionalInput)();
    });