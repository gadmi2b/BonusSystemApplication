﻿$(document).ready(function () {
  $("#" + Table.bodyId + " input").on('change', tableModified);
  $("#" + Table.bodyId + " textarea").on('change', tableModified);

  for (let row = 0; row < Table.totalRows; row++) {

    Table.setCellBackgroundColor(Table.getCellId(Table.row, row));
    Table.setCellBackgroundColor(Table.getCellId(Table.statement, row));
    Table.setCellBackgroundColor(Table.getCellId(Table.description, row));
    Table.setCellBackgroundColor(Table.getCellId(Table.isKey, row));
    Table.setCellBackgroundColor(Table.getCellId(Table.isMeasurable, row));
    Table.setCellBackgroundColor(Table.getCellId(Table.unit, row));
    Table.setCellBackgroundColor(Table.getCellId(Table.threshold, row));
    Table.setCellBackgroundColor(Table.getCellId(Table.target, row));
    Table.setCellBackgroundColor(Table.getCellId(Table.challenge, row));
    Table.setCellBackgroundColor(Table.getCellId(Table.weightFactor, row));
    Table.setCellBackgroundColor(Table.getCellId(Table.kpiUpperLimit, row));
    Table.setCellBackgroundColor(Table.getCellId(Table.achieved, row));

    const keyCheckId = Table.getCellId(Table.keyCheck, row);
    Table.setCellBackgroundColor(keyCheckId);
    Table.setCellFontSize(keyCheckId);

    const kpiElemId = Table.getCellId(Table.kpi, row);
    Table.setCellBackgroundColor(kpiElemId);
    Table.setCellFontSize(kpiElemId);
  }

  calculateWeightFactorSum();
  calculateKpiUpperLimitSum();
  checkProposalForBonusPaymentState();
});

// ---------- Business logic: content-table -----------

function tableModified() {

  const affectedCellId = this.id;
  const affectedColumnName = Table.getColumnNameById(affectedCellId);
  const affectedRowNumber = Table.getRowNumberById(affectedCellId);

  switch (affectedColumnName) {
                                                    // following cells potentially affected by method:
    case Table.isKey:
      isKeyModified(affectedRowNumber);             // threshold, keyCheck
      keyCheckModification(affectedRowNumber);      // keyCheck
      calculateKpiSum();                            // kpiSum
      break;

    case Table.isMeasurable:
      isMeasurableModified(affectedRowNumber);      // unit, threshold, target, challaenge, achieved
      kpiUpperLimitModification(affectedRowNumber); // kpiUpperLimit
      keyCheckModification(affectedRowNumber);      // keyCheck
      kpiModification(affectedRowNumber);           // kpi
      calculateKpiSum();                            // kpiSum
      break;

    case Table.threshold:
      keyCheckModification(affectedRowNumber);      // keyCheck
      kpiModification(affectedRowNumber);           // kpi
      calculateKpiSum();                            // kpiSum
      break;

    case Table.target:
      keyCheckModification(affectedRowNumber);      // keyCheck
      kpiModification(affectedRowNumber);           // kpi
      calculateKpiSum();
      break;

    case Table.challenge:
      keyCheckModification(affectedRowNumber);      // keyCheck
      kpiModification(affectedRowNumber);           // kpi
      calculateKpiSum();                            // kpiSum
      break;

    case Table.weightFactor:
      weightFactorModified(affectedRowNumber);
      calculateWeightFactorSum();                   // weightFactorSum
      calculateKpiUpperLimitSum();                  // kpiUpperLimitSum
      calculateKpiSum();                            // kpiSum
      break;

    case Table.kpiUpperLimit:
      kpiUpperLimitModification(affectedRowNumber)  // kpiUpperLimit
      kpiModification(affectedRowNumber);           // kpi
      calculateKpiUpperLimitSum();                  // kpiUpperLimitSum
      calculateKpiSum();                            // kpiSum
      break;

    case Table.achieved:
      keyCheckModification(affectedRowNumber);      // keyCheck
      kpiModification(affectedRowNumber);           // kpi
      calculateKpiSum();                            // kpiSum
      break;

    default:
      //do nothing
      break;
  }

  Table.setCellBackgroundColor(affectedCellId)
  checkProposalForBonusPaymentState();
}


