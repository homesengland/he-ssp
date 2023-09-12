function setUpBackLink() {
    let backLink = document.getElementById("js-back-link");

    backLink.onclick = function () {
        window.history.back();
    }
}

document.addEventListener("DOMContentLoaded", function (event) {
    setUpBackLink();
});
