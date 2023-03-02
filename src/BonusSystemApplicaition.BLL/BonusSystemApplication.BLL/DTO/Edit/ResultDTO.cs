using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.BLL.DTO.Edit
{
    public class ResultDTO
    {
        public string KeyCheck { get; set; } = string.Empty;
        public string Achieved { get; set; } = string.Empty;
        public string Kpi { get; set; } = string.Empty;

        //public ResultDTO(Result source)
        //{
        //    KeyCheck = source.KeyCheck == null ? string.Empty : source.KeyCheck;
        //    Achieved = source.Achieved == null ? string.Empty : source.Achieved;
        //    Kpi = source.Kpi == null ? string.Empty : source.Kpi;
        //}
    }
}
