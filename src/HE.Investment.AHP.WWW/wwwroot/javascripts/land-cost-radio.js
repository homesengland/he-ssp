function setUpBuyLandRadio() {
    var radios = document.getElementsByName("DoYouNeedToBuyTheLand");
    var landCostField = document.getElementById("landCostField");
    var landCostNoWarningField = document.getElementById("landCostNoWarningField");
    var landCost = document.getElementById("LandCosts");
    var checkedValue = "null";

    for (let i = 0; i < radios.length; i++) {
        const radio = radios[i];
        radio.onclick = function () {
            if (radio.value === "1") {
                landCostField.style.display = "block";
                landCostNoWarningField.style.display = "none";
            }
            else {
                landCostField.style.display = "none";
                landCostNoWarningField.style.display = "block";
                landCost.value = "";
            }
        }

        if (radio.checked) {
            checkedValue = radio.value;
        }
    }

    if (checkedValue === "1") {
        landCostField.style.display = "block";
        landCostNoWarningField.style.display = "none";
    }
    else if (checkedValue === "2") {
        landCostField.style.display = "none";
        landCostNoWarningField.style.display = "block";
    }
    else {
        landCostField.style.display = "none";
        landCostNoWarningField.style.display = "none";
    }
}

document.addEventListener("DOMContentLoaded", function(event) {
    setUpBuyLandRadio();
});
