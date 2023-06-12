  // --- to send an object ---
  //let data = {
  //  selectedId: element.value,
  //}
  //let requestData = JSON.stringify(data);

function requestWorkprojectDescription(element) {
  $.ajax({
    type: "Get",
    url: '/Form/GetWorkprojectDescription',
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
        $("#js-workprojectDescription").val(response.workprojectDescription);
        resizeWorkprojectDescriptionInput();
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


function requestEmployeeData(element) {
  $.ajax({
    type: "Get",
    url: '/Form/GetEmployeeData',
    data: { employeeId: element.value },
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
        $("#js-teamName").val(response.employeeTeam);
        $("#js-positionName").val(response.employeePosition);
        $("#js-pid").val(response.employeePid);
        resizeEmployeeInformationInputs();
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