(() => {
  const setUpBackLink = () => {
    const backLink = document.getElementById("js-back-link");
    if (backLink) {
      backLink.onclick = () => window.history.back();
    }
  }

  document.addEventListener("DOMContentLoaded", function () {
    setUpBackLink();
  });
})();

(() => {
    const initModule = (Module, selector) => {
        var components = document.querySelectorAll(`[data-module="${selector}"]`)
        components.forEach(item => new Module(item).init())
    }

    initModule(window.GOVUKFrontend.Button, "govuk-button");
})();
