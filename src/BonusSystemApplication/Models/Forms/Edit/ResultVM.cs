using BonusSystemApplication.BLL.DTO.Edit;

namespace BonusSystemApplication.Models.Forms.Edit
{
    public class ResultVM
    {
        public string? KeyCheck { get; set; } = null;
        public string? Achieved { get; set; } = null;
        public string? Kpi { get; set; } = null;

        public ResultVM() { }
    }
}
