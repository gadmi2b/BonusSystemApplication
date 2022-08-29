$(document).ready(function () {
  runTests();
});

function runTests() {
  console.log("testTableClass-GetCellId: " + testTableClassGetCellId());
  console.log("testTableClass-GetRowNumberById: " + testTableClassGetRowNumberById());
  console.log("testTableClass-GetColumnNameById: " + testTableClassGetColumnNameById());
  console.log("testTableClass-IsKeyChecked: " + testTableClassIsKeyChecked());
  console.log("testTableClass-IsMeasurableChecked: " + testTableClassIsMeasurableChecked());
  console.log("testTableClass-SetValue: " + testTableClassSetValue());
}

function testTableClassGetCellId() {
  //arrange
  const expected = "ObjectivesResults_3__Row";

  //act
  const actual = Table.getCellId(Table.row, 3);

  //asset
  if (expected === actual) {
    return true;
  } else {
    return false;
  }
}

function testTableClassGetRowNumberById() {
  //arrange
  const expected = 5;

  //act
  const actual = Table.getRowNumberById("ObjectivesResults_5__Target");

  //asset
  if (expected === actual) {
    return true;
  } else {
    return false;
  }
}

function testTableClassGetColumnNameById() {
  //arrange
  const expected = "Target";

  //act
  const actual = Table.getColumnNameById("ObjectivesResults_5__Target");

  //asset
  if (expected === actual) {
    return true;
  } else {
    return false;
  }
}

function testTableClassIsKeyChecked() {
  //arrange
  const expected = false;

  //act
  const actual = Table.isKeyChecked(4);

  //asset
  if (expected === actual) {
    return true;
  } else {
    return false;
  }
}

function testTableClassIsMeasurableChecked() {
  //arrange
  const expected = true;

  //act
  const actual = Table.isMeasurableChecked(0);

  //asset
  if (expected === actual) {
    return true;
  } else {
    return false;
  }
}

function testTableClassSetValue() {
  //arrange
  const expected = "55";

  //act
  Table.setValue("ObjectivesResults_9__Target", 55);
  const actual = document.getElementById("ObjectivesResults_9__Target").value;

  //asset
  if (expected === actual) {
    return true;
  } else {
    return false;
  }
}