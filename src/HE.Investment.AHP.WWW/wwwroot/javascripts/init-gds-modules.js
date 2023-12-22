(() => {
    const initModule = (Module, selector) => {
        var components = document.querySelectorAll(`[data-module="${selector}"]`)
        components.forEach(item => new Module(item).init())
    }

    initModule(window.GOVUKFrontend.CharacterCount, "govuk-character-count");

})();