function isKeyModified(row) {

  const keyCheckId = Table.getCellId(Table.keyCheck, row);
  const thresholdId = Table.getCellId(Table.threshold, row);

  if (Table.isKeyChecked(row) === true) {
    if (Table.isMeasurableChecked(row) === false) {
      Table.setValue(thresholdId, "???");
    }
  }
  else {
    Table.setValue(keyCheckId, "N/A");

    if (Table.isMeasurableChecked(row) === false) {
      Table.setValue(thresholdId, "N/A");
    }
  }

  return;
}


function isMeasurableModified(row) {

  const unitId = Table.getCellId(Table.unit, row);
  const thresholdId = Table.getCellId(Table.threshold, row);
  const targetId = Table.getCellId(Table.target, row);
  const challengeId = Table.getCellId(Table.challenge, row);
  const achievedId = Table.getCellId(Table.achieved, row);

  if (Table.isMeasurableChecked(row) === true) {

    Table.setValue(unitId, "");
    Table.setValue(thresholdId, "");
    Table.setValue(targetId, "");
    Table.setValue(challengeId, "");
    Table.setValue(achievedId, "");

    //TODO: no restriction on Achieved value
    //TODO: help message: KPI Upper Limit value shall be between 108% and 120%
  } else {
    Table.setValue(unitId, "KPI value, %");

    if (Table.isKeyChecked(row) === true) {
      Table.setValue(thresholdId, "???");
    } else {
      Table.setValue(thresholdId, "N/A");
    }

    Table.setValue(targetId, "N/A");
    Table.setValue(challengeId, "N/A");
    Table.setValue(achievedId, "");

    //TODO: help message: Achieved value shall be between 0% and 115%.
    //TODO: help message: KPI Upper Limit value shall be between 108% and 115%.
  }
}


function weightFactorModified(row) {

  const weightFactorValue = Table.getValue(Table.weightFactor, row);
  const weightFactorId = Table.getCellId(Table.weightFactor, row);

  if (isNanCheck(+weightFactorValue)) {
    Table.setCellFontColorWeight(weightFactorId, true);
    return;
  }

  if (+weightFactorValue < 0 || +weightFactorValue > 40) {
    Table.setCellFontColorWeight(Table.getCellId(Table.weightFactor, row), true);
    return;
  }

  Table.setCellFontColorWeight(Table.getCellId(Table.weightFactor, row), false);
}

/**
 * modifies cell value in keyCheck column in a specified row
 * launched each time when value in one of following cells is modified:
 * Key, Measurable, Threshold, Target, Challenge, Achieved
 * 
 * @param {number} row - affected row number
 */
