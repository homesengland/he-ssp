(() => {
  const exclusiveCheckboxGroupClassName = "exclusive-checkbox-group";
  const exclusiveCheckboxGroupExclusiveClassName = `${exclusiveCheckboxGroupClassName}--exclusive-option`;

  const setUpExclusiveCheckbox = () => {
    const checkboxGroup = document.getElementsByClassName(exclusiveCheckboxGroupClassName);
    const exclusiveCheckboxGroup = document.getElementsByClassName(exclusiveCheckboxGroupExclusiveClassName);
    if (checkboxGroup.length !== 1 || exclusiveCheckboxGroup.length !== 1) {
      return;
    }

    addCheckboxChangeEventHandlers(checkboxGroup[0].getElementsByTagName("input"), exclusiveCheckboxGroupExclusiveClassName);
    addCheckboxChangeEventHandlers(exclusiveCheckboxGroup[0].getElementsByTagName("input"), exclusiveCheckboxGroupClassName);
  }

  const addCheckboxChangeEventHandlers = (checkboxes, checkboxesToDeselect) =>{
    for (let i = 0; i < checkboxes.length; i++) {
      checkboxes[i].addEventListener('change', (event) => {
        if (event.currentTarget.value) {
          deselectAllByClassName(checkboxesToDeselect);
        }
      });
    }
  }

  const deselectAllByClassName = (className) => {
    const checkboxes = document.getElementsByClassName(className).item(0).getElementsByTagName("input");
    for (let i = 0; i < checkboxes.length; i++) {
      checkboxes[i].checked = false;
    }
  }

  document.addEventListener("DOMContentLoaded", function() {
    setUpExclusiveCheckbox();
  });
})();
