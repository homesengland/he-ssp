function setUpBuyLandRadio() {
    var radios = document.getElementsByName("DoYouKnowYourMortgageTerm");
    var mortgageTermField = document.getElementById("mortgageTermField");
    var mortgageTerm2WarningField = document.getElementById("mortgageTermNoWarningField");
    var mortgageTerm = document.getElementById("MortgageTerm");
    var checkedValue = "null";

    for (let i = 0; i < radios.length; i++) {
        const radio = radios[i];
        radio.onclick = function () {
            if (radio.value === "1") {
                mortgageTermField.style.display = "block";
                mortgageTermNoWarningField.style.display = "none";
            }
            else {
                mortgageTermField.style.display = "none";
                mortgageTermNoWarningField.style.display = "block";
                mortgageTerm.value = "";
            }
        }

        if (radio.checked) {
            checkedValue = radio.value;
        }
    }

    if (checkedValue === "1") {
        mortgageTermField.style.display = "block";
        mortgageTermNoWarningField.style.display = "none";
    }
    else if (checkedValue === "2") {
        mortgageTermField.style.display = "none";
        mortgageTermNoWarningField.style.display = "block";
    }
    else {
        mortgageTermField.style.display = "none";
        mortgageTermNoWarningField.style.display = "none";
    }
}

document.addEventListener("DOMContentLoaded", function(event) {
    setUpBuyLandRadio();
});
