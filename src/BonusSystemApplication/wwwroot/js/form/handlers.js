/**
 * Type Checking: Actual types:
 * 
 * String:            typeof variable === "string"
 * Number:            typeof variable === "number"
 * Boolean:           typeof variable === "boolean"
 * Object:            typeof variable === "object"
 * Array:             Array.isArray( arrayLikeObject )
 * Node:              elem.nodeType === 1
 * Null:              variable === null
 * null or undefined: variable == null
 * 
 * undefined Global Variables:  typeof variable === "undefined"
 * undefined Local Variables:   variable === undefined
 * undefined properties:        object.prop === undefined
 *                              object.hasOwnProperty( prop )
 *                              "prop" in object
 * 
 * array has length:        if ( array.length ) ... 
 * array is empty:          if ( !array.length ) ...
 * string is not empty:     if ( string ) ...
 * string _is_ empty:       if ( !string ) ...
 * check for boolean false: if ( foo === false ) ...
 * 
 */


/**
 * After document was loaded:
 * - resizes all document's textareas blocks to fit their content
 */
$(document).ready(function () {
  resizeAllTextAreas();
});

/**
 * Launchs ResizeTextArea() method for each textarea
 */
function resizeAllTextAreas() {
  const textAreaElems = document.getElementsByTagName('textarea');
  for (let i = 0; i < textAreaElems.length; i++) {
    resizeTextArea(textAreaElems[i]);
  }
}

/**
 * Calculates and changes the height of textarea according to its scrollheight
 * @param {object} element - textarea element to operate.
 */
function resizeTextArea(element) {
  element.style.height = 'auto';
  element.style.height = (element.scrollHeight) + 'px';
}



/**
 * resizes selects with class .content-employee to fit its content after change
 */
$(document).ready(function () {
  $('.content-employee select').change(function () {
    var text = $(this).find('option:selected').text();
    var $aux = $('<select/>').append($('<option/>').text(text));
    $(this).after($aux);
    $(this).width($aux.width());
    $aux.remove();
  }).change();
});



/**
 * resizes inputs width to fit their content
 */
$(document).ready(function () {
  resizeEmployeeInformationInputs();
  resizeWorkprojectDescriptionInput();
});

function resizeEmployeeInformationInputs() {
  resizeInputElement(document.getElementById('js-teamName'));
  resizeInputElement(document.getElementById('js-positionName'));
  resizeInputElement(document.getElementById('js-pid'));
}

function resizeWorkprojectDescriptionInput() {
  resizeInputElement(document.getElementById('js-workprojectDescription'));
}

function resizeInputElement(element) {
  const initialMinwidth = $(element).css('min-width');
  $(element).css('min-width', '0px');
  $(element).css('width', '0px');
  $(element).css('width', 1.1 * element.scrollWidth + 'px');
  $(element).css('min-width', initialMinwidth);
}