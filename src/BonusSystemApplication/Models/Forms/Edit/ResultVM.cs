using BonusSystemApplication.BLL.DTO.Edit;

namespace BonusSystemApplication.Models.Forms.Edit
{
    public class ResultVM
    {
        public string KeyCheck { get; set; } = string.Empty;
        public string Achieved { get; set; } = string.Empty;
        public string Kpi { get; set; } = string.Empty;

        public ResultVM(ResultDTO source)
        {
            KeyCheck = source.KeyCheck;
            Achieved = source.Achieved;
            Kpi = source.Kpi;
        }
    }
}