function keyCheckModification(row) {

  if (!Table.isKeyChecked(row)) { return; }

  const keyCheckId = Table.getCellId(Table.keyCheck, row);

  const thresholdValue = Table.getValue(Table.threshold, row);
  const targetValue = Table.getValue(Table.target, row);
  const challengeValue = Table.getValue(Table.challenge, row);
  const achievedValue = Table.getValue(Table.achieved, row);

  if (Table.isMeasurableChecked(row) === false) {

    // Data completeness check
    if (!thresholdValue || !achievedValue) {
      Table.setValue(keyCheckId, "???");
      return;
    }

    // Data correctness check (numbers?)
    if (isNanCheck(+thresholdValue) || isNanCheck(+achievedValue)) {
      Table.setValue(keyCheckId, "Error: Not a number");
      return;
    }

    // Calculation
    if(+achievedValue >= +thresholdValue) {
      Table.setValue(keyCheckId, "OK");
    } else {
      Table.setValue(keyCheckId, "KO");
    }
  } else {

    // Data completeness check
    if(!thresholdValue || !targetValue || !challengeValue || !achievedValue) {
      Table.setValue(keyCheckId, "???");
      return;
    }

    // Data correctness check (numbers?)
    if (isNanCheck(+thresholdValue) || isNanCheck(+targetValue) ||
        isNanCheck(+challengeValue) || isNanCheck(+achievedValue)) {

      Table.setValue(keyCheckId, "Error: Not a number");
      return;
    }

    // Data correctness check (monotonicity of sequence Threshold-Target-Challenge)
    if(+targetValue >= Math.max(+thresholdValue, +challengeValue) ||
       +targetValue <= Math.min(+thresholdValue, +challengeValue)) {

      Table.setValue(keyCheckId, "Error: Non monotonic");
      return;
    }

    // Calculation
    Table.setValue(keyCheckId, "KO");
    if(+thresholdValue < +challengeValue) {
      if (+achievedValue >= +thresholdValue) {
        Table.setValue(keyCheckId, "OK");
      }
    } else {
      if (+achievedValue <= +thresholdValue) {
        Table.setValue(keyCheckId, "OK");
      }
    }
  }

  return;
}

/**
 * modifies/calculates kpi cell value in specific row
 * launched each time when value in one of following cells is modified:
 * Measurable, Threshold, Target, Challenge, Achieved, KpiUpperLimit
 * @param {number} row - affected row number
 */
function kpiModification(row) {

  const kpiId = Table.getCellId(Table.kpi, row);

  const thresholdValue = Table.getValue(Table.threshold, row);
  const targetValue = Table.getValue(Table.target, row);
  const challengeValue = Table.getValue(Table.challenge, row);
  const achievedValue = Table.getValue(Table.achieved, row);
  const kpiUpperLimitValue = Table.getValue(Table.kpiUpperLimit, row);

  if (Table.isMeasurableChecked(row) === false) {

    // Data completeness check
    if (!achievedValue) {
      Table.setValue(kpiId, "");
      return;
    }

    // Data correctness check (numbers?)
    if (isNanCheck(+achievedValue) || isNanCheck(+kpiUpperLimitValue)) {
      Table.setValue(kpiId, "Error: Not a number");
      return;
    }

    // Max KPI = 115%
    let kpi = Math.max(Math.min(+achievedValue, +kpiUpperLimitValue, 115), 0);
    kpi = roundAndFormatValue(kpi, 2);
    Table.setValue(kpiId, checkForZero(kpi));

  } else {

    // Data completeness check
    if(!thresholdValue || !targetValue || !challengeValue || !achievedValue) {
      Table.setValue(kpiId, "");
      return;
    }

    // Data correctness check (numbers?)
    if (isNanCheck(+thresholdValue) || isNanCheck(+targetValue) ||
        isNanCheck(+challengeValue) || isNanCheck(+achievedValue) ||
        isNanCheck(+kpiUpperLimitValue)) {

      Table.setValue(kpiId, "Error: Not a number");
      return;
    }

    // Data correctness check (monotonicity of sequence Threshold-Target-Challenge)
    if (+targetValue >= Math.max(+thresholdValue, +challengeValue) ||
        +targetValue <= Math.min(+thresholdValue, +challengeValue)) {

      Table.setValue(kpiId, "Error: Non monotonic");
      return;
    }

    // Calculation
    let kpi = 0;
    if(+thresholdValue < +challengeValue) {
      if(+achievedValue < +targetValue) {
        kpi = 100 / (+targetValue - +thresholdValue) * (+achievedValue - +thresholdValue);
        if(kpi < 0) { kpi = 0; }
      } else {
        kpi = 100 * (1 + 0.2 / (+challengeValue - +targetValue) * (+achievedValue - +targetValue));
        if(kpi > 120) { kpi = 120; }
      }
    } else {
      if(+achievedValue > +targetValue) {
        kpi = 100 / (+targetValue - +thresholdValue) * (+achievedValue - +thresholdValue);
        if(kpi < 0) { kpi = 0; }
      } else {
        kpi = 100 * (1 + 0.2 / (+challengeValue - +targetValue) * (+achievedValue - +targetValue));
        if(kpi > 120) { kpi = 120; }
      }
    }

    // KPI Upper limit for objectivies. Max KPI = KPI_ULimit <= 120%
    kpi = Math.min(kpi, kpiUpperLimitValue);
    kpi = roundAndFormatValue(kpi, 2);
    Table.setValue(kpiId, checkForZero(kpi));
  }

  function checkForZero(calculatedKpi) {
    if (+calculatedKpi === 0) {
      return "";
    } else {
      return calculatedKpi;
    }
  }

  return;
}


