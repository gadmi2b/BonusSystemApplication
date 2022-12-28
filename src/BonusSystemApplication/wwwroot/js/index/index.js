/**
 * when Open botton is clicked:
 * gets values of all selected checkboxes = form ids
 * opens a new tab in a browser for each id and calls Controller/Action/id url
 * >> Its request from business to open several forms at once << 
 * >> POPUPs in browser should be allowed for this functionality <<
 * 
 * >> UPD: 14.09.22 functionality was cancelled
 */

//$(document).ready(function () {
//  $("#js-open-button").click(function () {
//    // get all ids of selected checkboxes
//    let checkboxValues = $($('.js-checkbox:checkbox:checked')).map(function () { return this.value; }).get();

//    // for each selected formId call Form action with id and open new view in new tab
//    for (let i = 0; i < checkboxValues.length; i++) {
//      window.open('/Home/Form/' + checkboxValues[i]);
//    }
//  });
//});

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


$(document).ready(function () {
  $('.js-unopenable').click(function () {
    event.stopPropagation();
  });
});


$(document).ready(function () {
  $(".content-table").on('click','tr',function(e){
    e.preventDefault();
    var id = $(this).attr('id');
    alert("Clicked = " + id);
    var $form = $('#js-tableForm');
    // set the input value
    var elem = $form.find('input');
    elem[0].value = id;

    $form.submit();
  });
});
