using AutoMapper;
using BonusSystemApplication.BLL.DTO.Edit;
using BonusSystemApplication.BLL.Infrastructure;
using BonusSystemApplication.DAL.Interfaces;

namespace BonusSystemApplication.BLL.Processes
{
    internal class ObjectiveResultValidator
    {
        private int _minRequiredObjectivesResultsQuantity { get; } = 7;
        private int _minWeightFactor { get; } = 0;
        private int _maxWeightFactor { get; } = 40;
        private int _minKpiUpperLimit { get; } = 108;
        private int _maxKpiUpperLimitMeasurable { get; } = 120;
        private int _maxKpiUpperLimitNonMeasurable { get; } = 115;
        private int _requiredSumWeighFactor { get; } = 100;
        private int _minSumKpiUpperLimit { get; } = 114;

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

            int objectivesCounter = 0;
            int sumWeightFactor = 0;
            int sumKpiUpperLimit = 0;
            foreach (ObjectiveResultDTO or in objectiveResultDTOs)
            {
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
                if (!Int32.TryParse(or.Objective.WeightFactor, out int weightFactor))
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
                if (!Int32.TryParse(or.Objective.KpiUpperLimit, out int kpiUpperLimit))
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
    }
}
