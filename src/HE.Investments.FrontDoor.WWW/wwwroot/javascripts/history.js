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
