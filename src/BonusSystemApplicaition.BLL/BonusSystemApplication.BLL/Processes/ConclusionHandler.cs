using BonusSystemApplication.BLL.DTO.Edit;

namespace BonusSystemApplication.BLL.Processes
{
    internal class ConclusionHandler
    {
        private ConclusionDTO _conclusionDTO { get; set; }
        private KeyChecksHandler _keyChecksHandler { get; set; }
        private List<ObjectiveResultDTO> _objectiveResultDTOs { get; set; }

        public ConclusionHandler(ConclusionDTO conclusionDTO,
                                 List<ObjectiveResultDTO> objectiveResultDTOs)
        {
            _conclusionDTO = conclusionDTO;
            _objectiveResultDTOs = objectiveResultDTOs;
            _keyChecksHandler = new KeyChecksHandler();
        }

        /// <summary>
        /// Calculates Overall Kpi and Proposal for bonus payment
        /// based on incoming from Presentation layer data
        /// during Create and Update form processes
        /// </summary>
        public void HandleUpdateProcess()
        {
            CalculateOverallKpi();
            CalculateProposalForBonusPayment();
        }


        private void CalculateOverallKpi()
        {
            double? overallKpi = null;
            foreach (ObjectiveResultDTO orDTO in _objectiveResultDTOs)
            {
                if (double.TryParse(orDTO.Result.Kpi, out double kpi) &&
                    double.TryParse(orDTO.Objective.WeightFactor, out double weightFactor))
                {
                    overallKpi = overallKpi == null ? kpi * weightFactor / 100
                                                    : kpi * weightFactor / 100 + overallKpi;
                }
            }
            _conclusionDTO.OverallKpi = overallKpi;
        }
        private void CalculateProposalForBonusPayment()
        {
            /// Logic description:
            /// bonus will not be paid if overalkpi is less than 100
            /// bonus will not be paid if at least one keyCheck of any objective is KO

            if (_conclusionDTO.OverallKpi == null ||
                _conclusionDTO.OverallKpi < 100)
            {
                _conclusionDTO.IsProposalForBonusPayment = false;
                return;
            }

            foreach (ObjectiveResultDTO orDTO in _objectiveResultDTOs)
            {
                if (string.IsNullOrEmpty(orDTO.Result.KeyCheck))
                    continue;

                if (orDTO.Result.KeyCheck == _keyChecksHandler.GetKeyCheck(KeyChecks.KeyCheckKO))
                {
                    _conclusionDTO.IsProposalForBonusPayment = false;
                    return;
                }
            }

            _conclusionDTO.IsProposalForBonusPayment = true;
        }
    }
}
