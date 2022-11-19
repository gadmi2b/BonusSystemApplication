$(document).ready(function () {
  $('.js-signature').change(function (event) {
    
    //event.target.id - id of changed checkbox
    //event.target.checked - new value of changed checkbox

    // TODO: call ajax request and send: id and value of checkbox
    //       all signature block should be locked until server responce
    //       responce should contain: success, id of affected signature block, signature (if target.value == false)
    //                                         id and string.empty signature             (if target.value == true) --- !need to drop reject also!
    //                                error,   message, => return checkbox value back

    //       (in the case of reject): success, id of affected signature block, signature (if reject was before main signature) (if target.value == false)
    //       (in the case of reject): success, id = string.empty (do nothing)            (if reject was after main signature)  (if target.value == false)
    //       (in the case of reject): success, id = string.empty (do nothing)            (in any cased) (if target.value == true)
    //                                error,   message, => return checkbox value back

    let formId = $("#js-formId").val();
    let signatureCheckboxId = event.target.id;
    let isSignatureCheckboxChecked = event.target.checked;


    $.ajax({
      type: "Get",
      url: "/Home/SignatureProcess",
      data: {
        formId: formId,
        checkboxId: signatureCheckboxId,
        ischeckboxChecked: isSignatureCheckboxChecked,
      },
      dataType: "json",
      contentType: "application/json; charset=utf-8",

      success: function(response) {
        if (response.status !== "success") {
          document.getElementById(signatureCheckboxId).checked = !isSignatureCheckboxChecked
          alert(response.status + " |  " + response.message);
        } else {
          for (let property in response.propertiesValues) {
            let propertyID = '#' + property;
            let propertyValue = response.propertiesValues[property];

            if (typeof propertyValue === "boolean") {
              $(propertyID).prop("checked", propertyValue);
            } else if (typeof propertyValue === "string") {
              $(propertyID).text(propertyValue);
            }
          }
        }
      },
      failure: function (response) {
        alert("Failure: " + response.status + " |  " + response.message);
      },
      error: function() {
        alert("Error: " + response.status + " |  " + response.message);
      },
    });

  });
});