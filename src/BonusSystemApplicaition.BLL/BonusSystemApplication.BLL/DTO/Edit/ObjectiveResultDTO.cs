using BonusSystemApplication.DAL.Entities;

namespace BonusSystemApplication.BLL.DTO.Edit
{
    public class ObjectiveResultDTO
    {
        public long Id { get; set; }
        public int Row { get; set; }
        public ObjectiveDTO Objective { get; set; }
        public ResultDTO Result { get; set; }

        //public ObjectiveResultDTO(ObjectiveResult source)
        //{
        //    Id = source.Id;
        //    Row = source.Row;
        //    Objective = new ObjectiveDTO(source.Objective);
        //    Result = new ResultDTO(source.Result);
        //}
    }
}
