var activeConditionalInput = null;

const onChange = (radio, thisInput) => () => {
    if (radio.checked) {
        if (activeConditionalInput !== null) {
            activeConditionalInput.classList.add("govuk-radios__conditional--hidden");
            activeConditionalInput = null;
        }

        if (thisInput !== null) {
            thisInput.classList.remove("govuk-radios__conditional--hidden");

            activeConditionalInput = thisInput;
        }
    }
}

const radiosTuples = Array.from(document.getElementsByName('radio')).map(v => JSON.parse(v.value))

const radios = radiosTuples.map(
    ({ radioId, conditionalId }) =>
    ({
        radio: document.getElementById(radioId),
        conditionalInput: document.getElementById(conditionalId)
    }))

radios.forEach(
    ({ radio, conditionalInput }) => {
        radio.addEventListener('change', onChange(radio, conditionalInput));

        onChange(radio, conditionalInput)();
    });