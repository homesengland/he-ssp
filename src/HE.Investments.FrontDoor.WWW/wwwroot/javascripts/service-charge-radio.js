function setUpServiceChargeRadio() {
    var radios = document.getElementsByName("DoYouKnowTheServiceCharge");
    var serviceChargeField = document.getElementById("serviceChargeField");
    var serviceCharge = document.getElementById("ServiceCharge");
    var checkedValue = "null";

    for (let i = 0; i < radios.length; i++) {
        const radio = radios[i];
        radio.onclick = function () {
            if (radio.value === "1") {
                serviceChargeField.style.display = 'block';
            }
            else {
                serviceChargeField.style.display = 'none';
                serviceCharge.value = "";
                clearErrors();
            }
        }

        if (radio.checked) {
            checkedValue = radio.value;
        }
    }

    if (checkedValue === "1") {
        serviceChargeField.style.display = 'block';
    }
    else {
        serviceChargeField.style.display = 'none';
    }
}

document.addEventListener('DOMContentLoaded', function(event) {
    setUpServiceChargeRadio();
});
