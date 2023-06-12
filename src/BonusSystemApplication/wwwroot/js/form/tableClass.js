/**
 * maintains the table with Objectives and Results
 * 
 */

class Table {

  static totalRows = 10;

  static baseObjectivesResults = "ObjectivesResults"
  static baseObjectives = "Objective";
  static baseResults = "Result";

  static areObjectivesFrozenId = "js-areObjectivesFrozen";
  static areResultsFrozenId = "js-areResultsFrozen";

  static row = "Row";
  static statement = "Statement";
  static description = "Description";
  static isKey = "IsKey";
  static isMeasurable = "IsMeasurable";
  static unit = "Unit";
  static threshold = "Threshold";
  static target = "Target";
  static challenge = "Challenge";
  static weightFactor = "WeightFactor";
  static kpiUpperLimit = "KpiUpperLimit";
  static keyCheck = "KeyCheck";
  static achieved = "Achieved";
  static kpi = "Kpi";

  static bodyId = "js-table";
  static kpiSumId = "js-kpiSum";
  static weightFactorSumId = "js-weightFactorSum";
  static kpiUpperLimitSumId = "js-kpiUpperLimitSum";

  static backgroundColorNotEditable = "rgb(235, 235, 235)";
  static backgroundColorError = "rgb(255, 204, 204)";
  static backgroundColor = "rgb(220, 245, 255)";
  static fontColorError = "rgb(251, 28, 28)";
  static fontWeightError = "Bold";
  static fontSizeSmall = "12px";


  static #checkStringCellId(cellId, deliminator) {
    if (typeof cellId !== "string") {
      throw new Error("cellId is not a string");
    }

    if (!cellId) {
      throw new Error("cellId is empty");
    }

    if (!deliminator == null) {
      const parts = cellId.split(deliminator);
      if (!parts.length && parts.length <= 1) {
        throw new Error("cellId: " + cellId + " has no deliminator: " + deliminator);
      }
    }