function kpiUpperLimitModification(row) {

  const kpiUpperLimitId = Table.getCellId(Table.kpiUpperLimit, row);
  const kpiUpperLimitValue = Table.getValue(Table.kpiUpperLimit, row);

  if (isNanCheck(+kpiUpperLimitValue)) {
    Table.setCellFontColorWeight(kpiUpperLimitId, true);
    return;
  } else {
    Table.setCellFontColorWeight(kpiUpperLimitId, false);
  }

  if (Table.isMeasurableChecked(row) === true) {
    if (+kpiUpperLimitValue === 0) {
      Table.setValue(kpiUpperLimitId, 120);
    } else {
      let kpiUL = Math.min(120, Math.max(108, +kpiUpperLimitValue));
      Table.setValue(kpiUpperLimitId, kpiUL);
    }
  } else {
    if (+kpiUpperLimitValue === 120 || +kpiUpperLimitValue === 0) {
      Table.setValue(kpiUpperLimitId, 115);
    } else {
      let kpiUL = Math.min(115, Math.max(108, +kpiUpperLimitValue));
      Table.setValue(kpiUpperLimitId, kpiUL);
    }
  }

  return;
}


function calculateWeightFactorSum() {

  let calculatedWeightFactorSum = 0;
  for (let row = 0; row < Table.totalRows; row++) {
    const weightFactorValue = Table.getValue(Table.weightFactor, row);
    const weightFactorId = Table.getCellId(Table.weightFactor, row);

    if (isNanCheck(+weightFactorValue)) {
      Table.setCellFontColorWeight(weightFactorId, true);
      continue;
    } else {
      calculatedWeightFactorSum += +weightFactorValue;
    }
  }

  calculatedWeightFactorSum = roundAndFormatValue(calculatedWeightFactorSum, 1);
  Table.setValue(Table.weightFactorSumId, calculatedWeightFactorSum);

  if (+calculatedWeightFactorSum != 100) {
    Table.setCellFontColorWeight(Table.weightFactorSumId, true);
  } else {
    Table.setCellFontColorWeight(Table.weightFactorSumId, false);
  }

}


function calculateKpiUpperLimitSum() {
  let calculatedKpiUpperLimitSum = 0;
  for (let row = 0; row < Table.totalRows; row++) {
    const kpiUpperLimitId = Table.getCellId(Table.kpiUpperLimit, row);
    const weightFactorId = Table.getCellId(Table.weightFactor, row);

    const kpiUpperLimitValue = Table.getValue(Table.kpiUpperLimit, row);
    const weightFactorValue = Table.getValue(Table.weightFactor, row);

    let isNotANumber = false;
    if (isNanCheck(+kpiUpperLimitValue)) {
      Table.setCellFontColorWeight(kpiUpperLimitId, true)
      isNotANumber = true;
    }
    if (isNanCheck(+weightFactorValue)) {
      Table.setCellFontColorWeight(weightFactorId, true)
      isNotANumber = true;
    }

    if (isNotANumber) { continue; }

    calculatedKpiUpperLimitSum += +kpiUpperLimitValue * +weightFactorValue/100;
  }

  calculatedKpiUpperLimitSum = roundAndFormatValue(calculatedKpiUpperLimitSum, 1);

  Table.setValue(Table.kpiUpperLimitSumId, calculatedKpiUpperLimitSum);

  if (+calculatedKpiUpperLimitSum < 114) {
    Table.setCellFontColorWeight(Table.kpiUpperLimitSumId, true)
    return;
  } else {
    Table.setCellFontColorWeight(Table.kpiUpperLimitSumId, false)
  }

  return;
}


