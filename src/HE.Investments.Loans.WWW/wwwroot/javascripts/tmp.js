var activeConditionalInput = null;

const onChange = (radio, thisInput) => () => {
    if (radio.checked) {
        if (activeConditionalInput !== null) {
            activeConditionalInput.classList.add("govuk-radios__conditional--hidden");
        }
        else if (thisInput !== null) {
            thisInput.classList.remove("govuk-radios__conditional--hidden");

            activeConditionalInput = thisInput;
        }
    }
}

const radiosTuples = [{ radioId: 'yesRadioId', conditionalId: '@yesRadioId-conditional' }]

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
