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
        $("#js-workprojectDescription").val(response.workprojectDescription);
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
    url: '/Home/GetEmployeeData',
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


function changeState() {
  let formId = $("#ObjectivesDefinition_FormId").val();
  // TODO: current signatures will be dropped - to ask user
  //       positive answer - run ajax request

  $.ajax({
    type: "Get",
    url: '/Home/ChangeState',
    data: { formId: formId },
    dataType: "json",
    contentType: "application/json; charset=utf-8",
    success: function (response) {
      if (response.status !== "success") {
        alert("Exception: " + response.status + " |  " + response.message);
      }
      else {
        $("#js-teamName").val(response.employeeTeam);
        $("#js-positionName").val(response.employeePosition);
        $("#js-pid").val(response.employeePid);
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