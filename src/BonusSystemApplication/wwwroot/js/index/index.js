/**
 * disables button if amount of checked checkboxes > 1
 * enables button if amount of checked checkboxes = 0
 */
$(document).ready(function () {
  $(".js-checkbox").click(function () {
    let checkboxChecked = $('.js-checkbox:checkbox:checked');
    let createButtonElement = document.getElementById("js-create-button");

    if (checkboxChecked.length > 1) {
      disableElement(createButtonElement);
    } else {
      enableElement(createButtonElement);
    }
  });
});

function disableElement(elem) {
  if (elem.disabled === false) {
    elem.disabled = true;
  }
}

function enableElement(elem) {
  if (elem.disabled === true) {
    elem.disabled = false;
  }
}
