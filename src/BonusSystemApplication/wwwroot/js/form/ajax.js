function requestWorkprojectDescription(element) {

  let data = {
    selectedId: element.value,
  }

  let requestData = JSON.stringify(data);

  $.ajax({
    type: "POST",
    url: '/Home/GetWorkprojectDescription',
    data: { selectedData: requestData },
    dataType: "json",
    contentType: "application/json; charset=utf-8",

    success: function (response) {
      if (response.Status !== "OK") {
        alert("Exception: " + response.Status + " |  " + response.Message);
      }
      else {
        // TODO: change workproject description
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