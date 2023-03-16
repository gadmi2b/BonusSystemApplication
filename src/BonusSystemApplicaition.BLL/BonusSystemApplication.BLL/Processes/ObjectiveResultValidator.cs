using AutoMapper;
using BonusSystemApplication.BLL.DTO.Edit;
using BonusSystemApplication.BLL.Infrastructure;
using BonusSystemApplication.DAL.Interfaces;

namespace BonusSystemApplication.BLL.Processes
{
    internal class ObjectiveResultValidator
    {
        private int _minRequiredObjectivesResultsQuantity { get; } = 7;
        private double _minWeightFactor { get; } = 0;
        private double _maxWeightFactor { get; } = 40;
        private double _minKpiUpperLimit { get; } = 108;
        private double _maxKpiUpperLimitMeasurable { get; } = 120;
        private double _maxKpiUpperLimitNonMeasurable { get; } = 115;
        private double _requiredSumWeighFactor { get; } = 100;
        private double _minSumKpiUpperLimit { get; } = 114;

        private string _stdUnit { get; } = "KPI Value, %";
        private string _stdTarget { get; } = "N/A";
        private string _stdThreshold { get; } = "N/A";
        private string _stdChallenge { get; } = "N/A";


        private long _formId { get; set; }
        private IMapper _mapper { get; set; }
        private List<ObjectiveResultDTO> _objectiveResultDTOs { get; set; }
        private IObjectiveResultRepository _objectiveResultRepository { get; set; }

        public ObjectiveResultValidator(long formId,
                                        IMapper mapper,
                                        IObjectiveResultRepository objectiveResultRepo)
        {
            _formId = formId;
            _mapper = mapper;
            _objectiveResultRepository = objectiveResultRepo;
        }

        public void ValidateObjectivesCreateProcess()
        {
            // no business rules;
        }
        public void ValidateObjectivesUpdateProcess()
        {
            // no business rules;
        }
        public void ValidateObjectivesChangeStateProcess()
        {
            List<ObjectiveResultDTO> objectiveResultDTOs = new List<ObjectiveResultDTO>();
            try
            {
                objectiveResultDTOs = _mapper
                    .Map<List<ObjectiveResultDTO>>(_objectiveResultRepository.GetObjectivesResults(_formId));
            }
            catch
            {
                throw;
            }

            PrepareConversionStringsToDouble(ref objectiveResultDTOs);

            int objectivesCounter = 0;
            double sumWeightFactor = 0;
            double sumKpiUpperLimit = 0;
            foreach (ObjectiveResultDTO or in objectiveResultDTOs)
            {
                #region Objectives checking
                if (string.IsNullOrEmpty(or.Objective.Statement) ||
                    string.IsNullOrEmpty(or.Objective.Description))
                {
                    continue;
                }

                // Unit is filled
                if (string.IsNullOrEmpty(or.Objective.Unit))
                    throw new ValidationException($"The form's state could't be changed. " +
                                                  $"There is objective in row {or.Row} without specified Unit",
                                                  $"{nameof(or.Objective.Unit)}");
                // WeightFactor is number
                if (!Double.TryParse(or.Objective.WeightFactor, out double weightFactor))
                    throw new ValidationException($"The form's state could't be changed. " +
                                                  $"Row: {or.Row}. {nameof(or.Objective.WeightFactor)} must be a number.",
                                                  $"{nameof(or.Objective.WeightFactor)}");
                // WeightFactor is limited
                if (weightFactor < _minWeightFactor ||
                    weightFactor > _maxWeightFactor)
                    throw new ValidationException($"The form's state could't be changed. " +
                                                  $"Row: {or.Row}. {nameof(or.Objective.WeightFactor)} must be between: " +
                                                  $"{_minWeightFactor} and {_maxWeightFactor}",
                                                  $"{nameof(or.Objective.WeightFactor)}");
                // KpiUpperLimit is number
                if (!Double.TryParse(or.Objective.KpiUpperLimit, out double kpiUpperLimit))
                    throw new ValidationException($"The form's state could't be changed. " +
                                                  $"Row: {or.Row}. {nameof(or.Objective.KpiUpperLimit)} must be a number.",
                                                  $"{nameof(or.Objective.KpiUpperLimit)}");
                if (or.Objective.IsMeasurable)
                {
                    // Threshold is number
                    if (!Double.TryParse(or.Objective.Threshold, out double threshold))
                        throw new ValidationException($"The form's state could't be changed. " +
                                                      $"Row: {or.Row}. {nameof(or.Objective.Threshold)} must be a number.",
                                                      $"{nameof(or.Objective.Unit)}");
                    // Target is number
                    if (!Double.TryParse(or.Objective.Target, out double target))
                        throw new ValidationException($"The form's state could't be changed. " +
                                                      $"Row: {or.Row}. {nameof(or.Objective.Target)} must be a number.",
                                                      $"{nameof(or.Objective.Unit)}");
                    // Challenge is number
                    if (!Double.TryParse(or.Objective.Challenge, out double challenge))
                        throw new ValidationException($"The form's state could't be changed. " +
                                                      $"Row: {or.Row}. {nameof(or.Objective.Challenge)} must be a number.",
                                                      $"{nameof(or.Objective.Unit)}");
                    // KpiUpperLimit is limited
                    if (kpiUpperLimit < _minKpiUpperLimit ||
                        kpiUpperLimit > _maxKpiUpperLimitMeasurable)
                        throw new ValidationException($"The form's state could't be changed. " +
                                                      $"Row: {or.Row}. {nameof(or.Objective.KpiUpperLimit)} must be between: " +
                                                      $"{_minKpiUpperLimit} and {_maxKpiUpperLimitMeasurable}",
                                                      $"{nameof(or.Objective.KpiUpperLimit)}");

                    // Data correctness check (monotonicity of sequence Threshold-Target-Challenge)
                    if (target >= Math.Max(threshold, challenge) ||
                        target <= Math.Min(threshold, challenge))
                        throw new ValidationException($"The form's state could't be changed. " +
                                                      $"Row: {or.Row}. {nameof(or.Objective.Target)} must be between: " +
                                                      $"{or.Objective.Threshold} and {or.Objective.Challenge}",
                                                      $"{nameof(or.Objective.Target)}");
                }
                else
                {
                    // KpiUpperLimit is limited
                    if (kpiUpperLimit < _minKpiUpperLimit ||
                        kpiUpperLimit > _maxKpiUpperLimitNonMeasurable)
                        throw new ValidationException($"The form's state could't be changed. " +
                                                      $"Row: {or.Row}. {nameof(or.Objective.KpiUpperLimit)} must be between: " +
                                                      $"{_minKpiUpperLimit} and {_maxKpiUpperLimitNonMeasurable}",
                                                      $"{nameof(or.Objective.KpiUpperLimit)}");
                    if (or.Objective.IsKey)
                    {
                        // Threshold is number
                        if (!Double.TryParse(or.Objective.Threshold, out double threshold))
                            throw new ValidationException($"The form's state could't be changed. " +
                                                          $"Row: {or.Row}. {nameof(or.Objective.Threshold)} must be a number.",
                                                          $"{nameof(or.Objective.Unit)}");
                    }
                }
                objectivesCounter++;

                sumWeightFactor = sumWeightFactor + weightFactor;
                sumKpiUpperLimit = sumKpiUpperLimit + kpiUpperLimit;
                #endregion

                #region Results checking
                // Achieved is number
                if (!Double.TryParse(or.Result.Achieved, out double achieved))
                    throw new ValidationException($"The form's state could't be changed. " +
                                                  $"Row: {or.Row}. {nameof(or.Result.Achieved)} must be a number.",
                                                  $"{nameof(or.Result.Achieved)}");
                // Kpi is number
                if (!Double.TryParse(or.Result.Kpi, out double kpi))
                    throw new ValidationException($"The form's state could't be changed. " +
                                                  $"Row: {or.Row}. {nameof(or.Result.Kpi)} must be a number.",
                                                  $"{nameof(or.Result.Kpi)}");

                #endregion
            }
            // Minimum objectives should be filled (contain statements and definition)
            if (objectivesCounter < _minRequiredObjectivesResultsQuantity)
                throw new ValidationException($"The form's state could't be changed. " +
                                              $"At least {_minRequiredObjectivesResultsQuantity} objecties" +
                                              $"should contain Statement and Description.", "");

            // Sum of all weight factors should be equal to _requiredSumWeighFactor
            if (sumWeightFactor != _requiredSumWeighFactor)
                throw new ValidationException($"The form's state could't be changed. " +
                                              $"Current sum of weight factors ({sumWeightFactor}) " +
                                              $"is not equal to {_requiredSumWeighFactor}", "");

            // Sum of all kpi upper limits should be greated than _minSumKpiUpperLimit
            if (sumKpiUpperLimit < _minSumKpiUpperLimit)
                throw new ValidationException($"The form's state could't be changed. " +
                                              $"Current sum of Kpi upper limits ({sumKpiUpperLimit}) " +
                                              $"is less than {_minSumKpiUpperLimit}", "");
        }

