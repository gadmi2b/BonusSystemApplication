using BonusSystemApplication.BLL.DTO.Edit;

namespace BonusSystemApplication.Models.Forms.Edit
{
    public class ObjectiveResultVM
    {
        public long Id { get; set; }
        public int Row { get; set; }
        public ObjectiveVM Objective { get; set; }
        public ResultVM Result { get; set; }

        public ObjectiveResultVM(ObjectiveResultDTO source)
        {
            Id = source.Id;
            Row = source.Row;
            Objective = new ObjectiveVM(source.Objective);
            Result = new ResultVM(source.Result);
        }
    }
}
