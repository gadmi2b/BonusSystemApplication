$(document).ready(function () {
  $('.js-signature').change(function (event) {
    
    let formId = $("#js-formId").val();
    let signatureCheckboxId = event.target.id;
    let isSignatureCheckboxChecked = event.target.checked;


    $.ajax({
      type: "Get",
      url: "/Form/SignatureProcess",
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
            let propertyID = property;
            let propertyValue = response.propertiesValues[property];

            if (typeof propertyValue === "boolean") {
              document.getElementById(propertyID).checked = propertyValue;
            } else if (typeof propertyValue === "string") {
              document.getElementById(propertyID).innerHTML = propertyValue;
            }
          }
        }
      },
      failure: function (response) {
        alert("Failure: " + response.status + " | " + response.message);
      },
      error: function() {
        alert("Error: " + response.status + " | " + response.message);
      },
    });

  });
});