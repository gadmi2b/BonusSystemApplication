using BonusSystemApplication.BLL.DTO.Edit;
using BonusSystemApplication.BLL.Infrastructure;
using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.BLL.Processes
{
    internal class ObjectivesResultsHandler
    {
        private int _maxObjectivesResults { get; } = 10;
        private int _minRequiredObjectivesResultsQuantity { get; } = 7;
        private double _minWeightFactor { get; } = 0;
        private double _maxWeightFactor { get; } = 40;
        private double _minKpiUpperLimit { get; } = 108;
        private double _maxKpiUpperLimitMeasurable { get; } = 120;
        private double _maxKpiUpperLimitNonMeasurable { get; } = 115;
        private double _requiredSumWeighFactor { get; } = 100;
        private double _minSumKpiUpperLimit { get; } = 114;

        private KeyChecksHandler _keyChecksHandler { get; set; }
        private List<ObjectiveResult> _objectiveResults { get; set; }
        private List<ObjectiveResultDTO> _objectiveResultDTOs { get; set; }

        public ObjectivesResultsHandler()
        {
            _keyChecksHandler = new KeyChecksHandler();
        }


        /// <summary>
        /// Handles Objectives and Results incoming from Presentation layer
        /// during Create and Update form processes
        /// </summary>
        /// <param name="objectiveResultDTOs"></param>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void HandleObjectivesUpdateProcess(List<ObjectiveResultDTO> objectiveResultDTOs)
        {
            ArgumentNullException.ThrowIfNull(objectiveResultDTOs, nameof(objectiveResultDTOs));
            _objectiveResultDTOs = objectiveResultDTOs;

            /// Logic description:
            /// checks the [objectives] before they will be updated:
            /// most values should be parseable to double? type

            PrepareCastStringsToDouble();

            int objectivesCounter = 0;
            foreach (ObjectiveResultDTO or in _objectiveResultDTOs)
            {
                double? kpiUpperLimit = 0;
                double? weightFactor = 0;
                double? threshold = 0;
                double? target = 0;
                double? challenge = 0;
                double? achieved = 0;
                double? kpi = 0;

                #region Objectives checking
                objectivesCounter++;

                if (objectivesCounter > _maxObjectivesResults)
                    throw new ValidationException($"Unable to update form. " +
                                                  $"To many objectives were filled. " +
                                                  $"Maximum allowable number of objectives are: {_maxObjectivesResults}");

                threshold = TryGetNullableDoubleFromNullableString(or.Objective.Threshold, or.Row, nameof(or.Objective.Threshold));
                target = TryGetNullableDoubleFromNullableString(or.Objective.Target, or.Row, nameof(or.Objective.Target));
                challenge = TryGetNullableDoubleFromNullableString(or.Objective.Challenge, or.Row, nameof(or.Objective.Challenge));

                weightFactor = TryGetNullableDoubleFromNullableString(or.Objective.WeightFactor, or.Row, nameof(or.Objective.WeightFactor));
                kpiUpperLimit = TryGetNullableDoubleFromNullableString(or.Objective.KpiUpperLimit, or.Row, nameof(or.Objective.KpiUpperLimit));

                if (weightFactor != null)
                    MustBeLimited(weightFactor, _minWeightFactor, _maxWeightFactor,
                                    or.Row, nameof(or.Objective.WeightFactor));

                if (or.Objective.IsMeasurable)
                {
                    if (kpiUpperLimit != null)
                        MustBeLimited(kpiUpperLimit, _minKpiUpperLimit, _maxKpiUpperLimitMeasurable,
                                        or.Row, nameof(or.Objective.KpiUpperLimit));

                    if (threshold != null && target != null && challenge != null)
                        MustBeMonotonicSequence(threshold, target, challenge,
                                                or.Row, nameof(or.Objective.Target));
                }
                else
                {
                    if (!or.Objective.IsKey)
                        MustBeNullOrEmpty(threshold, or.Row, nameof(or.Objective.Threshold));

                    MustBeNullOrEmpty(target, or.Row, nameof(or.Objective.Target));
                    MustBeNullOrEmpty(challenge, or.Row, nameof(or.Objective.Challenge));

                    if (kpiUpperLimit != null)
                        MustBeLimited(kpiUpperLimit, _minKpiUpperLimit, _maxKpiUpperLimitNonMeasurable,
                                        or.Row, nameof(or.Objective.KpiUpperLimit));
                }
                #endregion

                #region Results checking / calculation
                achieved = TryGetNullableDoubleFromNullableString(or.Result.Achieved, or.Row, nameof(or.Result.Achieved));

                or.Result.KeyCheck = KeyCheckCalculation(or.Objective.IsMeasurable,
                                                         or.Objective.IsKey,
                                                         threshold,
                                                         target,
                                                         challenge,
                                                         achieved);

                kpi = KpiCalculation(or.Objective.IsMeasurable,
                                     kpiUpperLimit,
                                     threshold,
                                     target,
                                     challenge,
                                     achieved);
                or.Result.Kpi = CastNullableStringFromNullableDouble(kpi);
                #endregion
            }
        }
        /// <summary>
        /// Handles Results incoming from Presentation layer
        /// and Objectives stored in Database
        /// during Update form process
        /// </summary>
        /// <param name="objectiveResults"></param>
        /// <param name="objectiveResultDTOs"></param>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void HandleResultsUpdateProcess(List<ObjectiveResult> objectiveResults,
                                               List<ObjectiveResultDTO> objectiveResultDTOs)
        {
            ArgumentNullException.ThrowIfNull(objectiveResultDTOs, nameof(objectiveResultDTOs));
            ArgumentNullException.ThrowIfNull(objectiveResults, nameof(objectiveResults));
            _objectiveResultDTOs = objectiveResultDTOs;
            _objectiveResults = objectiveResults;

            PrepareCastStringsToDouble();

            for (int index = 0; index < _objectiveResults.Count; index++)
            {
                if (string.IsNullOrEmpty(_objectiveResults[index].Objective.Statement) ||
                    string.IsNullOrEmpty(_objectiveResults[index].Objective.Description))
                {
                    continue;
                }

                string? keyCheck = null;
                double? achieved = 0;
                double? kpi = 0;

                achieved = TryGetNullableDoubleFromNullableString(_objectiveResultDTOs[index].Result.Achieved,
                                                                  _objectiveResultDTOs[index].Row,
                                                                  nameof(ObjectiveResultDTO.Result.Achieved));

                keyCheck = KeyCheckCalculation(_objectiveResults[index].Objective.IsMeasurable,
                                               _objectiveResults[index].Objective.IsKey,
                                               _objectiveResults[index].Objective.Threshold,
                                               _objectiveResults[index].Objective.Target,
                                               _objectiveResults[index].Objective.Challenge,
                                               achieved);

                kpi = KpiCalculation(_objectiveResults[index].Objective.IsMeasurable,
                                     _objectiveResults[index].Objective.KpiUpperLimit,
                                     _objectiveResults[index].Objective.Threshold,
                                     _objectiveResults[index].Objective.Target,
                                     _objectiveResults[index].Objective.Challenge,
                                     achieved);

                _objectiveResultDTOs[index].Result.KeyCheck = keyCheck;
                _objectiveResultDTOs[index].Result.Kpi = kpi.ToString();
            }
        }

        /// <summary>
        /// Validates Objectives stored in DAL
        /// during Objectives freezing process
        /// </summary>
        /// <param name="objectiveResults"></param>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void ValidateObjectivesChangeStateProcess(List<ObjectiveResult> objectiveResults)
        {
            ArgumentNullException.ThrowIfNull(objectiveResults, nameof(objectiveResults));
            _objectiveResults = objectiveResults;

            /// Logic description:
            /// checks the [objectives] before they will be frozen:
            /// the rows should be correctly filled or fully empty

            int objectivesCounter = 0;
            double? sumWeightFactor = null;
            double? sumKpiUpperLimit = null;
            foreach (ObjectiveResult or in _objectiveResults)
            {
                bool isShouldBeFilled = false;
                #region Is current row should be filled or fully empty
                /// Logic description:
                /// if one of two: statement or description was filled
                /// but another one was not => throw an exception.
                /// both filled or both not empty => ok, but different logic
                if (!string.IsNullOrEmpty(or.Objective.Statement))
                {
                    if (string.IsNullOrEmpty(or.Objective.Description))
                    {
                        throw new ValidationException($"Unable to update form. " +
                                                      $"There is objective in row {or.Row} without specified {nameof(or.Objective.Description)}",
                                                      $"{nameof(or.Objective.Description)}");
                    }
                    else // both statement or description are filled
                    {
                        objectivesCounter++;
                        isShouldBeFilled = true;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(or.Objective.Description))
                    {
                        throw new ValidationException($"Unable to update form. " +
                                                      $"There is objective in row {or.Row} without specified {nameof(or.Objective.Statement)}",
                                                      $"{nameof(or.Objective.Statement)}");
                    }
                    else // both statement or description are not filled
                    {
                        // do nothing
                    }
                }
                #endregion
                
                if (objectivesCounter > _maxObjectivesResults)
                    throw new ValidationException($"Unable to update form. " +
                                                  $"To many objectives were filled. " +
                                                  $"Maximum allowable number of objectives are: {_maxObjectivesResults}");
                #region Check if row correctly filled or empty
                if (isShouldBeFilled)
                {
                    MustNotBeNullOrEmpty(or.Objective.Unit, or.Row, nameof(or.Objective.Unit));
                    MustNotBeNullOrEmpty(or.Objective.WeightFactor, or.Row, nameof(or.Objective.WeightFactor));
                    MustNotBeNullOrEmpty(or.Objective.KpiUpperLimit, or.Row, nameof(or.Objective.KpiUpperLimit));

                    MustBeLimited(or.Objective.WeightFactor, _minWeightFactor, _maxWeightFactor,
                                    or.Row, nameof(or.Objective.WeightFactor));

                    if (or.Objective.IsMeasurable)
                    {
                        MustNotBeNullOrEmpty(or.Objective.Threshold, or.Row, nameof(or.Objective.Threshold));
                        MustNotBeNullOrEmpty(or.Objective.Target, or.Row, nameof(or.Objective.Target));
                        MustNotBeNullOrEmpty(or.Objective.Challenge, or.Row, nameof(or.Objective.Challenge));

                        MustBeLimited(or.Objective.KpiUpperLimit, _minKpiUpperLimit, _maxKpiUpperLimitMeasurable,
                                        or.Row, nameof(or.Objective.KpiUpperLimit));

                        MustBeMonotonicSequence(or.Objective.Threshold, or.Objective.Target, or.Objective.Challenge,
                                                or.Row, nameof(or.Objective.Target));
                    }
                    else
                    {
                        MustBeNullOrEmpty(or.Objective.Target, or.Row, nameof(or.Objective.Target));
                        MustBeNullOrEmpty(or.Objective.Challenge, or.Row, nameof(or.Objective.Challenge));

                        if (or.Objective.IsKey)
                            MustNotBeNullOrEmpty(or.Objective.Threshold, or.Row, nameof(or.Objective.Threshold));
                        else
                            MustBeNullOrEmpty(or.Objective.Threshold, or.Row, nameof(or.Objective.Threshold));

                        MustBeLimited(or.Objective.KpiUpperLimit, _minKpiUpperLimit, _maxKpiUpperLimitNonMeasurable,
                                        or.Row, nameof(or.Objective.KpiUpperLimit));
                    }
                }
                else
                {
                    MustBeNullOrEmpty(or.Objective.Unit, or.Row, nameof(or.Objective.Unit));
                    MustBeNullOrEmpty(or.Objective.Threshold, or.Row, nameof(or.Objective.Threshold));
                    MustBeNullOrEmpty(or.Objective.Target, or.Row, nameof(or.Objective.Target));
                    MustBeNullOrEmpty(or.Objective.Challenge, or.Row, nameof(or.Objective.Challenge));
                    MustBeNullOrEmpty(or.Objective.WeightFactor, or.Row, nameof(or.Objective.WeightFactor));
                    MustBeNullOrEmpty(or.Objective.KpiUpperLimit, or.Row, nameof(or.Objective.KpiUpperLimit));
                }
                #endregion

                sumWeightFactor = CountNullableValuesSum(sumWeightFactor, or.Objective.WeightFactor);
                sumKpiUpperLimit = CountNullableValuesSum(sumKpiUpperLimit, or.Objective.KpiUpperLimit);
            }

            #region Check sum values
            MustBeGreaterOrEqualTo(objectivesCounter, _minRequiredObjectivesResultsQuantity, "number of filled objectives");
            MustBeGreaterOrEqualTo(sumKpiUpperLimit, _minSumKpiUpperLimit, "sum of Kpi upper limits");
            MustBeEqualTo(sumWeightFactor, _requiredSumWeighFactor, "sum of weight factors");
            #endregion
        }
        /// <summary>
        /// Validates Results stored in DAL
        /// during Results freezing process
        /// </summary>
        /// <param name="objectiveResults"></param>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void ValidateResultsChangeStateProcess(List<ObjectiveResult> objectiveResults)
        {
            ArgumentNullException.ThrowIfNull(objectiveResults, nameof(objectiveResults));
            _objectiveResults = objectiveResults;

            foreach (ObjectiveResult or in _objectiveResults)
            {
                if (string.IsNullOrEmpty(or.Objective.Statement) ||
                    string.IsNullOrEmpty(or.Objective.Description))
                {
                    MustBeNullOrEmpty(or.Result.Achieved, or.Row, nameof(or.Result.Achieved));
                }
                else
                {
                    MustNotBeNullOrEmpty(or.Result.Achieved, or.Row, nameof(or.Result.Achieved));
                }
            }
        }


        /// <summary>
        /// Checks all string values inside objectives and results
        /// which should be converted to double? and
        /// replaces empty and KeyCheckNA string representation by null
        /// </summary>
        private void PrepareCastStringsToDouble()
        {
            for (int i = 0; i < _objectiveResultDTOs.Count; i++)
            {
                _objectiveResultDTOs[i].Objective.Threshold = PrepareString(_objectiveResultDTOs[i].Objective.Threshold);
                _objectiveResultDTOs[i].Objective.Target = PrepareString(_objectiveResultDTOs[i].Objective.Target);
                _objectiveResultDTOs[i].Objective.Challenge = PrepareString(_objectiveResultDTOs[i].Objective.Challenge);

                _objectiveResultDTOs[i].Objective.KpiUpperLimit = PrepareString(_objectiveResultDTOs[i].Objective.KpiUpperLimit);
                _objectiveResultDTOs[i].Objective.WeightFactor = PrepareString(_objectiveResultDTOs[i].Objective.WeightFactor);

                _objectiveResultDTOs[i].Result.Achieved = PrepareString(_objectiveResultDTOs[i].Result.Achieved);
            }

            string? PrepareString(string? value)
            {
                if (string.IsNullOrEmpty(value))
                    return null;

                if (value == _keyChecksHandler.GetKeyCheck(KeyChecks.KeyCheckNA))
                    return null;

                return value;
            }
        }

        private double? TryGetNullableDoubleFromNullableString(string? property, int row, string propertyName)
        {
            if (property == null)
                return null;

            if (double.TryParse(property, out double temp))
                return temp;

            throw new ValidationException($"Unable to update form. " +
                                          $"Row: {row}. {propertyName} must be a number.",
                                          $"{propertyName}");
        }
        private string? CastNullableStringFromNullableDouble(double? property)
        {
            if (property == null)
                return null;

            return property.ToString();
        }
        
        private double? CountNullableValuesSum(double? sumValue, double? addedValue)
        {
            // To avoid cases when [any value + null = null]
            if (addedValue != null)
            {
                return sumValue != null ? sumValue + addedValue
                                        : addedValue;
            }
            return sumValue;
        }
        private string? KeyCheckCalculation(bool isMeasurable,
                                            bool isKey,
                                            double? threshold,
                                            double? target,
                                            double? challenge,
                                            double? achieved)
        {
            if (!isKey)
                return _keyChecksHandler.GetKeyCheck(KeyChecks.KeyCheckNA);

            if (isMeasurable)
            {
                if (threshold == null ||
                    challenge == null ||
                    achieved == null ||
                    target == null)
                {
                    return _keyChecksHandler.GetKeyCheck(KeyChecks.KeyCheckError);
                }

                if (threshold < challenge)
                    if (achieved >= threshold)
                        return _keyChecksHandler.GetKeyCheck(KeyChecks.KeyCheckOK);
                else
                    if (achieved <= threshold)
                        return _keyChecksHandler.GetKeyCheck(KeyChecks.KeyCheckOK);

                return _keyChecksHandler.GetKeyCheck(KeyChecks.KeyCheckKO);
            }
            else
            {
                if (threshold == null ||
                    achieved == null)
                {
                    return _keyChecksHandler.GetKeyCheck(KeyChecks.KeyCheckError);
                }

                if (achieved >= threshold)
                    return _keyChecksHandler.GetKeyCheck(KeyChecks.KeyCheckOK);
                else
                    return _keyChecksHandler.GetKeyCheck(KeyChecks.KeyCheckKO);
            }
        }
        private double? KpiCalculation(bool isMeasurable,
                                       double? kpiUpperLimit,
                                       double? threshold,
                                       double? target,
                                       double? challenge,
                                       double? achieved)
        {
            double? kpi = null;

            if (isMeasurable)
            {
                if (kpiUpperLimit == null ||
                    threshold == null ||
                    challenge == null ||
                    achieved == null ||
                    target == null)
                {
                    return null; // [double? = null] converted by ToString() to ""
                }

                if (threshold < challenge)
                {
                    if (achieved < target)   // same as next one
                    {
                        kpi = 100 / (target - threshold) * (achieved - threshold);
                        if (kpi < 0) { kpi =  0; }
                    }
                    else
                    {
                        kpi = 100 * (1 + 0.2 / (challenge - target) * (achieved - target));
                        if (kpi > _maxKpiUpperLimitMeasurable) { kpi = _maxKpiUpperLimitMeasurable; }
                    }
                }
                else
                {
                    if (achieved > target)   // same as prev one
                    {
                        kpi = 100 / (target - threshold) * (achieved - threshold);
                        if (kpi < 0) { kpi = 0; }
                    }
                    else
                    {
                        kpi = 100 * (1 + 0.2 / (challenge - target) * (achieved - target));
                        if (kpi > _maxKpiUpperLimitMeasurable) { kpi = _maxKpiUpperLimitMeasurable; }
                    }
                }
                kpi = Math.Min((double)kpi, (double)kpiUpperLimit);
            }
            else
            {
                if (kpiUpperLimit == null ||
                    achieved == null)
                {
                    return null;  // [double? = null] converted by ToString() to ""
                }
                double minimum = Math.Min((double)achieved, (double)kpiUpperLimit);
                       minimum = Math.Min(minimum, _maxKpiUpperLimitNonMeasurable);

                kpi = Math.Max(minimum, 0);
            }

            return kpi;
        }

        private void MustBeNullOrEmpty(double? property, int row, string propertyName)
        {
            if (property != null)
                throw new ValidationException($"Unable to update form. " +
                                              $"Row: {row}. {propertyName} must be empty",
                                              $"{propertyName}");
        }
        private void MustBeNullOrEmpty(string? property, int? row, string? propertyName)
        {
            if (!string.IsNullOrEmpty(property))
                throw new ValidationException($"Unable to update form. " +
                                              $"Row: {row}. {propertyName} must be empty.",
                                              $"{propertyName}");
        }
        private void MustNotBeNullOrEmpty(double? property, int row, string propertyName)
        {
            if (property == null)
                throw new ValidationException($"Unable to update form. " +
                                              $"Row: {row}. {propertyName} must be filled.",
                                              $"{propertyName}");
        }
        private void MustNotBeNullOrEmpty(string? property, int row, string propertyName)
        {
            if (string.IsNullOrEmpty(property))
                throw new ValidationException($"Unable to update form. " +
                                              $"Row: {row}. {propertyName} must be filled.",
                                              $"{propertyName}");
        }
        private void MustBeLimited(double? property, double lowerLimit, double upperLimit, int row, string propertyName)
        {
            if (property == null ||
                property < lowerLimit ||
                property > upperLimit)
                throw new ValidationException($"Unable to update form. " +
                                              $"Row: {row}. {propertyName} must be between: " +
                                              $"{lowerLimit} and {upperLimit}",
                                              $"{propertyName}");
        }
        private void MustBeMonotonicSequence(double? threshold, double? target, double? challenge, int row, string propertyName)
        {
            if (threshold == null || target == null || challenge == null)
                throw new ValidationException($"Unable to update form. " +
                                              $"Row: {row}. Unable to check " +
                                              $"{nameof(Objective.Threshold)}-{nameof(Objective.Target)}-{nameof(Objective.Challenge)} " +
                                              $"monotonic sequence.");
            // MonotonicSequence means that all three values
            // should be successively increased or decreased
            if (target >= Math.Max((double)threshold, (double)challenge) ||
                target <= Math.Min((double)threshold, (double)challenge))
                throw new ValidationException($"Unable to update form. " +
                                              $"Row: {row}. {propertyName} must be between: " +
                                              $"{threshold} and {challenge}",
                                              $"{propertyName}");
        }
        private void MustBeGreaterOrEqualTo(double? property, double lowerLimit, string propertyName)
        {
            if (property == null ||
                property < lowerLimit)
                throw new ValidationException($"Unable to update form. " +
                                              $"Current {propertyName} ({property}) " +
                                              $"is less than {lowerLimit}");
        }
        private void MustBeEqualTo(double? property, double requiredValue, string propertyName)
        {
            if (property == null ||
                property != requiredValue)
                throw new ValidationException($"Unable to update form. " +
                                              $"Current sum of {propertyName} ({property}) " +
                                              $"is not equal to {requiredValue}");
        }
    }
}