    return;
  }

  /**
   * creates Id of cell by column name and row number
   * @param {string} columnName - the name of column cell belongs to
   * @param {number} rowNumber - the number of row cell belong to
   */
  static getCellId(columnName, rowNumber) {
    if (columnName == this.row) {
      return (this.baseObjectivesResults + "_" + rowNumber + "__" + columnName);
    } else if(columnName === this.keyCheck || columnName === this.achieved || columnName === this.kpi) {
      return (this.baseObjectivesResults + "_" + rowNumber + "__" + this.baseResults + "_" + columnName);
    } else {
      return (this.baseObjectivesResults + "_" + rowNumber + "__" + this.baseObjectives + "_" + columnName);
    }
  }


  static getValue(columnName, rowNumber) {
    const cellId = this.getCellId(columnName, rowNumber);

    const element = document.getElementById(cellId);
    if (element == null) {
      throw new Error("Element with cellId: " + cellId + " could not be found");
    }

    const value = document.getElementById(cellId).value;
    return value;
  }


  static getValueById(cellId) {
    try {
      this.#checkStringCellId(cellId);
    }
    catch (e) {
      throw e;
    }

    const element = document.getElementById(cellId);
    if (element == null) {
      throw new Error("Element with cellId: " + cellId + " could not be found");
    }

    return element.value;
  }

  /**
   * returns cell's row number by its Id
   * @param {string} cellId - id of table <td> tag
   */
  static getRowNumberById(cellId) {
    const deliminator = '_';

    try {
      this.#checkStringCellId(cellId, deliminator);
    }
    catch (e) {
      throw e;
    }

    const parts = cellId.split(deliminator);
    const rowNumber = Number.parseInt(parts[1]);
    return rowNumber;
  }

  /**
   * returns cell's column name by its Id
   * @param {string} cellId - id of table <td> tag
   */
  static getColumnNameById(cellId) {
    const deliminator = '_';

    try {
      this.#checkStringCellId(cellId, deliminator);
    }
    catch (e) {
      throw e;
    }

    const parts = cellId.split(deliminator);
    const columnName = parts[parts.length - 1];
    return columnName;
  }


  static isKeyChecked(row) {

    const cellId = this.getCellId(this.isKey, row);
    try {
      const isChecked = this.#isCheckBoxChecked(cellId);
      return isChecked;
    }
    catch (e) {
      throw e;
    }
  }


  static isMeasurableChecked(row) {

    const cellId = this.getCellId(this.isMeasurable, row);
    try {
      const isChecked = this.#isCheckBoxChecked(cellId);
      return isChecked;
    }
    catch (e) {
      throw e;
    }
  }


  static #isCheckBoxChecked(cellId) {

    try {
      this.#checkStringCellId(cellId);
    }
    catch (e) {
      throw e;
    }

    const element = document.getElementById(cellId);
    if (element == null) {
      throw new Error("Element with cellId: " + cellId + " could not be found");
    }

    if (element.type !== "checkbox") {
      throw new Error("Element with cellId: " + cellId + " is not a checkbox");
    }

    const isChecked = element.checked;
    return isChecked;
  }


  static setValue(cellId, valueToSet) {
    try {
      this.#checkStringCellId(cellId);
    }
    catch (e) {
      throw e;
    }

    const element = document.getElementById(cellId);
    if (element == null) {
      throw new Error("Element with cellId: " + cellId + " could not be found");
    }

    if (valueToSet == null) {
      throw new Error("Can't set null/undefined value for element with cellId: " + cellId);
    }

    element.value = valueToSet;

    this.setCellFontSize(cellId)

    if (element.nodeName === "TEXTAREA") {
      resizeTextArea(element);
    }

    this.setCellBackgroundColor(cellId);

    return;
  }


  static setCellFontColorWeight(cellId, isContentError) {

    let tdElement = document.getElementById(cellId).parentElement;

    if (isContentError === true) {
      if ($(tdElement).css("color") !== this.fontColorError) {
        $(tdElement).css({ "color": this.fontColorError, "font-weight": this.fontWeightError });
      }
    } else if (isContentError === false) {
      if ($(tdElement).css("color") === this.fontColorError) {
        $(tdElement).css({ "color": "", "font-weight": "" });
      }
    } else { return; }
  }


  static setCellBackgroundColor(cellId) {

    if (this.#isObjective(cellId)) {
      const areObjectivesFrozen = Table.getValueById(this.areObjectivesFrozenId);

      if (areObjectivesFrozen === "True") {
        this.#setCellBackgroundColorAsNotEditable(cellId);
      } else if (areObjectivesFrozen === "False") {

        if (this.#isEmpty(cellId)) {
          this.#setCellBackgroundColorAsEditable(cellId, true);
        } else {
          this.#setCellBackgroundColorAsEditable(cellId, false);
        }
      }
    } else if (this.#isResult(cellId)) {
      const areResultsFrozen = Table.getValueById(this.areResultsFrozenId);

      if (areResultsFrozen === "True") {
        this.#setCellBackgroundColorAsNotEditable(cellId);
      } else if (areResultsFrozen === "False") {

        if (this.#isEmpty(cellId)) {
          this.#setCellBackgroundColorAsEditable(cellId, true);
        } else {
          this.#setCellBackgroundColorAsEditable(cellId, false);
        }
      }
    }

    return;
  }

  static #isEmpty(cellId) {
    if (!Table.getValueById(cellId)) {
      return true;
    } else {
      return false;
    }
  }

  static #setCellBackgroundColorAsEditable(cellId, isError) {

    if (cellId === this.weightFactorSumId ||
        cellId === this.kpiUpperLimitSumId ||
        cellId === this.kpiSumId) {
      return;
    }

    const cellColumnName = this.getColumnNameById(cellId);
    if (cellColumnName === this.row ||
        cellColumnName === this.isKey ||
        cellColumnName === this.isMeasurable ||
        cellColumnName === this.kpi ||
        cellColumnName === this.keyCheck) {
      return;
    }

    let tdElement = document.getElementById(cellId).parentElement;

    if (isError === true) {
      if ($(tdElement).css("background-color") !== this.backgroundColorError) {
        $(tdElement).css("background-color", this.backgroundColorError);
      }
    } else if (isError === false) {
      if ($(tdElement).css("background-color") !== this.backgroundColor) {
        $(tdElement).css("background-color", this.backgroundColor);
      }
    } else { return; }
  }


  static #setCellBackgroundColorAsNotEditable(cellId) {

    let tdElement = document.getElementById(cellId).parentElement;

    if ($(tdElement).css("background-color") !== this.backgroundColorNotEditable) {
      $(tdElement).css("background-color", this.backgroundColorNotEditable);
    } else { return; }
  }


  static setCellFontSize(cellId) {
    const element = document.getElementById(cellId);
    const tdElement = element.parentElement;

    if (element.value.length > 10) {
      if ($(tdElement).css("font-size") !== this.fontSizeSmall) {
        $(tdElement).css("font-size", this.fontSizeSmall);
      }
    } else {
      if ($(tdElement).css("font-size") == this.fontSizeSmall) {
        $(tdElement).css("font-size", "");
      }
    }
  }

  static #isObjective(cellId) {
    const cellColumnName = this.getColumnNameById(cellId);
    if (cellColumnName === this.row ||
        cellColumnName === this.statement ||
        cellColumnName === this.description ||
        cellColumnName === this.isKey ||
        cellColumnName === this.isMeasurable ||
        cellColumnName === this.unit ||
        cellColumnName === this.threshold ||
        cellColumnName === this.target ||
        cellColumnName === this.challenge ||
        cellColumnName === this.weightFactor ||
        cellColumnName === this.kpiUpperLimit) {
      return true;
    }
    return false;
  }

  static #isResult(cellId) {
    const cellColumnName = this.getColumnNameById(cellId);
    if (cellColumnName === this.keyCheck ||
        cellColumnName === this.achieved ||
        cellColumnName === this.kpi) {
      return true;
    }
    return false;
  }
}
