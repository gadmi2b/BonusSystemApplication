namespace BonusSystemApplication.BLL.DTO.Edit
{
    public class ObjectiveResultDTO
    {
        public long Id { get; set; }
        public int Row { get; set; }
        public ObjectiveDTO Objective { get; set; }
        public ResultDTO Result { get; set; }
    }
}