        public void ValidateResultsCreateProcess()
        {

        }
        public void ValidateResultsUpdateProcess()
        {

        }
        public void ValidateResultsChangeStateProcess()
        {

        }
    

        /// <summary>
        /// Checks inside objectives and results all string values which should be converted to double and
        /// if only single comma deliminator persists in value - replaces it by point deliminator.
        /// </summary>
        /// <param name="objectiveResultDTOs"></param>
        private void PrepareConversionStringsToDouble(ref List<ObjectiveResultDTO> objectiveResultDTOs)
        {
            for (int i = 0; i < objectiveResultDTOs.Count; i++)
            {
                if (objectiveResultDTOs[i].Objective.IsMeasurable)
                {
                    objectiveResultDTOs[i].Objective.Threshold = PrepareString(objectiveResultDTOs[i].Objective.Threshold);
                    objectiveResultDTOs[i].Objective.Target = PrepareString(objectiveResultDTOs[i].Objective.Target);
                    objectiveResultDTOs[i].Objective.Challenge = PrepareString(objectiveResultDTOs[i].Objective.Challenge);
                }
                else if (objectiveResultDTOs[i].Objective.IsKey)
                {
                    objectiveResultDTOs[i].Objective.Threshold = PrepareString(objectiveResultDTOs[i].Objective.Threshold);
                }

                objectiveResultDTOs[i].Objective.KpiUpperLimit = PrepareString(objectiveResultDTOs[i].Objective.KpiUpperLimit);
                objectiveResultDTOs[i].Objective.WeightFactor = PrepareString(objectiveResultDTOs[i].Objective.WeightFactor);

                objectiveResultDTOs[i].Result.Achieved = PrepareString(objectiveResultDTOs[i].Result.Achieved);
                objectiveResultDTOs[i].Result.Kpi = PrepareString(objectiveResultDTOs[i].Result.Kpi);
            }
        }
        private string? PrepareString(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            if (value == "N/A")
                return null;

            string[] parts = value.Split(',');
            if (parts.Length != 2)
                return value;

            return $"{parts[0]}.{parts[1]}";
        }
    }
}
