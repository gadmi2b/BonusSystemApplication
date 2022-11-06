function requestWorkprojectDescription(element) {

  // --- to send an object ---
  //let data = {
  //  selectedId: element.value,
  //}
  //let requestData = JSON.stringify(data);

  $.ajax({
    type: "Get",
    url: '/Home/GetWorkprojectDescription',
    data: { workprojectId: element.value },
    dataType: "json",
    contentType: "application/json; charset=utf-8",
    //beforeSend: function () {
    //  Show(); // Show loader icon  
    //},
    success: function (response) {
      if (response.status !== "success") {
        alert("Exception: " + response.status + " |  " + response.message);
      }
      else {
        // TODO: change workproject description
        alert(response.workprojectDescription);
      }
    },
    failure: function (response) {
      alert("Failure: " + response.Status + " |  " + response.Message);
    },
    error: function (response) {
      alert("Error: " + response.Status + " |  " + response.Message);
    }
  });
}