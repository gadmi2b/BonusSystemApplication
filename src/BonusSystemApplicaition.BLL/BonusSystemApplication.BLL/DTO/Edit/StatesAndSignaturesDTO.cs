namespace BonusSystemApplication.BLL.DTO.Edit
{
    public class StatesAndSignaturesDTO
    {
        public bool AreObjectivesFrozen { get; set; }
        public bool AreResultsFrozen { get; set; }
        public SignaturesDTO? SignaturesDTO { get; set; } = null;
    }
}
