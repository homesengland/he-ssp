(() => {
  const setUpBannerButtons = () => {
    var banners = document.getElementsByClassName("govuk-cookie-banner");
    if(banners.length !== 1){
      return;
    }

    var policyAcknowledged = getCookie('accept_additional_cookies');
    if(policyAcknowledged !== undefined){
      banners[0].remove();
      return;
    }

    banners[0].classList.remove('govuk-!-display-none');
    var buttons = banners[0].getElementsByTagName("button");

    for (let button of buttons) {
      button.onclick = () => {
        setCookie(button.value);
        banners[0].remove();}
    }
  }

  const setCookie =(value) => {
    const data = new Date();
    data.setTime(data.getTime() + (365 * 24*60*60*1000));
    document.cookie = "accept_additional_cookies=".concat(value, '; expires=', data.toUTCString(), '; path=/; secure');
  }

  const getCookie = (name) => {
    if (document.cookie !== "") {
      const cookies = document.cookie.split(/; */);

      for (let cookie of cookies) {
        const [ cookieName, cookieVal ] = cookie.split("=");
        if (cookieName === decodeURIComponent(name)) {
          return decodeURIComponent(cookieVal);
        }
      }
    }

    return undefined;
  }

  document.addEventListener("DOMContentLoaded", function(event) {
    setUpBannerButtons();
  });

})();

(() => {
    const initModule = (Module, selector) => {
        var components = document.querySelectorAll(`[data-module="${selector}"]`)
        components.forEach(item => new Module(item).init())
    }

  initModule(window.GOVUKFrontend.CharacterCount, "govuk-character-count");

})();
