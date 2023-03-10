namespace BonusSystemApplication.BLL.DTO.Edit
{
    public class FormDTO
    {
        public long Id { get; set; }
        public bool IsObjectivesFreezed { get; set; }
        public bool IsResultsFreezed { get; set; }
        public DateTime LastSavedDateTime { get; set; }
        public string LastSavedBy { get; set; } = string.Empty;

        public DefinitionDTO? Definition { get; set; } = null;
        public ConclusionDTO? Conclusion { get; set; } = null;
        public SignaturesDTO? Signatures { get; set; } = null;
        public List<ObjectiveResultDTO>? ObjectivesResults { get; set; } = null;
    }
}
