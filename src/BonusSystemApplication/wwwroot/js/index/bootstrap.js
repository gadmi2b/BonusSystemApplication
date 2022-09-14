//Initialize the multiselect plugin:

$(document).ready(function() {
  $('.js-bootstrap-multiselect').multiselect({
    enableFiltering: true,
    includeResetOption: true,
    buttonWidth: '200px',
    maxHeight: 200,
    numberDisplayed: 1,
    visibility: true
  });
});