function calculateKpiSum() {
  let calculatedKpiSum = 0;
  for (let row = 0; row < Table.totalRows; row++) {
    const kpiValue = Table.getValue(Table.kpi, row);
    const weightFactorValue = Table.getValue(Table.weightFactor, row);

    if (isNanCheck(+kpiValue)) {
      continue;
    }
    if (isNanCheck(+weightFactorValue)) {
      continue;
    }

    calculatedKpiSum += +kpiValue * +weightFactorValue / 100;
  }

  calculatedKpiSum = roundAndFormatValue(calculatedKpiSum, 2);
  if (+calculatedKpiSum === 0) {
    Table.setValue(Table.kpiSumId, "");
  } else {
    Table.setValue(Table.kpiSumId, calculatedKpiSum);
  }

  return;
}

// ---------- Business logic: content-advanced -----------

function checkProposalForBonusPaymentState() {

  const isProposalForBonusYes = document.getElementById("js-IsProposalForBonusPaymentYes")
  const isProposalForBonusNo = document.getElementById("js-IsProposalForBonusPaymentNo")

  // if objectives aren't frozen
  const areObjectivesFrozenValue = Table.getValueById("js-areObjectivesFrozen");
  const areResultsFrozenValue = Table.getValueById("js-areResultsFrozen");
  if (areObjectivesFrozenValue === "False") {

    disableElement(isProposalForBonusYes);
    disableElement(isProposalForBonusNo);
    isProposalForBonusYes.checked = false;
    isProposalForBonusNo.checked = true;
    return;
  }

  // if results are already frozen
  if (areResultsFrozenValue === "True") {

    disableElement(isProposalForBonusYes);
    disableElement(isProposalForBonusNo);
    return;
  }

  // if kpi summary is NaN or less than 100
  const kpiSumValue = Table.getValueById(Table.kpiSumId);
  if (isNanCheck(+kpiSumValue) || +kpiSumValue < 100) {

    disableElement(isProposalForBonusYes);
    disableElement(isProposalForBonusNo);
    isProposalForBonusYes.checked = false;
    isProposalForBonusNo.checked = true;
    return;
  }

  // if there are at least one KO value in keyCheck
  for (let row = 0; row < Table.totalRows; row++) {
    const keyCheckValue = Table.getValue(Table.keyCheck, row);
    if (keyCheckValue === "KO") {

      disableElement(isProposalForBonusYes);
      disableElement(isProposalForBonusNo);
      isProposalForBonusYes.checked = false;
      isProposalForBonusNo.checked = true;
      return;
    }
  }

  enableElement(isProposalForBonusYes);
  enableElement(isProposalForBonusNo);
}

// ---------- Help functions --------------------

/**
 * 
 * @param {any} valueToCheck
 */
function isNanCheck(valueToCheck) {
  if (valueToCheck !== valueToCheck) {
    return true;
  } else {
    return false;
  }
}


function enableElement(element) {
  if (element.disabled) {
    element.disabled = false;
  }
}


function disableElement(element) {
  if (!element.disabled) {
    element.disabled = true;
  }
}


/**
 * 
 * @param {any} value
 * @param {any} digits
 */
function roundAndFormatValue(value, digits) {
  return (Math.round(value * 100) / 100).toFixed(digits);
}
